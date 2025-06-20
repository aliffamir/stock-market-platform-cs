using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
   Task<List<Comment>> GetAllAsync();
   Task<Comment?> GetByIdAsync(int id);
   Task<Comment?> CreateAsync(Comment commentModel);
   Task<Comment?> UpdateAsync(int commentId, UpdateCommentRequestDto commentModel);
   Task<Comment?> DeleteAsync(int commentId);
}