using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface ICrud<T, TKey>
    {
        Task Add(T model);

        Task Delete(T model);

        Task Update(T model);

        Task GetById(TKey id);

        Task GetAll();
    }
}