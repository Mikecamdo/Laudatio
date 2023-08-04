﻿using EmployeeRecognition.Api.Converters;
using EmployeeRecognition.Api.Dtos;
using EmployeeRecognition.Core.Converters;
using EmployeeRecognition.Core.Interfaces.UseCases.Comments;
using EmployeeRecognition.Core.UseCases.Comments.AddComment;
using EmployeeRecognition.Core.UseCases.Comments.DeleteComment;
using EmployeeRecognition.Core.UseCases.Comments.GetCommentsByKudoId;
using EmployeeRecognition.Core.UseCases.Comments.UpdateComment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecognition.Api.Controllers;

//[Authorize]
[Route("comments")]
public class CommentController : ControllerBase
{
    private readonly IGetCommentsUseCase _getCommentsUseCase;
    private readonly IGetCommentsByKudoIdUseCase _getCommentsByKudoIdUseCase;
    private readonly IAddCommentUseCase _addCommentUseCase;
    private readonly IUpdateCommentUseCase _updateCommentUseCase;
    private readonly IDeleteCommentUseCase _deleteCommentUseCase;

    public CommentController(
        IGetCommentsUseCase getCommentsUseCase,
        IGetCommentsByKudoIdUseCase getCommentsByKudoIdUseCase,
        IAddCommentUseCase addCommentUseCase,
        IUpdateCommentUseCase updateCommentUseCase,
        IDeleteCommentUseCase deleteCommentUseCase)
    {
        _getCommentsUseCase = getCommentsUseCase;
        _getCommentsByKudoIdUseCase = getCommentsByKudoIdUseCase;
        _addCommentUseCase = addCommentUseCase;
        _updateCommentUseCase = updateCommentUseCase;
        _deleteCommentUseCase = deleteCommentUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var allComments = await _getCommentsUseCase.ExecuteAsync();
        return Ok(CommentModelConverter.ToModel(allComments));

    }

    [HttpGet("{kudoId}")]
    public async Task<IActionResult> GetCommentsByKudoId([FromRoute] int kudoId)
    {
        var getCommentsByKudoIdResponse = await _getCommentsByKudoIdUseCase.ExecuteAsync(kudoId);

        return getCommentsByKudoIdResponse switch
        {
            GetCommentsByKudoIdResponse.Success success => Ok(success.Comments),
            GetCommentsByKudoIdResponse.KudoNotFound => BadRequest(getCommentsByKudoIdResponse.Message),
            _ => Problem("Unexpected response")
        };
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] CommentDto comment)
    {
        var newComment = CommentDtoConverter.ToModel(comment);

        var addCommentResponse = await _addCommentUseCase.ExecuteAsync(newComment);

        return addCommentResponse switch
        {
            AddCommentResponse.Success success => Ok(success.NewComment),
            AddCommentResponse.InvalidRequest => BadRequest(addCommentResponse.Message),
            AddCommentResponse.KudoNotFound => BadRequest(addCommentResponse.Message),
            _ => Problem("Unexpected response")
        };
    }

    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdateComment([FromRoute] int commentId, [FromBody] CommentDto updatedCommentInfo)
    {
        var currentComment = CommentDtoConverter.ToModel(updatedCommentInfo);
        currentComment.Id = commentId;
        var updateCommentResponse = await _updateCommentUseCase.ExecuteAsync(currentComment);

        return updateCommentResponse switch
        {
            UpdateCommentResponse.Success success => Ok(success.UpdatedComment),
            UpdateCommentResponse.CommentNotFound => NotFound(updateCommentResponse.Message),
            _ => Problem("Unexpected response")
        };
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
    {
        var deleteCommentResponse = await _deleteCommentUseCase.ExecuteAsync(commentId);

        return deleteCommentResponse switch
        {
            DeleteCommentResponse.Success => NoContent(),
            DeleteCommentResponse.CommentNotFound => NotFound(deleteCommentResponse.Message),
            _ => Problem("Unexpected response")
        };
    }
}
