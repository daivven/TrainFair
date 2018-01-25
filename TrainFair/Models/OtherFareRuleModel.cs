namespace TrainFair.Models
{
    public class OtherFareRuleModel : IFareRule
    {
        public int FareRuleId { get; set; }

        public string OtherFareName { get; set; }

        public float AdditionalFare { get; set; }
    }
}
