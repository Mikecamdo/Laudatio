﻿using EmployeeRecognition.Api.Models;

namespace EmployeeRecognition.Core.UseCases.Kudos.GetKudosBySenderId;

public record GetKudosBySenderIdResponse(string Message)
{
    public record UserNotFound() : GetKudosBySenderIdResponse("A User with the given SenderId was not found");
    public record Success(IEnumerable<KudoModel> SenderKudos) : GetKudosBySenderIdResponse("");
}
