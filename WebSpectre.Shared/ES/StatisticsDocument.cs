namespace WebSpectre.Shared.ES
{
    public class StatisticsDocument
    {
        public Guid Id { get; set; }

        public string Agent { get; set; }

        public Statistics Statistics { get; set; }
    }
}
