using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface ICrud<T>
    {
        Task Add(T model);

        Task Delete(T model);

        Task Update(T model);

        Task GetAll();
    }
}