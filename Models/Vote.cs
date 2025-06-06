namespace VotingAppAPI.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public required string TopicName { get; set; }
        public required string TopicDescription { get; set; }

        public required ICollection<Option> Options { get; set; }

    }
}