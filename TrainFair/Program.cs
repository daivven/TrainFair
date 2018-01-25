using System;

namespace TrainFair
{
    class Program
    {
        static void Main(string[] args)
        {
            FareRuleBuilder.FareRuleBuilder fareRuleBuilder = new FareRuleBuilder.FareRuleBuilder();
            fareRuleBuilder.DisplayTrainFare();
            Console.ReadKey();
        }
    }
}
