using ScientistAssistant_ConsoleVersion.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace ScientistAssistant_ConsoleVersion.QueryTagLogic
{
    public interface LogicNode
    {
        bool checkObject<T>(T x, string propertyName) where T : class;
    }

    static class LogicConstructor
    {
        interface ExpressionTerm
        {

        }

        class Operation : ExpressionTerm
        {
            public char type { get; set; }

            public int priority
            {
                get
                {
                    if (type == '!') return 2;
                    if (type == '&') return 1;
                    if (type == '|') return 0;

                    return -1;
                }
            }

            public Operation() { }
            public Operation(char type)
            {
                this.type = type;
            }

            public override string ToString()
            {
                return type.ToString();
            }
        }

        class Value : ExpressionTerm
        {
            public string val { get; set; }

            public Value() { }
            public Value(string val)
            {
                this.val = val;
            }

            public override string ToString()
            {
                return val;
            }
        }
    
        class Bracket : ExpressionTerm
        {
            public char type { get; set; }

            public Bracket() { }
            public Bracket(char type)
            {
                this.type = type;
            }

            public override string ToString()
            {
                return type.ToString();
            }
        }

        class BinaryNode : LogicNode
        {
            public Operation op { get; set; }
            public LogicNode L { get; set; }
            public LogicNode R { get; set; }

            public bool checkObject<T>(T x, string propertyName) where T : class
            {
                bool lVal = L.checkObject(x, propertyName);
                bool rVal = R.checkObject(x, propertyName);

                if (op.type == '|') return (lVal | rVal);
                if (op.type == '&') return (lVal & rVal);
                return false;
            }

            public override string ToString()
            {
                string lString;
                if (L.GetType() == typeof(ValueNode)) lString = L.ToString();
                else lString = $"({L})";

                string rString;
                if (R.GetType() == typeof(ValueNode)) rString = R.ToString();
                else rString = $"({R})";

                return $"{lString}{op}{rString}";
            }
        }

        class UnaryNode : LogicNode
        {
            public bool invert { get; set; }
            public LogicNode child { get; set; }

            public bool checkObject<T>(T x, string propertyName) where T : class
            {
                return child.checkObject(x, propertyName) ^ invert;
            }

            public override string ToString()
            {
                string childString;
                if (child.GetType() == typeof(ValueNode)) childString = child.ToString();
                else childString = $"({child})";

                return $"{((invert == false) ? "" : "!")}{childString}";
            }
        }

        class ValueNode : LogicNode
        {
            public Value val { get; set; }

            public ValueNode() { }
            public ValueNode(Value val)
            {
                this.val = val;
            }

            public override string ToString()
            {
                return $"{val}";
            }

            public bool checkObject<T>(T x, string propertyName) where T : class
            {
                return checkProperty(x, propertyName, val.val);
            }

            public bool checkProperty<T>(T curr, string propertyName, string x) where T : class
            {
                List<string> candidates = new List<string>();
                if (propertyName.Contains('.') == false)
                {
                    PropertyInfo p = curr.GetType().GetProperties().FirstOrDefault(x => x.Name == propertyName);

                    if (p != null)
                    {
                        candidates.Add(p.GetValue(curr) as string);
                    }
                    else
                    {
                        throw new WrongPropertyNameException(propertyName);
                    }
                }
                else
                {
                    string[] path = propertyName.Split('.');
                    PropertyInfo p0 = curr.GetType().GetProperties().FirstOrDefault(x => x.Name == path[0]);

                    if (p0 != null)
                    {
                        IEnumerable<object> l = null;
                        try
                        {
                            l = (p0.GetValue(curr)) as IEnumerable<object>;
                        }
                        catch
                        {
                            throw new WrongPropertyNameException(propertyName);
                        }
                        
                        foreach (object item in l)
                        {
                            PropertyInfo p1 = item.GetType().GetProperties().FirstOrDefault(x => x.Name == path[1]);
                            if (p1 != null)
                            {
                                candidates.Add(p1.GetValue(item) as string);
                            }
                            else
                            {
                                throw new WrongPropertyNameException(propertyName);
                            }
                        }
                    }
                    else
                    {
                        throw new WrongPropertyNameException(propertyName);
                    }
                }

                return candidates.Contains(x);
            }
        }

        private static int getSplit(int l, int r, List <ExpressionTerm> parsed)
        {
            int ind = -1, cnt = 0;
            for(int i = l;i<=r;i++)
            {
                if(parsed[l].GetType() == typeof(Bracket))
                {
                    if ((parsed[i] as Bracket).type == '(') cnt++;
                    else cnt--;
                }

                if(cnt==0 && parsed[i].GetType()==typeof(Operation))
                {
                    if((parsed[i] as Operation).type!='!')
                    {
                        if (ind == -1) ind = i;
                        else if ((parsed[ind] as Operation).priority > (parsed[i] as Operation).priority) ind = i;
                    }
                }
            }

            return ind;
        }

        private static LogicNode rec(int l, int r, List <ExpressionTerm> parsed)
        {
            if(parsed[l].GetType()==typeof(Bracket) && parsed[l].GetType() == typeof(Bracket))
            {
                return rec(l + 1, r - 1, parsed);
            }

            int ind = getSplit(l, r, parsed);

            if(ind!=-1)
            {
                BinaryNode output = new BinaryNode();

                output.op = parsed[ind] as Operation;
                output.L = rec(l, ind - 1, parsed);
                output.R = rec(ind + 1, r, parsed);

                return output;
            }
            else
            {
                if (parsed[l].GetType() == typeof(Operation))
                {
                    UnaryNode output = new UnaryNode();

                    output.invert = true;
                    output.child = rec(l + 1, r, parsed);

                    return output;
                }
                else
                {
                    ValueNode output = new ValueNode(parsed[l] as Value);
                    return output;
                }
            }
        }

        private static List <ExpressionTerm> parseString(string s)
        {
            List<ExpressionTerm> l = new List<ExpressionTerm>();

            for (int i = 0; i < s.Length;)
            {
                if (s[i] == '(' || s[i] == ')')
                {
                    l.Add(new Bracket(s[i]));
                    i++;
                }
                else if (s[i] == '!' || s[i] == '&' || s[i] == '|')
                {
                    l.Add(new Operation(s[i]));
                    i++;
                }
                else
                {
                    string val = "";
                    while (i < s.Length && s[i] != '(' && s[i] != ')' && s[i] != '!' && s[i] != '&' && s[i] != '|')
                    {
                        val += s[i];
                        i++;
                    }

                    l.Add(new Value(val));
                }
            }

            return l;
        }

        //add actions for invalid expressions
        public static LogicNode constructLogicTree(string s)
        {
            List<ExpressionTerm> parsed = parseString(s);
            LogicNode root = rec(0, parsed.Count - 1, parsed);
            
            return root;
        }
    }
}