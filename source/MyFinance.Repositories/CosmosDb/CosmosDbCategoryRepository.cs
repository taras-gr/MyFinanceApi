using Microsoft.Azure.Cosmos;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Repositories.CosmosDb
{
    public class CosmosDbCategoryRepository : ICategoryRepository
    {
        private readonly Container _container;

        public CosmosDbCategoryRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddCategory(Category category)
        {
            await _container.CreateItemAsync(category, new PartitionKey(category.Id.ToString()));
        }

        public async Task DeleteUserCategoryById(Guid userId, Guid categoryId)
        {
            await _container.DeleteItemAsync<Category>(categoryId.ToString(), new PartitionKey(categoryId.ToString()));
        }

        public async Task<IEnumerable<Category>> GetUserCategories(Guid userId)
        {
            var parameterizedQuery = new QueryDefinition(query: "SELECT * FROM Categories p WHERE p.UserId = @userId")
                .WithParameter("@userId", userId.ToString());

            // Query multiple items from container
            using FeedIterator<Category> filteredFeed = _container.GetItemQueryIterator<Category>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Category>();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Category> response = await filteredFeed.ReadNextAsync();

                // Iterate query results
                foreach (Category item in response)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public async Task<Category> GetUserCategoryById(Guid userId, Guid categoryId)
        {
            var parameterizedQuery = new QueryDefinition(query: "SELECT * FROM Categories p WHERE p.Id = @id")
                .WithParameter("@id", categoryId);

            // Query multiple items from container
            using FeedIterator<Category> filteredFeed = _container.GetItemQueryIterator<Category>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Category>();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Category> response = await filteredFeed.ReadNextAsync();

                // Iterate query results
                foreach (Category item in response)
                {
                    result.Add(item);
                }
            }

            return result.FirstOrDefault();
        }

        public async Task<Category> GetUserCategoryByTitle(Guid userId, string categoryTitle)
        {
            var parameterizedQuery = new QueryDefinition(query: "SELECT * FROM Categories p WHERE p.UserId = @userId AND p.Title = @title")
                .WithParameter("@userId", userId.ToString())
                .WithParameter("@title", categoryTitle);

            // Query multiple items from container
            using FeedIterator<Category> filteredFeed = _container.GetItemQueryIterator<Category>(
                queryDefinition: parameterizedQuery
            );

            var result = new List<Category>();

            // Iterate query result pages
            while (filteredFeed.HasMoreResults)
            {
                FeedResponse<Category> response = await filteredFeed.ReadNextAsync();

                // Iterate query results
                foreach (Category item in response)
                {
                    result.Add(item);
                }
            }

            return result.FirstOrDefault();
        }
    }
}
