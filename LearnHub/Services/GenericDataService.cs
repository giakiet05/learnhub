﻿using LearnHub.Data;
using LearnHub.Exceptions;
using LearnHub.Models;
using LearnHub.Stores;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearnHub.Services
{
    public class GenericDataService<T> where T : class, IAdminId
    {
        private static readonly Lazy<GenericDataService<T>> _instance = new Lazy<GenericDataService<T>>(() => new GenericDataService<T>());
        private readonly LearnHubDbContextFactory _contextFactory;
      
        // Singleton instance property
        public static GenericDataService<T> Instance => _instance.Value;
        

        private GenericDataService()
        {
            _contextFactory = LearnHubDbContextFactory.Instance;
         
        }


        public async Task<IEnumerable<TResult>> Query<TResult>(
    Func<DbSet<T>, IQueryable<TResult>> query)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    // Thực hiện truy vấn tùy chỉnh
                    return await query(context.Set<T>()).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }


        public async Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    IQueryable<T> query = context.Set<T>().Where(e => e.AdminId == AccountStore.Instance.CurrentUser.Id);

                    // Apply include if provided
                    if (include != null)
                    {
                        query = include(query);
                    }

                    return await query.ToListAsync(); // Sử dụng query đã được thay đổi bởi include
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }


        public async Task<T> GetOne(
    Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    IQueryable<T> query = context.Set<T>().Where(e => e.AdminId == AccountStore.Instance.CurrentUser.Id);

                    // Apply include if provided
                    if (include != null)
                    {
                        query = include(query);
                    }

                    return await query.FirstOrDefaultAsync(predicate);
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }


        public async Task<IEnumerable<T>> GetMany(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    IQueryable<T> query = context.Set<T>().Where(e => e.AdminId == AccountStore.Instance.CurrentUser.Id);

                    // Apply include if provided
                    if (include != null)
                    {
                        query = include(query);
                    }

                    return await query.Where(predicate).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }


        public async Task<T> CreateOne(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Đối tượng không tồn tại");
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var createdResult = await context.Set<T>().AddAsync(entity);
                    await context.SaveChangesAsync();
                    return createdResult.Entity;
                }
                catch (DbUpdateException dbEx)
                {
                    if (dbEx.InnerException is SqliteException sqliteEx)
                    {
                        switch (sqliteEx.SqliteErrorCode)
                        {
                            case 19: // Constraint violation
                                if (sqliteEx.Message.Contains("UNIQUE"))
                                {
                                    throw new UniqueConstraintException("Trùng khóa chính hoặc thuộc tính unique", dbEx);
                                }
                                if (sqliteEx.Message.Contains("CHECK"))
                                {
                                    throw new CheckConstraintException("Vi phạm check constraint", dbEx);
                                }
                                break;
                        }
                    }

                    throw new Exception("Có lỗi khi thao tác với database", dbEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }





        public async Task<IEnumerable<T>> CreateMany(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                throw new ArgumentException("Mảng rỗng hoặc không tồn tại", nameof(entities));
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    await context.Set<T>().AddRangeAsync(entities); // Add all entities at once
                    await context.SaveChangesAsync(); // Save changes to the database
                    return entities; // Return the input collection as the entities are now tracked
                }
                catch (DbUpdateException dbEx)
                {
                    if (dbEx.InnerException is SqliteException sqliteEx)
                    {
                        switch (sqliteEx.SqliteErrorCode)
                        {
                            case 19: // Constraint violation
                                if (sqliteEx.Message.Contains("UNIQUE"))
                                {
                                    throw new UniqueConstraintException("Trùng khóa chính hoặc thuộc tính unique", dbEx);
                                }
                                if (sqliteEx.Message.Contains("CHECK"))
                                {
                                    throw new CheckConstraintException("Vi phạm check constraint", dbEx);
                                }
                                break;
                        }
                    }

                    throw new Exception("Có lỗi khi thao tác với database", dbEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }


        public async Task<T> UpdateOne(T entity, Expression<Func<T, bool>> predicate)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Đối tượng không tồn tại");
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var existingEntity = await context.Set<T>().Where(e => e.AdminId == AccountStore.Instance.CurrentUser.Id).FirstOrDefaultAsync(predicate);
                    if (existingEntity == null)
                    {
                        throw new Exception("Đối tượng không tồn tại");
                    }

                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    await context.SaveChangesAsync();
                    return existingEntity;
                }
                catch (DbUpdateException dbEx)
                {
                    if (dbEx.InnerException is SqliteException sqliteEx)
                    {
                        switch (sqliteEx.SqliteErrorCode)
                        {
                            case 19: // Constraint violation
                                if (sqliteEx.Message.Contains("UNIQUE"))
                                {
                                    throw new UniqueConstraintException("Trùng khóa chính hoặc thuộc tính unique", dbEx);
                                }
                                if (sqliteEx.Message.Contains("CHECK"))
                                {
                                    throw new CheckConstraintException("Vi phạm check constraint", dbEx);
                                }
                                break;
                        }
                    }

                    throw new Exception("Có lỗi khi thao tác với database", dbEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }

        public async Task<int> UpdateMany(T entity, Expression<Func<T, bool>> predicate)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Đối tượng không tồn tại");
            }

            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entities = await context.Set<T>().Where(predicate).Where(e => e.AdminId == AccountStore.Instance.CurrentUser.Id).ToListAsync();
                    if (!entities.Any()) return 0;

                    foreach (var existingEntity in entities)
                    {
                        context.Entry(existingEntity).CurrentValues.SetValues(entity);
                    }

                    return await context.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    if (dbEx.InnerException is SqliteException sqliteEx)
                    {
                        switch (sqliteEx.SqliteErrorCode)
                        {
                            case 19: // Constraint violation
                                if (sqliteEx.Message.Contains("UNIQUE"))
                                {
                                    throw new UniqueConstraintException("Trùng khóa chính hoặc thuộc tính unique", dbEx);
                                }
                                if (sqliteEx.Message.Contains("CHECK"))
                                {
                                    throw new CheckConstraintException("Vi phạm check constraint", dbEx);
                                }
                                break;
                        }
                    }

                    throw new Exception("Có lỗi khi thao tác với database", dbEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }


        public async Task<bool> DeleteOne(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var entity = await context.Set<T>().Where(e => e.AdminId == AccountStore.Instance.CurrentUser.Id).FirstOrDefaultAsync(predicate);
                    if (entity == null)
                    {
                        throw new EntityNotFoundException("Không tìm thấy đối tượng");
                    }

                    context.Set<T>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException dbEx) when (dbEx.InnerException is SqliteException sqliteEx)
                {
                    if (sqliteEx.SqliteErrorCode == 19 && sqliteEx.Message.Contains("FOREIGN KEY"))
                        throw new ForeignKeyConstraintException("Không thể xóa do có dữ liệu liên quan.", dbEx);

                    throw new Exception("Có lỗi xảy ra khi thao tác với database", dbEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }

        public async Task<int> DeleteMany(Expression<Func<T, bool>> predicate)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                try
                {

                    var entities = await context.Set<T>().Where(e => e.AdminId == AccountStore.Instance.CurrentUser.Id).Where(predicate).ToListAsync();
                    if (!entities.Any()) throw new EntityNotFoundException("Không tìm thấy đối tượng");

                    context.Set<T>().RemoveRange(entities);
                    return await context.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx) when (dbEx.InnerException is SqliteException sqliteEx)
                {
                    if (sqliteEx.SqliteErrorCode == 19 && sqliteEx.Message.Contains("FOREIGN KEY"))
                        throw new ForeignKeyConstraintException("Không thể xóa do có dữ liệu liên quan.", dbEx);

                    throw new Exception("Có lỗi xảy ra khi thao tác với database", dbEx);
                }
                catch (Exception ex)
                {
                    throw new Exception("Có lỗi xảy ra", ex);
                }
            }
        }


    }
}
