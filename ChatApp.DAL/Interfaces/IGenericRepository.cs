﻿namespace ChatApp.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T?> GetById(Guid id);
        T Find(Func<T, Boolean> predicate);
        Task<bool> Add(T item);
        bool Remove(T item);
    }
}