using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollywoodBetsGamingCenter.Api.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> Get();
        Task<T> GetBy(string ID);
        Task<List<T>> GetByParent(string ID);
        Task<string> Upsert(T data);
    }
}
