using LearnHub.Data;
using LearnHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
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
            using (LearnHubDbContext context = _contextFactory.CreateDbContext())
            {

                IEnumerable<T> entities = await context.Set<T>().ToListAsync();
                return entities;
            }
        }

        public async Task<T> Get(Guid id)
        {
            using (LearnHubDbContext context = _contextFactory.CreateDbContext())
            {

                T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
        }

        public async Task<T> Create(T entity)
        {
            using (LearnHubDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();
                return createdResult.Entity;
            }
        }

        public async Task<T> Update(Guid id, T entity)
        {
            using (LearnHubDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Id = id;
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
        public async Task<bool> Delete(Guid id)
        {
            using (LearnHubDbContext context = _contextFactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}
