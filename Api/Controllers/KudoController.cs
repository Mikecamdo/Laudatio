﻿using EmployeeRecognition.Api.Converters;
using EmployeeRecognition.Api.Models;
using EmployeeRecognition.Core.Entities;
using EmployeeRecognition.Core.Interfaces.UseCases.Kudos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRecognition.Api.Controllers;

[Authorize]
[Route("kudos")]
public class KudoController : ControllerBase
{
    private readonly IGetAllKudosUseCase _getAllKudosUseCase;
    private readonly IGetKudosBySenderIdUseCase _getKudosBySenderIdUseCase;
    private readonly IGetKudosByReceiverIdUseCase _getKudosByReceiverIdUseCase;
    private readonly IAddKudoUseCase _addKudoUseCase;
    private readonly IDeleteKudoUseCase _deleteKudoUseCase;

    public KudoController(
        IGetAllKudosUseCase getAllKudosUseCase,
        IGetKudosBySenderIdUseCase getKudosBySenderIdUseCase,
        IGetKudosByReceiverIdUseCase getKudosByReceiverIdUseCase,
        IAddKudoUseCase addKudoUseCase,
        IDeleteKudoUseCase deleteKudoUseCase)
    {
        _getAllKudosUseCase = getAllKudosUseCase;
        _getKudosBySenderIdUseCase = getKudosBySenderIdUseCase;
        _getKudosByReceiverIdUseCase = getKudosByReceiverIdUseCase;
        _addKudoUseCase = addKudoUseCase;
        _deleteKudoUseCase = deleteKudoUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> GetKudos()
    {
        var allKudos = await _getAllKudosUseCase.ExecuteAsync();
        return Ok(KudoModelConverter.ToModel(allKudos));
    }

    [HttpGet("sender/{senderId}")]
    public async Task<IActionResult> GetKudosBySenderId([FromRoute] string senderId)
    {
        var senderKudos = await _getKudosBySenderIdUseCase.ExecuteAsync(senderId);
        return Ok(KudoModelConverter.ToModel(senderKudos));
    }

    [HttpGet("receiver/{receiverId}")]
    public async Task<IActionResult> GetKudosByReceiverId([FromRoute] string receiverId)
    {
        var receiverKudos = await _getKudosByReceiverIdUseCase.ExecuteAsync(receiverId);
        return Ok(KudoModelConverter.ToModel(receiverKudos));
    }

    [HttpPost]
    public async Task<IActionResult> AddKudo([FromBody] KudoDto kudo)
    {
        var newKudo = new Kudo() //FIXME need to move to a converter
        {
            SenderId = kudo.SenderId,
            ReceiverId = kudo.ReceiverId,
            Title = kudo.Title,
            Message = kudo.Message,
            TeamPlayer = kudo.TeamPlayer,
            OneOfAKind = kudo.OneOfAKind,
            Creative = kudo.Creative,
            HighEnergy = kudo.HighEnergy,
            Awesome = kudo.Awesome,
            Achiever = kudo.Achiever,
            Sweetness = kudo.Sweetness,
            TheDate = DateOnly.FromDateTime(DateTime.Now)
        };

        var addedKudo = await _addKudoUseCase.ExecuteAsync(newKudo);
        return Ok(KudoModelConverter.ToModel(addedKudo));
    }

    [HttpDelete("{kudoId}")]
    public async Task<IActionResult> DeleteKudo([FromRoute] int kudoId)
    {
        await _deleteKudoUseCase.ExecuteAsync(kudoId);
        return NoContent();
    }
}
