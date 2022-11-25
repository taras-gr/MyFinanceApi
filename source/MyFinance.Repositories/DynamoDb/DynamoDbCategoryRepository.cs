using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MyFinance.Domain.Models;
using MyFinance.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Repositories.DynamoDb
{
    public class DynamoDbCategoryRepository : ICategoryRepository
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;
        private readonly Table _categoriesTable;

        public DynamoDbCategoryRepository()
        {
            _client = new AmazonDynamoDBClient();
            _context = new DynamoDBContext(_client);
            _categoriesTable = Table.LoadTable(_client, "Categories");
        }

        public async Task AddCategory(Category category)
        {            
            var categoryDocument = new Document();
            categoryDocument["Id"] = Guid.NewGuid();
            categoryDocument["Title"] = category.Title;
            categoryDocument["UserId"] = category.UserId;

            await _categoriesTable.PutItemAsync(categoryDocument);
        }

        public async Task DeleteUserCategoryById(Guid userId, Guid categoryId)
        {
            DeleteItemOperationConfig config = new DeleteItemOperationConfig
            {
                // Return the deleted item.
                ReturnValues = ReturnValues.AllOldAttributes
            };

            await _categoriesTable.DeleteItemAsync(categoryId, config);
        }

        // TODO: Change flow to filter categories on DynamoDB Server
        public async Task<IEnumerable<Category>> GetUserCategories(Guid userId)
        {
            //var config = new DynamoDBOperationConfig
            //{
            //    OverrideTableName = "Categories",
            //    //QueryFilter = new QueryFilter("UserId", ScanOperator.Equal, userId.ToString())
            //};

            var categories = await _context.ScanAsync<Category>(new List<ScanCondition>(), new DynamoDBOperationConfig { OverrideTableName="Categories" }).GetRemainingAsync();
            var userCategories = categories.Where(s => s.UserId == userId);

            return userCategories;
        }

        public async Task<Category> GetUserCategoryById(Guid userId, Guid categoryId)
        {
            var categories = await _context.ScanAsync<Category>(new List<ScanCondition>(), new DynamoDBOperationConfig { OverrideTableName = "Categories" }).GetRemainingAsync();
            var userCategory = categories.Where(s => s.UserId == userId && s.Id == categoryId).FirstOrDefault();

            return userCategory;
        }

        // TODO: Change flow to filter categories on DynamoDB Server
        public async Task<Category> GetUserCategoryByTitle(Guid userId, string categoryTitle)
        {
            //var config = new DynamoDBOperationConfig
            //{
            //    QueryFilter = new List<ScanCondition>
            //    {
            //        new ScanCondition("UserId", ScanOperator.Equal, userId),
            //        new ScanCondition("Title", ScanOperator.Equal, categoryTitle)
            //    }
            //};

            var categories = await _context.ScanAsync<Category>(new List<ScanCondition>(), new DynamoDBOperationConfig { OverrideTableName = "Categories" }).GetRemainingAsync();
            var userCategory = categories.Where(s => s.UserId == userId && s.Title == categoryTitle).FirstOrDefault();

            return userCategory;
        }
    }
}
