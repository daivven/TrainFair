using TrainFair.Models;

namespace TrainFair.FareCalculator
{
    public class OtherRuleFareCalculator : IFareStrategy
    {
        public float GetFare(IFareRule ruleValues, float basicFare) {
            var otherFareRuleModel = ruleValues as OtherFareRuleModel;
            if (otherFareRuleModel == null)
                return basicFare;

            float totalFare = basicFare + otherFareRuleModel.AdditionalFare;
            return totalFare;
        }
    }
}
