using ScientistAssistant_ConsoleVersion.Datasets.EONET.DatasetClasses;
using ScientistAssistant_ConsoleVersion.QueryTagLogic;
using ScientistAssistant_ConsoleVersion.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScientistAssistant_ConsoleVersion.Datasets.EONET.Queries.ReportQuery
{
    interface IReportQuery : IQuery
    {

    }

    class ReportQuery : IReportQuery
    {
        Dictionary<string, IReportQuery> queries = new Dictionary<string, IReportQuery>();

        public ReportQuery()
        {
            queries["general"] = new ReportGeneralQuery();
        }

        public void execute(List<string> flags)
        {
            if (flags.Count > 0)
            {
                string reportType = flags[0];
                flags.RemoveAt(0);

                if (queries.ContainsKey(reportType) == true)
                {
                    queries[reportType].execute(flags);
                }
                else
                {
                    throw new WrongFlagException(reportType);
                }
            }
            else
            {
                throw new InsufficientNumberOfFlagsException();
            }
        }
    }

    class ReportGeneralQuery : IReportQuery
    {
        FilteringFunctionDictionary<Event> mp = new FilteringFunctionDictionary<Event>();

        public ReportGeneralQuery() 
        {
            mp.addFun("properties", GenericOperations.filterListByProperties);
            mp.addFun("position", GenericOperations.filterListByPosition);
        }

        public void execute(List<string> flags)
        {
            List<Event> matching = EONETDataset.events;
            matching = GenericOperations.filterList(matching, flags, mp);

            Console.WriteLine();
            Console.WriteLine($"MatchingCount: {matching.Count}");
            Console.WriteLine($"Average latitude: {matching.Average(e => e.getAverageCoordinate(1))}");
            Console.WriteLine($"Average longtitude: {matching.Average(e => e.getAverageCoordinate(0))}");
            
            if(matching.Count>=2)
            {
                Tuple<double, EarthPoint, EarthPoint> closest =
                GeometryUtils.findClosestPoints(matching.Select(e => new EarthPoint(e)).ToList());
             
                Console.WriteLine($"Closest two events: " +
                    $"{((Event)closest.Item2.additionalInfo).id} and {((Event)closest.Item3.additionalInfo).id}" +
                    $" at distance {closest.Item1}");

                //--------------------------------------

                Tuple<double, EarthPoint, EarthPoint> furthest =
                GeometryUtils.findFurthestPoints(matching.Select(e => new EarthPoint(e)).ToList());

                Console.WriteLine($"Furthest two events: " +
                    $"{((Event)furthest.Item2.additionalInfo).id} and {((Event)furthest.Item3.additionalInfo).id}" +
                    $" at distance {closest.Item1}");
            }
            
        }
    }
}
