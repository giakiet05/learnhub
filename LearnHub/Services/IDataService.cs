using LearnHub.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LearnHub.Services
{
    public interface IDataService<T> where T: class
    //các phương thức dùng predicate có thể áp dụng cho các bảng có composite key
    //param predicate là một biểu thức lambda
    {
        // Retrieval methods
        Task<IEnumerable<T>> GetAll(); // lấy toàn bộ bảng
       // Task<T> GetById(string id); // lấy bằng id
        Task<T> GetOne(Expression<Func<T, bool>> predicate); //lấy 1 đối tượng thỏa điều kiện
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> predicate); // lấy tất cả đối tượng thỏa điều kiện

        // Creation method
        Task<T> Create(T entity); // tạo đối tượng

        // Update methods
       // Task<T> UpdateById(string id, T entity); // tìm bằng id và sửa
        Task<T> UpdateOne(T entity, Expression<Func<T, bool>> predicate); // tìm bằng điều kiện và sứa 1 đối tượng
        Task<int> UpdateMany(T entity, Expression<Func<T, bool>> predicate); // tìm bằng điều kiện và sửa nhiều đối tượng

        // Deletion methods
        //Task<bool> DeleteById(string id); // xóa bằng id
        Task<bool> DeleteOne(Expression<Func<T, bool>> predicate); //xóa 1 đối tượng thỏa điều kiện
        Task<int> DeleteMany(Expression<Func<T, bool>> predicate); // xóa tất cả đối tượng thỏa điều kiện
    }
}

