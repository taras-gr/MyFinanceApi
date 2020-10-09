using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyFinance.Api.Controllers;
using MyFinance.Domain.Models;
using MyFinance.Services.DataTransferObjects;
using MyFinance.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Security.Claims;

namespace MyFinance.Api.Tests
{
    [TestFixture]
    class ExpenseControllerTests
    {
        private ExpenseController _expenseController;
        private Mock<IExpenseService> _expenseServiceMock;
        private Mock<ICategoryService> _categoryServiceMock;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void Setup()
        {
            _expenseServiceMock = new Mock<IExpenseService>();
            _categoryServiceMock = new Mock<ICategoryService>();
            _mapper = new Mock<IMapper>();

            _expenseController = new ExpenseController(
                _expenseServiceMock.Object, 
                _categoryServiceMock.Object, 
                _mapper.Object);            
        }

        [Test]
        public void GetUserExpenseById_UnauthorizedUser_Unauthorized()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { }, "mock"));

            _expenseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ActionResult<ExpenseDto> result = _expenseController.GetUserExpenseById("TestUser", new Guid()).Result;
            var resultAsStatusCodeResult = result.Result as StatusCodeResult;

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(resultAsStatusCodeResult);
            Assert.IsTrue(resultAsStatusCodeResult.StatusCode == 401);
        }

        [Test]
        public void GetUserExpenseById_ServiceReturnedNull_NotFound()
        {
            // Arange
            Expense nullExpenseToReturn = null;
            _mapper.Setup(s => s.Map<ExpenseDto>(It.IsAny<Expense>())).Returns(CreateExpenseDto());
            _expenseServiceMock.Setup(s => s.GetUserExpenseById(It.IsAny<Guid>(), 
                It.IsAny<Guid>())).ReturnsAsync(nullExpenseToReturn);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", Guid.Empty.ToString()),
                new Claim("UserName", "TestUser")
            }, "mock"));

            _expenseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ActionResult<ExpenseDto> result = _expenseController.GetUserExpenseById("TestUser", new Guid()).Result;
            var resultAsStatusCodeResult = result.Result as StatusCodeResult;

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(resultAsStatusCodeResult);
            Assert.IsTrue(resultAsStatusCodeResult.StatusCode == 404);
        }

        [Test]
        public void GetUserExpenseById_GivenId_UserExpenseWithGivenId()
        {
            // Arange
            _expenseServiceMock.Setup(x => x.GetUserExpenseById(new Guid(), new Guid())).ReturnsAsync(CreateExpense());
            _mapper.Setup(s => s.Map<ExpenseDto>(It.IsAny<Expense>())).Returns(CreateExpenseDto());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", Guid.Empty.ToString()),
                new Claim("UserName", "TestUser")
            }, "mock"));

            _expenseController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var expectedExpense = CreateExpenseDto();

            // Act
            ActionResult<ExpenseDto> result = _expenseController.GetUserExpenseById("TestUser", new Guid()).Result;
            var resultAsObjectResult = result.Result as ObjectResult;

            // Assert            
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            Assert.IsTrue(resultAsObjectResult.StatusCode == 200);
            Assert.AreEqual(expectedExpense, resultAsObjectResult.Value);
        }     

        private Expense CreateExpense()
        {
            var expense = new Expense()
            {
                Id = new Guid("640f82ac-96d1-4fde-b7a7-fe284ee62c72"),
                Title = "TestTitle",
                ExpenseDate = new DateTime(2020, 1, 1, 1, 1, 1, 0),
                Cost = 1,
                Category = "Test",
                User = new User(),
                UserId = Guid.Empty
            };

            return expense;
        }

        private ExpenseDto CreateExpenseDto()
        {
            var expense = new ExpenseDto()
            {
                Id = new Guid("640f82ac-96d1-4fde-b7a7-fe284ee62c72"),
                Title = "TestTitle",
                ExpenseDate = new DateTime(2020, 1, 1, 1, 1, 1, 0),
                Cost = 1,
                Category = "Test",
                UserId = Guid.Empty
            };

            return expense;
        }
    }
}
