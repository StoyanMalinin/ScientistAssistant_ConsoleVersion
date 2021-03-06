﻿using ScientistAssistant_ConsoleVersion.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScientistAssistant_ConsoleVersion.Datasets.EONET;
using ScientistAssistant_ConsoleVersion.UI;

namespace ScientistAssistant_ConsoleVersion.UI
{
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

            if (datasets.ContainsKey(dataset) == true)
            {
                datasets[dataset].processQuery(flags);
            }
            else
            {
                throw new InvalidDatasetException(dataset);
            }
        }
    }
}
