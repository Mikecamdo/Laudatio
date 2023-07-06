﻿using EmployeeRecognition.Api.Models;
using EmployeeRecognition.Core.Entities;
using EmployeeRecognition.Core.Interfaces.UseCases.Comments;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecognition.Api.Controllers;

[ApiController]
[Route("comment")]
public class CommentController : ControllerBase
{
    private readonly IGetCommentsUseCase _getCommentsUseCase;
    private readonly IGetCommentsByKudoIdUseCase _getCommentsByKudoIdUseCase;
    private readonly IAddCommentUseCase _addCommentUseCase;

    public CommentController(
        IGetCommentsUseCase getCommentsUseCase,
        IGetCommentsByKudoIdUseCase getCommentsByKudoIdUseCase,
        IAddCommentUseCase addCommentUseCase)
    {
        _getCommentsUseCase = getCommentsUseCase;
        _getCommentsByKudoIdUseCase = getCommentsByKudoIdUseCase;
        _addCommentUseCase = addCommentUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var allComments = await _getCommentsUseCase.ExecuteAsync();
        return Ok(allComments);

    }

    [HttpGet("{kudoId}")]
    public async Task<IActionResult> GetCommentsByKudoId(int kudoId)
    {
        var kudoComments = await _getCommentsByKudoIdUseCase.ExecuteAsync(kudoId);
        return Ok(kudoComments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] CommentDto comment)
    {
        var newComment = new Comment() //FIXME need to move to a converter
        {
            KudoId = comment.KudoId,
            SenderId = comment.SenderId,
            SenderName = comment.SenderName,
            SenderAvatar = comment.SenderAvatar,
            Message = comment.Message,
        };

        var addedComment = await _addCommentUseCase.ExecuteAsync(newComment);
        return Ok(addedComment);
    }
}
