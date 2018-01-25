using System;
using TrainFair.Models;

namespace TrainFair.FareCalculator
{
    public class StationRuleFareCalculator : IFareStrategy
    {
        public float GetFare(IFareRule ruleValues, float basicFare) {
            var stationFareRuleModel = ruleValues as StationFareRuleModel;            

            if (stationFareRuleModel == null || stationFareRuleModel.StationDistance <= 0.0f)
                return 0;
            
            if (stationFareRuleModel.StationDistance < 6)
                return basicFare;

            int restChargingStations = (int)Math.Ceiling((stationFareRuleModel.StationDistance - 6.0f)/10.0f);
            var totalFare = basicFare + restChargingStations * stationFareRuleModel.IncrementalPrice;           

            return totalFare;
        }
    }
}
