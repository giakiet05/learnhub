using LearnHub.Data;
using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearnHub.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        private readonly LearnHubDbContextFactory _contextFactory;

        public GenericDataService(LearnHubDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().ToListAsync();
            }
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            }
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().FirstOrDefaultAsync(predicate);
            }
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().Where(predicate).ToListAsync();
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return createdResult.Entity;
            }
        }

        public async Task<T> UpdateByIdAsync(Guid id, T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {

                var existingEntity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                if (existingEntity == null) return null;
                context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
                return existingEntity;
            }
        }


        public async Task<T> UpdateOneAsync(Expression<Func<T, bool>> predicate, T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existingEntity = await context.Set<T>().FirstOrDefaultAsync(predicate);
                if (existingEntity == null) return null;

                context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
                return existingEntity;
            }
        }

        public async Task<int> UpdateManyAsync(Expression<Func<T, bool>> predicate, T entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entities = await context.Set<T>().Where(predicate).ToListAsync();
                if (!entities.Any()) return 0;

                foreach (var existingEntity in entities)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                }

                return await context.SaveChangesAsync();
            }
        }
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                if (entity == null) return false;

                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteOneAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entity = await context.Set<T>().FirstOrDefaultAsync(predicate);
                if (entity == null) return false;

                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<int> DeleteManyAsync(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var entities = await context.Set<T>().Where(predicate).ToListAsync();
                if (!entities.Any()) return 0;

                context.Set<T>().RemoveRange(entities);
                return await context.SaveChangesAsync();
            }
        }
    }
}
