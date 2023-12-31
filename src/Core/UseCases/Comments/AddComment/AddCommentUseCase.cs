﻿using Laudatio.Api.Models;
using Laudatio.Core.Converters;
using Laudatio.Core.Interfaces.Repositories;
using Laudatio.Core.Interfaces.UseCases.Comments;

namespace Laudatio.Core.UseCases.Comments.AddComment;

public class AddCommentUseCase : IAddCommentUseCase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IKudoRepository _kudoRepository;

    public AddCommentUseCase(ICommentRepository commentRepository, IKudoRepository kudoRepository)
    {
        _commentRepository = commentRepository;
        _kudoRepository = kudoRepository;
    }

    public async Task<AddCommentResponse> ExecuteAsync(CommentModel comment)
    {
        if (comment.SenderId == "" || comment.Message == "" ||
           comment.SenderId == null || comment.Message == null)
        {
            return new AddCommentResponse.InvalidRequest("Missing/invalid parameters");
        }

        var kudo = await _kudoRepository.GetKudoByIdAsync(comment.KudoId);

        if (kudo == null)
        {
            return new AddCommentResponse.KudoNotFound();
        }

        var newComment = await _commentRepository.AddCommentAsync(CommentModelConverter.ToEntity(comment));
        return new AddCommentResponse.Success(CommentModelConverter.ToModel(newComment));
    }
}
