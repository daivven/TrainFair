using TrainFair.Models;

namespace TrainFair.FareCalculator
{
    public class FareCalculatorContext {

        private readonly IFareStrategy _fareStrategy;
        public FareCalculatorContext(IFareStrategy fareStrategy) {
            this._fareStrategy = fareStrategy;
        }

        public float GetFareDetails(IFareRule fareRules, float basicFare)
        {
            return _fareStrategy.GetFare(fareRules, basicFare);
        }
    }
}
