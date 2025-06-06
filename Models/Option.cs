namespace VotingAppAPI.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string OptionName { get; set; }

        public int VoteId { get; set; }
        public Vote Vote { get; set; }

        public int VoteCount { get; set; }
    }
}
