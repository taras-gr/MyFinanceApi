using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Helpers;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Repositories.CosmosDb
{
    public class CosmosDbExpenseRepository : IExpenseRepository
    {
        private readonly Container _container;

        public CosmosDbExpenseRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddExpense(Expense expense)
        {
            await _container.CreateItemAsync(expense, new PartitionKey(expense.Id.ToString()));
        }

        public async Task DeleteExpense(Expense expense)
        {
            await _container.DeleteItemAsync<Category>(expense.Id.ToString(), new PartitionKey(expense.Id.ToString()));
        }

        public async Task<Expense> GetExpenseById(Guid expenseId)
        {
            var parameterizedQuery = new QueryDefinition(query: "SELECT * FROM Expenses p WHERE p.id = @id")
                .WithParameter("@id", expenseId.ToString());

            // Query multiple items from container
            using FeedIterator<Expense> filteredFeed = _container.GetItemQueryIterator<Expense>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Expense>();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Expense> response = await filteredFeed.ReadNextAsync();

                // Iterate query results
                foreach (Expense item in response)
                {
                    result.Add(item);
                }
            }

            return result.FirstOrDefault();
        }

        public async Task<Expense> GetUserExpenseById(Guid userId, Guid expenseId)
        {
            var parameterizedQuery = new QueryDefinition(query: "SELECT * FROM Expenses p WHERE p.id = @id AND p.userId = @userId")
                .WithParameter("@id", expenseId.ToString())
                .WithParameter("@userId", userId.ToString());

            // Query multiple items from container
            using FeedIterator<Expense> filteredFeed = _container.GetItemQueryIterator<Expense>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Expense>();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Expense> response = await filteredFeed.ReadNextAsync();

                // Iterate query results
                foreach (Expense item in response)
                {
                    result.Add(item);
                }
            }

            return result.FirstOrDefault();
        }

        public async Task<IQueryable<Expense>> GetUserExpenses(Guid userId, 
            ExpensesResourceParameters expensesResourceParameters)
        {
            if (expensesResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(ExpensesResourceParameters));
            }

            // Get LINQ IQueryable object
            IOrderedQueryable<Expense> queryable = _container.GetItemLinqQueryable<Expense>();

            // Construct LINQ query
            var collection = queryable.Where(s => s.UserId == userId);

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

            // Convert to feed iterator
            using FeedIterator<Expense> linqFeed = collection.ToFeedIterator();

            var result = new List<Expense>();

            // Iterate query result pages
            while (linqFeed.HasMoreResults)
            {
                FeedResponse<Expense> response = await linqFeed.ReadNextAsync();

                // Iterate query results
                foreach (Expense item in response)
                {
                    result.Add(item);
                }
            }

            return result.AsQueryable();
        }

        public Task UpdateExpense(Expense expense)
        {
            throw new NotImplementedException();
        }
    }
}
