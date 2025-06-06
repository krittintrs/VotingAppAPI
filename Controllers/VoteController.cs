using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingAppAPI.Data;
using VotingAppAPI.Models;

namespace VotingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly VotingDbContext _context;

        public VoteController(VotingDbContext context)
        {
            _context = context;
        }

        // GET: api/Vote
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
            return await _context.Votes
                .Include(v => v.Options)
                .ToListAsync();
        }


        // GET: api/Vote/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(int id)
        {
            var vote = await _context.Votes
                .Include(v => v.Options)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vote == null)
            {
                return NotFound();
            }

            return vote;
        }


        // PUT: api/Vote/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Vote>> PutVote(int id, Vote vote)
        {
            if (id != vote.Id)
            {
                return BadRequest(new { error = "Vote ID mismatch." });
            }

            if (vote.Options == null || !vote.Options.Any())
            {
                return BadRequest(new { error = "At least one option is required." });
            }

            var existingVote = await _context.Votes
                .Include(v => v.Options)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (existingVote == null)
            {
                return NotFound();
            }

            // Update vote fields
            existingVote.TopicName = vote.TopicName;
            existingVote.TopicDescription = vote.TopicDescription;

            // Remove old options
            _context.Options.RemoveRange(existingVote.Options);

            // Add new options
            foreach (var option in vote.Options)
            {
                option.VoteId = id; // Set FK
                _context.Options.Add(option);
            }

            await _context.SaveChangesAsync();

            var updatedVote = await _context.Votes
                 .Include(v => v.Options)
                 .FirstOrDefaultAsync(v => v.Id == id);

            return Ok(updatedVote);

        }


        // POST: api/Vote
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(Vote vote)
        {
            if (vote.Options == null || !vote.Options.Any())
            {
                return BadRequest(new
                {
                    error = "At least one option is required to create a vote."
                });
            }

            foreach (var option in vote.Options)
            {
                option.Vote = vote;
            }

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVote", new { id = vote.Id }, vote);
        }

        // POST: api/Vote/5/options/11/vote
        [HttpPost("{voteId}/options/{optionId}/vote")]
        public async Task<ActionResult<Vote>> CastVote(int voteId, int optionId)
        {
            var option = await _context.Options
                .Include(o => o.Vote)
                .ThenInclude(v => v.Options)
                .FirstOrDefaultAsync(o => o.Id == optionId && o.VoteId == voteId);

            if (option == null)
            {
                return NotFound(new { error = "Option not found for this vote." });
            }

            option.VoteCount += 1;
            await _context.SaveChangesAsync();

            // Return the full vote with updated options
            var updatedVote = option.Vote;

            return Ok(updatedVote);
        }


        // DELETE: api/Vote/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoteExists(int id)
        {
            return _context.Votes.Any(e => e.Id == id);
        }
    }
}
