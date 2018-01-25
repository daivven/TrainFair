using TrainFair.Models;

namespace TrainFair.FareCalculator
{
    public class VIPRuleFareCalculator : IFareStrategy
    {
        public float GetFare(IFareRule ruleValues, float basicFare) {
            var vipFareRuleModel = ruleValues as VIPFareRuleModel;
            if (vipFareRuleModel == null)
                return 0;
           
            var totalFare = basicFare - (basicFare * vipFareRuleModel.Discount);
            return totalFare;
        }
    }
}
