using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RouteDelivery.Models;

namespace RouteDelivery.Data
{
    public interface IRepository<T>
    {
        void Add(T newEntity);
        void Remove(T entity);
        void Update(T entity);

        List<T> Find(Func<T, bool> match);
        List<T> FindAll();
        T FindByID(int id);
        void AddRange(List<T> range);
    }
}
