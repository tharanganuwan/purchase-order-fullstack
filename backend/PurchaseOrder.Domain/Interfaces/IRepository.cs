﻿namespace PurchaseOrder.Domain.Interfaces
{
    public interface IRepository
    {
        public interface IRepository<T> where T : class
        {
            Task<T?> GetByIdAsync(Guid id);
            Task AddAsync(T entity);
            void Update(T entity);
            void Remove(T entity);
            IQueryable<T> Query();
            Task SaveChangesAsync();
        }
    }
}
