
namespace TrainFair.Models
{
    public class VIPFareRuleModel : IFareRule
    {
        public int FareRuleId { get; set; }      

        public float Discount { get; set; }
    }
}
