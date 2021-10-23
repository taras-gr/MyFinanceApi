using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Repositories.DynamoDb
{
    public class DynamoDbExpenseRepository : IExpenseRepository
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;
        private readonly Table _expensesTable;

        public DynamoDbExpenseRepository()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
            _expensesTable = Table.LoadTable(_client, "Expenses");
        }

        public async Task AddExpense(Guid userId, Expense expense)
        {
            var expenseDocument = new Document();
            expenseDocument["Id"] = Guid.NewGuid();
            expenseDocument["Title"] = expense.Title;
            expenseDocument["Category"] = expense.Category;
            expenseDocument["ExpenseDate"] = expense.ExpenseDate;
            expenseDocument["Cost"] = expense.Cost;
            expenseDocument["UserId"] = userId;

            await _expensesTable.PutItemAsync(expenseDocument);
        }

        public async Task DeleteExpense(Expense expense)
        {
            DeleteItemOperationConfig config = new DeleteItemOperationConfig
            {
                // Return the deleted item.
                ReturnValues = ReturnValues.AllOldAttributes
            };

            await _expensesTable.DeleteItemAsync(expense.Id, config);
        }

        public async Task<Expense> GetExpenseById(Guid expenseId)
        {
            var expenses = await _context.ScanAsync<Expense>(new List<ScanCondition>(), new DynamoDBOperationConfig { OverrideTableName = "Expenses" }).GetRemainingAsync();
            var expense = expenses.Where(s => s.Id == expenseId).FirstOrDefault();

            return expense;
        }

        public async  Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId)
        {
            var expenses = await _context.ScanAsync<Expense>(new List<ScanCondition>(), new DynamoDBOperationConfig { OverrideTableName = "Expenses" }).GetRemainingAsync();
            var userCategory = expenses.Where(s => s.UserId == userId && s.Id == expenseId).FirstOrDefault();

            return userCategory;
        }

        public async Task<IQueryable<Expense>> GetUserExpenses(Guid userId, ExpensesResourceParameters expensesResourceParameters)
        {
            var expenses = await _context.ScanAsync<Expense>(new List<ScanCondition>(), new DynamoDBOperationConfig { OverrideTableName = "Expenses" }).GetRemainingAsync();
            
            if (expensesResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(ExpensesResourceParameters));
            }

            var collection = expenses.AsQueryable<Expense>();

            collection = collection.Where(s => s.UserId == userId);

            if (!string.IsNullOrWhiteSpace(expensesResourceParameters.SearchQuery))
            {
                var searchQuery = expensesResourceParameters.SearchQuery.Trim();
                collection = collection.Where(s => s.Title.Contains(searchQuery)
                || s.Category.Contains(searchQuery));
            }

            if (!string.IsNullOrWhiteSpace(expensesResourceParameters.OrderBy))
            {
                collection = collection.ApplySort(expensesResourceParameters.OrderBy);
            }

            return await Task.Run(() => collection);
        }

        public async Task UpdateExpense(Expense expense)
        {
            Guid partitionKey = expense.Id;

            var expenseDocument = new Document();
            expenseDocument["Id"] = partitionKey;
            // List of attribute updates.
            // The following replaces the existing authors list.
            expenseDocument["Title"] = expense.Title;
            expenseDocument["Category"] = expense.Category;
            expenseDocument["ExpenseDate"] = expense.ExpenseDate;
            expenseDocument["Cost"] = expense.Cost;
            expenseDocument["UserId"] = expense.UserId;

            // Optional parameters.
            UpdateItemOperationConfig config = new UpdateItemOperationConfig
            {
                // Get updated item in response.
                ReturnValues = ReturnValues.AllNewAttributes
            };
            await _expensesTable.UpdateItemAsync(expenseDocument, config);
        }
    }
}
