namespace TrainFair.Models
{
    public class StationFareRuleModel : IFareRule
    {
        public int FareRuleId { get; set; }

        public int  StationsCounts { get; set; }

        public float IncrementalPrice { get; set; }

        public float StationDistance { get; set; }
    }
}
