using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase 
{
   private readonly ICommentRepository _commentRepo;
   private readonly IStockRepository _stockRepo;
   public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
   {
      _commentRepo = commentRepo;
      _stockRepo = stockRepo;
   }

   [HttpGet]
   public async Task<IActionResult> GetAll()
   {
      var comments = await _commentRepo.GetAllAsync();
      var commentDto = comments.Select(comment => comment.ToCommentDto());
      return Ok(commentDto);
   }

   [HttpGet]
   [Route("{id}")]
   public async Task<IActionResult> GetById([FromRoute] int id)
   {
      var comment = await _commentRepo.GetByIdAsync(id);
      if (comment == null)
      {
         return NotFound();
      }

      return Ok(comment.ToCommentDto());
   }

   [HttpPost("{stockId}")]
   public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
   {
      if (!await _stockRepo.StockExistsAsync(stockId))
      {
         return NotFound();
      }

      var commentModel = commentDto.ToCommentFromCreateDto(stockId);
      await _commentRepo.CreateAsync(commentModel);
      return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
   }

   [HttpPut("{commentId}")]
   public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentRequestDto commentDto)
   {
      var commentModel = await _commentRepo.UpdateAsync(commentId, commentDto);
      if (commentModel == null)
      {
         return NotFound();
      }
      return Ok(commentModel.ToCommentDto());
   }
   
}
