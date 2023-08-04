﻿using EmployeeRecognition.Core.Converters;
using EmployeeRecognition.Core.Interfaces.Repositories;
using EmployeeRecognition.Core.Interfaces.UseCases.Users;

namespace EmployeeRecognition.Core.UseCases.Users.GetUserByName;

public class GetUserByNameUseCase : IGetUserByNameUseCase
{
    private readonly IUserRepository _userRepository;
    public GetUserByNameUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUserByNameResponse> ExecuteAsync(string name)
    {
        var currentUser = await _userRepository.GetUserByNameAsync(name);

        if (currentUser == null)
        {
            return new GetUserByNameResponse.UserNotFound();
        }

        return new GetUserByNameResponse.Success(UserModelConverter.ToModel(currentUser));
    }
}
