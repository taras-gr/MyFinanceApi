using Microsoft.EntityFrameworkCore;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.Repositories;
using MyFinance.Repositories.ResourceParameters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

            Seed(_myFinanceDbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _myFinanceDbContext.Expenses.RemoveRange(_myFinanceDbContext.Expenses);
            _myFinanceDbContext.SaveChanges();
        }

        [Test]
        public async Task GetUserExpenses_GivenId_ExpenseByIdAsync()
        {
            // Arrange
            Guid id = new Guid("46a7137d-894d-4d4c-97b0-9ec5ce846730");
            var expectedExpense = new Expense()
            {
                Id = new Guid("46a7137d-894d-4d4c-97b0-9ec5ce846730"),
                Title = "TestExpense_3",
                ExpenseDate = new DateTime(2020, 1, 2, 0, 0, 0, 0),
                Cost = 900,
                Category = "TestCategory_2",
                UserId = new Guid("63608a06-0e66-4758-8584-1d613e983635")
            };

            // Act
            var returnedExpense = await _expenseRepository.GetExpenseById(id);

            // Assert
            Assert.AreEqual(expectedExpense, returnedExpense);
        }

        [Test]
        public void GetUserExpenses_ExpensesResourceParametersIsNull_ArgumentNullException()
        {
            var testUserGuid = new Guid();
            ExpensesResourceParameters testExpensesResourceParameters = null;

            Assert.ThrowsAsync<ArgumentNullException>
                (async () => await _expenseRepository.GetUserExpenses(testUserGuid, testExpensesResourceParameters));
        }

        [Test]
        public async Task DeleteExpense_GivenExpense_ExpenseDeletedAsync()
        {
            var expectedExpenseToDelete = new Expense()
            {
                Id = new Guid("7c29c6bc-7b3c-42cc-8e6c-94ec609882b7"),
                Title = "TestExpense_5",
                ExpenseDate = new DateTime(2020, 9, 1, 0, 0, 0, 0),
                Cost = 5960,
                Category = "TestCategory_3",
                UserId = new Guid("5db0a816-659c-4ec4-af75-c5b0ce558b46")
            };           

            await _expenseRepository.DeleteExpense(expectedExpenseToDelete);

            var expenses = await _expenseRepository
                .GetUserExpenses(new Guid("5db0a816-659c-4ec4-af75-c5b0ce558b46"),
                new ExpensesResourceParameters());

            Assert.IsTrue(expenses.Count() == 1);
        }

        private void Seed(MyFinanceContext context)
        {
            var jsonTestDataFileName = "TestData.json";
            var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + jsonTestDataFileName);

            var json = File.ReadAllText(absolutePathForDocs);
            var jObject = JObject.Parse(json);
            var expenses = jObject["expenses"]?.ToObject<IEnumerable<Expense>>();

            foreach (var expense in expenses)
            {
                context.Expenses.Add(expense);
            }

            context.SaveChanges();

            foreach (var entity in context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}