﻿using System.Threading.Tasks;

namespace MyFinance.Services.Interfaces
{
    public interface IAuthenticationManagerService
    {
        Task<string> AuthenticateAsync(string username, string password);
    }
}