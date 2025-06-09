using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;

    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.Include(comment => comment.AppUser).ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.Include(comment => comment.AppUser).FirstOrDefaultAsync(comment => comment.Id == id);
    }

    public async Task<Comment?> CreateAsync(Comment commentModel)
    {
        await _context.Comments.AddAsync(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int commentId, UpdateCommentRequestDto commentDto)
    {
        var commentModel = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == commentId);
        if (commentModel == null)
        {
            return null;
        }

        commentModel.Title = commentDto.Title;
        commentModel.Content = commentDto.Content;
        await _context.SaveChangesAsync();

        return commentModel;
    }

    public async Task<Comment?> DeleteAsync(int commentId)
    {
        var commentModel = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == commentId);
        if (commentModel == null)
        {
            return null;
        }

        _context.Comments.Remove(commentModel);
        await _context.SaveChangesAsync();
        return commentModel;
    }
}