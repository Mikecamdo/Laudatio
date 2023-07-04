﻿using EmployeeRecognition.Database;

namespace EmployeeRecognition.Interfaces;

public interface IUserRepository
{
    public Task<User> AddUserAsync(User user);
}
