﻿using Entity = EmployeeRecognition.Core.Entities.Comment;
using Model = EmployeeRecognition.Api.Models.CommentModel;

namespace EmployeeRecognition.Api.Converters;

public class CommentModelConverter
{
    public static Entity ToEntity(Model model)
    {
        return new Entity()
        {
            Id = model.Id,
            KudoId = model.KudoId,
            SenderId = model.SenderId,
            Message = model.Message
        };
    }

    public static IEnumerable<Entity> ToEntity(IEnumerable<Model> models)
    {
        return models.Select(m => ToEntity(m));
    }

    public static Model ToModel(Entity entity)
    {
        return new Model()
        {
            Id = entity.Id,
            KudoId = entity.KudoId,
            SenderId = entity.SenderId,
            SenderName = entity.Kudo?.Sender?.Name,
            SenderAvatar = entity.Kudo?.Sender?.AvatarUrl,
            Message = entity.Message
        };
    }

    public static IEnumerable<Model> ToModel(IEnumerable<Entity> entities)
    {
        return entities.Select(e => ToModel(e));
    }
}
