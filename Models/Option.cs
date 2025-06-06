using System.Text.Json.Serialization;

namespace VotingAppAPI.Models
{
    public class Option
    {
        public int Id { get; set; }
        public required string OptionName { get; set; }

        public int VoteId { get; set; }
        [JsonIgnore]
        public Vote? Vote { get; set; }

        public int VoteCount { get; set; }
    }
}
