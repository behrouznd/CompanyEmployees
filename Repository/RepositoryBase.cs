﻿using Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext repositoryContext;

        public RepositoryBase(RepositoryContext repository) => this.repositoryContext = repository;

        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ?
             repositoryContext.Set<T>().AsNoTracking() :
             repositoryContext.Set<T>();


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression , bool trackChanges)=>
            !trackChanges ?
             repositoryContext.Set<T>().Where(expression).AsNoTracking() :
             repositoryContext.Set<T>().Where(expression);
         


        public void Create(T entity) => repositoryContext.Set<T>().Add(entity);
         

        public void Delete(T entity) => repositoryContext.Set<T>().Remove(entity);
          

        public void Update(T entity) => repositoryContext.Set<T>().Update(entity);
        
    }
}
