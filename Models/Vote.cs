namespace VotingAppAPI.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public string TopicName { get; set; }
        public string TopicDescription { get; set; }

        public ICollection<Option> Options { get; set; }

    }
}