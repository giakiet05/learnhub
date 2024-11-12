using LearnHub.Data;
using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearnHub.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        private static readonly Lazy<GenericDataService<T>> _instance = new Lazy<GenericDataService<T>>(() => new GenericDataService<T>());
        private readonly LearnHubDbContextFactory _contextFactory;

        // Singleton instance property
        public static GenericDataService<T> Instance => _instance.Value;

        // Private constructor to ensure the service is only instantiated once
        private GenericDataService()
        {
            _contextFactory = LearnHubDbContextFactory.Instance;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    return await context.Set<T>().ToListAsync();
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while retrieving all entities.");
                }
            }
        }

        public async Task<T> GetById(string id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                    if (entity == null)
                    {
                        throw new Exception("Entity not found.");
                    }
                    return entity;
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while retrieving the entity.");
                }
            }
        }

        public async Task<T> GetOne(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    return await context.Set<T>().FirstOrDefaultAsync(predicate);
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while retrieving the entity.");
                }
            }
        }

        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    return await context.Set<T>().Where(predicate).ToListAsync();
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while retrieving multiple entities.");
                }
            }
        }

        public async Task<T> Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var createdResult = await context.Set<T>().AddAsync(entity);
                    await context.SaveChangesAsync();
                    return createdResult.Entity;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while saving the entity.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while creating the entity.");
                }
            }
        }

        public async Task<T> UpdateById(string id, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var existingEntity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                    if (existingEntity == null)
                    {
                        throw new Exception("Entity not found.");
                    }

                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                    return existingEntity;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while updating the entity.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while updating the entity.");
                }
            }
        }

        public async Task<T> UpdateOne(Expression<Func<T, bool>> predicate, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var existingEntity = await context.Set<T>().FirstOrDefaultAsync(predicate);
                    if (existingEntity == null)
                    {
                        throw new Exception("Entity not found.");
                    }

                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                    return existingEntity;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while updating the entity.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while updating the entity.");
                }
            }
        }

        public async Task<int> UpdateMany(Expression<Func<T, bool>> predicate, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entities = await context.Set<T>().Where(predicate).ToListAsync();
                    if (!entities.Any()) return 0;

                    foreach (var existingEntity in entities)
                    {
                        context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    }

                    return await context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while updating multiple entities.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while updating multiple entities.");
                }
            }
        }

        public async Task<bool> DeleteById(string id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                    if (entity == null)
                    {
                        throw new Exception("Entity not found.");
                    }

                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while deleting the entity.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while deleting the entity.");
                }
            }
        }

        public async Task<bool> DeleteOne(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entity = await context.Set<T>().FirstOrDefaultAsync(predicate);
                    if (entity == null)
                    {
                        throw new Exception("Entity not found.");
                    }

                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while deleting the entity.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while deleting the entity.");
                }
            }
        }

        public async Task<int> DeleteMany(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entities = await context.Set<T>().Where(predicate).ToListAsync();
                    if (!entities.Any()) return 0;

                    context.Set<T>().RemoveRange(entities);
                    return await context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException("An error occurred while deleting multiple entities.");
                }
                catch (Exception)
                {
                    throw new Exception("An error occurred while deleting multiple entities.");
                }
            }
        }
    }
}
