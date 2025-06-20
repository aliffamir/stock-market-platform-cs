using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase 
{
   private readonly ICommentRepository _commentRepo;
   private readonly IStockRepository _stockRepo;
   private readonly UserManager<AppUser> _userManager;
   public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
   {
      _commentRepo = commentRepo;
      _stockRepo = stockRepo;
      _userManager = userManager;
   }

   [HttpGet]
   public async Task<IActionResult> GetAll()
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }
      var comments = await _commentRepo.GetAllAsync();
      var commentDto = comments.Select(comment => comment.ToCommentDto());
      return Ok(commentDto);
   }

   [HttpGet]
   [Route("{id:int}")]
   public async Task<IActionResult> GetById([FromRoute] int id)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }
      var comment = await _commentRepo.GetByIdAsync(id);
      if (comment == null)
      {
         return NotFound();
      }

      return Ok(comment.ToCommentDto());
   }

   [HttpPost("{stockId:int}")]
   [Authorize]
   public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }
      if (!await _stockRepo.StockExistsAsync(stockId))
      {
         return NotFound();
      }

      var username = User.GetUsername();
      var appUser = await _userManager.FindByNameAsync(username);
      var commentModel = commentDto.ToCommentFromCreateDto(stockId);
      commentModel.AppUserId = appUser.Id;
      await _commentRepo.CreateAsync(commentModel);
      return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
   }

   [HttpPut("{commentId:int}")]
   public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentRequestDto commentDto)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }

      var commentModel = await _commentRepo.UpdateAsync(commentId, commentDto);
      if (commentModel == null)
      {
         return NotFound();
      }
      return Ok(commentModel.ToCommentDto());
   }

   [HttpDelete("{commentId:int}")]
   public async Task<IActionResult> Delete([FromRoute] int commentId)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }
      var commentModel = await _commentRepo.DeleteAsync(commentId);
      if (commentModel == null)
      {
         return NotFound();
      }

      return NoContent();
   }
}
