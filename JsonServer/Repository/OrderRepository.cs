using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JsonServer.Models;
using Repository.Pattern.Repositories;

namespace JsonServer.Repository
{
    public static class OrderRepository
    {
        public static IEnumerable<Order> GetLabById(this IRepositoryAsync<Order> repository, int id)
        {
            var query = repository
               .Queryable()
               .Where(x => x.Id == id );
            return query;

        }
    }
}