using System;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Data.Interfaces;

namespace AYN.Services.Data.Implementations
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<CommentVote> commentVotesRepository;

        public CommentsService(
            IDeletableEntityRepository<Comment> commentsRepository,
            IDeletableEntityRepository<CommentVote> commentVotesRepository)
        {
            this.commentsRepository = commentsRepository;
            this.commentVotesRepository = commentVotesRepository;
        }

        public async Task Create(string content, string adId, string userId)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            var comment = new Comment()
            {
                AdId = adId,
                AddedByUserId = userId,
                Content = content,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task Delete(string commentId)
        {
            var comment = this.commentsRepository
                .All()
                .FirstOrDefault(c => c.Id == commentId);

            this.commentsRepository.Delete(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public async Task Vote(string voteValue, string commentId, string userId)
        {
            var commentVote = this.commentVotesRepository
                .All()
                .FirstOrDefault(cv => cv.ApplicationUserId == userId && cv.CommentId == commentId);

            if (commentVote != null)
            {
                commentVote.Value = Enum.Parse<CommentVoteValue>(voteValue);
                await this.commentVotesRepository.SaveChangesAsync();

                return;
            }

            commentVote = new CommentVote()
            {
                ApplicationUserId = userId,
                CommentId = commentId,
                Value = Enum.Parse<CommentVoteValue>(voteValue),
            };

            await this.commentVotesRepository.AddAsync(commentVote);
            await this.commentVotesRepository.SaveChangesAsync();
        }

        public bool IsCommentExisting(string commentId)
            => this.commentsRepository
                .All()
                .Any(c => c.Id == commentId);
    }
}
