using Microsoft.EntityFrameworkCore;
using Moq;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.Repositories;
using NUnit.Framework;
using System;

namespace MyFinance.Repositories.Tests
{
    [TestFixture]
    class ExpenseRepositoryTests
    {
        private MyFinanceContext _myFinanceDbContext;
        private IExpenseRepository _expenseRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<MyFinanceContext>()
                .UseInMemoryDatabase(databaseName: "MyFinanceDb")
                .Options;

            _myFinanceDbContext = new MyFinanceContext(options);
            _expenseRepository = new ExpenseRepository(_myFinanceDbContext);
        }

        [Test]
        public async System.Threading.Tasks.Task Get_ReturnExpenseByIdAsync()
        {
            // Arrange
            Guid id = new Guid("640f82ac-96d1-4fde-b7a7-fe284ee62c72");
            var expectedExpense = CreateExpense();

            await _myFinanceDbContext.Expenses.AddAsync(expectedExpense);
            await _myFinanceDbContext.SaveChangesAsync();

            // Act
            var returnedExpense = await _expenseRepository.GetExpenseById(id);

            // Assert
            Assert.AreEqual(expectedExpense, returnedExpense);
        }

        private Expense CreateExpense()
        {
            var expense = new Expense()
            {
                Id = new Guid("640f82ac-96d1-4fde-b7a7-fe284ee62c72"),
                Title = "TestTitle",
                ExpenseDate = DateTime.Now,
                Cost = 1
            };

            return expense;
        }
    }
}
