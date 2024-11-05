using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearnHub.Services
{
    public interface IDataService<T>
    //các phương thức dùng predicate có thể áp dụng cho các bảng có composite key
    {
        // Retrieval methods
        Task<IEnumerable<T>> GetAllAsync(); // lấy toàn bộ bảng
        Task<T> GetByIdAsync(Guid id); // lấy bằng id
        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate); //lấy 1 đối tượng thỏa điều kiện
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate); // lấy tất cả đối tượng thỏa điều kiện

        // Creation method
        Task<T> CreateAsync(T entity); // tạo đối tượng

        // Update methods
        Task<T> UpdateByIdAsync(Guid id, T entity); // tìm bằng id và sửa
        Task<T> UpdateOneAsync(Expression<Func<T, bool>> predicate, T entity); // tìm bằng điều kiện và sứa 1 đối tượng
        Task<int> UpdateManyAsync(Expression<Func<T, bool>> predicate, T entity); // tìm bằng điều kiện và sửa nhiều đối tượng

        // Deletion methods
        Task<bool> DeleteByIdAsync(Guid id); // xóa bằng id
        Task<bool> DeleteOneAsync(Expression<Func<T, bool>> predicate); //xóa 1 đối tượng thỏa điều kiện
        Task<int> DeleteManyAsync(Expression<Func<T, bool>> predicate); // xóa tất cả đối tượng thỏa điều kiện
    }
}

