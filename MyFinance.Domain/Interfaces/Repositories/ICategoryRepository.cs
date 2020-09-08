using System;
using System.Threading.Tasks;
using MyFinance.Domain.Models;

namespace MyFinance.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<ExpenseCategory> GetCategoryById(Guid userId);
        //Task<User> GetUserByName(string userName);
        //Task<User> GetUserByEmail(string email);
        //Task<List<User>> GetUsers();
        Task AddCategory(Guid userId, ExpenseCategory expense);
        //Task<bool> UpdateUser(string userId, User user);
        //Task<bool> DeleteUser(string userId);
        Task<int> Save();
    }
}