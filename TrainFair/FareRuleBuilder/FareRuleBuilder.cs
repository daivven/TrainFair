using System;
using System.Collections.Generic;
using TrainFair.Constants;
using TrainFair.FareCalculator;
using TrainFair.Models;

namespace TrainFair.FareRuleBuilder
{
    public class FareRuleBuilder : IFareRuleBuilder
    {
        public void DisplayTrainFare() {

            //票价定价规定 
            //按照市物价主管部门批复的轨道交通网络票价体系，即：轨道交通实行按里程计价的多级票价，0~6公里3元，6公里之后每10公里增加1元;票价计算采用最短路径法，即：当两个站点之间有超过1条换乘路径时，选取里程最短的一条路径作为两站间票价计算依据。
            
            Console.WriteLine("地铁票价计算");
            Console.WriteLine("-----------------------");
            Console.WriteLine("输入起始站名:");
            var fromStation = Console.ReadLine();
            Console.WriteLine("输入目的地站名:");
            var toStation = Console.ReadLine();

            StationFareRuleModel stations = new StationFareRuleModel();
            stations.FareRuleId = 1;           
            stations.StationDistance = CountStationsDistance(fromStation, toStation);
            stations.IncrementalPrice = FareConstants.OnStationFare;

            FareCalculatorContext fareCalculatorContext = new FareCalculatorContext(new StationRuleFareCalculator());
            var totalFare = fareCalculatorContext.GetFareDetails(stations, FareConstants.BasicFare);
            Console.WriteLine("-----------------------");
            Console.WriteLine("从 {1} 至 {2}，距离 {3}KM，车票价钱为：  {0}元", totalFare, fromStation, toStation, stations.StationDistance);
            Console.WriteLine("-----------------------");
           
            Console.WriteLine("是否是VIP (y/n):");
            var isSeniorCitize = Console.ReadKey();
            if (isSeniorCitize.Key == ConsoleKey.Y) {                
                VIPFareRuleModel ageFareRuleModel = new VIPFareRuleModel();
                ageFareRuleModel.FareRuleId = 2;              
                ageFareRuleModel.Discount = FareConstants.VIPDiscount;

                fareCalculatorContext = new FareCalculatorContext(new VIPRuleFareCalculator());
                totalFare = fareCalculatorContext.GetFareDetails(ageFareRuleModel, totalFare);
                Console.WriteLine("\n-----------------------");
                Console.WriteLine("享受会员折扣");
                Console.WriteLine("从 {1} 至 {2}，享受{3}折的优惠，车票价钱为：  {0}", totalFare, fromStation, toStation, FareConstants.VIPDiscount);
            }
           
            Console.WriteLine("\n是否还有其它费用 (y/n):");
            var isFestival = Console.ReadKey();
            if (isFestival.Key == ConsoleKey.Y) {
                var otherFareRuleModel = new OtherFareRuleModel();
                Console.WriteLine("\n输入费用名称:");
                otherFareRuleModel.FareRuleId = 3;
                otherFareRuleModel.OtherFareName = Console.ReadLine();
                Console.WriteLine("\n输入费用(元):");
                otherFareRuleModel.AdditionalFare = float.Parse(Console.ReadLine());
                fareCalculatorContext = new FareCalculatorContext(new OtherRuleFareCalculator());
                totalFare = fareCalculatorContext.GetFareDetails(otherFareRuleModel, totalFare);
                Console.WriteLine("-----------------------");
                Console.WriteLine("总票价");
                Console.WriteLine("从 {1} 至 {2}，车票价钱为：  {0}元", totalFare, fromStation, toStation);
            }
        }

        public float CountStationsDistance(string from, string to)
        {           
            return DistanceOfStations(from, to);
        }

        public float DistanceOfStations(string from, string to)
        {           
            string[] stations = new string[] { "1s", "2s", "3s", "4s", "5s", "6s", "7s","8s", "9s", "10s", "11s", "12s", "13s", "14s",
             "15s", "16s", "17s", "18s", "19s", "20s", "21s","22s"};
            float[] distances = new float[] { 1.8F, 1.2F, 1.2F, 1.6F, 2.2F, 2.6F, 1.2F,1.5F, 1.6F, 2.0F, 2.8F, 0.8F, 1.2F, 3.5F
            , 1.6F,1.8F, 1.2F, 2.8F, 2.4F, 1.8F, 2.3F, 2.5F};
            var fromIndex = Array.IndexOf(stations, from);
            var toIndex = Array.IndexOf(stations, to);
            List<string> travellingStations = new List<string>();
            float distance = 0.0f;
            if (fromIndex <= toIndex)
            {
                for (int i = fromIndex; i <= toIndex; i++)
                {                    
                    distance += distances[i];
                }
            }
            return distance;
        }
    }
}
