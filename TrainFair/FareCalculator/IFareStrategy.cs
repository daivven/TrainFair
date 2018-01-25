using TrainFair.Models;

namespace TrainFair.FareCalculator
{
    public interface IFareStrategy {
        float GetFare(IFareRule ruleValues, float basicFare);
    }
}
