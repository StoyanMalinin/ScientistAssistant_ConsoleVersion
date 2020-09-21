using ScientistAssistant_ConsoleVersion.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.UI
{
    //add actions for invalid commands
    class InputHandler
    {
        Dictionary<string, IDataset> datasets = new Dictionary<string, IDataset>();

        public InputHandler()
        {
            datasets["eonet"] = new EONETDataset();
        }

        private string readInput()
        {
            Console.Write("$");
            string input = Console.ReadLine();
            return input;
        }

        private List<string> parseInput(string input)
        {
            return input.Split(' ').Where(x => x != "").Select(x => x.ToLower()).ToList();
        }

        public void handleInput()
        {
            List<string> flags = parseInput(readInput());

            string dataset = flags[0];
            flags.RemoveAt(0);

            if(datasets.ContainsKey(dataset) ==true)
            {
                datasets[dataset].processQuery(flags);
            }
        }
    }
}
