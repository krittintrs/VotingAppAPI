namespace VotingAppAPI.Models
{
    public class Option
    {
        public int Id { get; set; }
        public required string OptionName { get; set; }

        public int VoteId { get; set; }
        public required Vote Vote { get; set; }

        public int VoteCount { get; set; }
    }
}
