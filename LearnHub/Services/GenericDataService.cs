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
        private readonly LearnHubDbContextFactory _contextFactory;

        public GenericDataService(LearnHubDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
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
                    throw new Exception();
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
                        throw new Exception();
                    }
                    return entity;
                }
                catch (Exception)
                {
                    throw new Exception();
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
                    throw new Exception();
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
                    throw new Exception();
                }
            }
        }

        public async Task<T> Create(T entity)
        {
            if (entity == null)
            {
                throw new Exception();
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
                    throw new DbUpdateException();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }

        public async Task<T> UpdateById(string id, T entity)
        {
            if (entity == null)
            {
                throw new Exception();
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var existingEntity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                    if (existingEntity == null)
                    {
                        throw new Exception();
                    }

                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                    return existingEntity;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }

        public async Task<T> UpdateOne(Expression<Func<T, bool>> predicate, T entity)
        {
            if (entity == null)
            {
                throw new Exception();
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var existingEntity = await context.Set<T>().FirstOrDefaultAsync(predicate);
                    if (existingEntity == null)
                    {
                        throw new Exception();
                    }

                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                    return existingEntity;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }

        public async Task<int> UpdateMany(Expression<Func<T, bool>> predicate, T entity)
        {
            if (entity == null)
            {
                throw new Exception();
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entities = await context.Set<T>().Where(predicate).ToListAsync();
                    if (!entities.Any())
                    {
                        return 0;
                    }

                    foreach (var existingEntity in entities)
                    {
                        context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    }

                    return await context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException();
                }
                catch (Exception)
                {
                    throw new Exception();
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
                        throw new Exception();
                    }

                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException();
                }
                catch (Exception)
                {
                    throw new Exception();
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
                        throw new Exception();
                    }

                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException)
                {
                    throw new DbUpdateException();
                }
                catch (Exception)
                {
                    throw new Exception();
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
                    throw new DbUpdateException();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }
    }
}
