﻿using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {

        private readonly MyRecipeBookDbContext _dbContext;

        public UnitOfWork(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
