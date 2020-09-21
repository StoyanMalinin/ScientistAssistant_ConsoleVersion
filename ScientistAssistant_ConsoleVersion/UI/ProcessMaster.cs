using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.UI
{
    static class ProcessMaster
    {
        public static void loop()
        {
            InputHandler ih = new InputHandler();

            while(true)
            {
                try
                {
                    ih.handleInput();
                }
                catch(CustomUIException e)
                {
                    Console.WriteLine("Invalid command");
                    Console.WriteLine(e.Message);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Invalid command");
                    Console.WriteLine("Unhandled syntax error or a potential bug");

                    Console.WriteLine();
                    Console.WriteLine("More еxception info:");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
