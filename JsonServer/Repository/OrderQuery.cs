using JsonServer.Models;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JsonServer.Repository
{
    public class OrderQuery : QueryObject<Order>
    {
        public OrderQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And(x => x.Id.ToString().Contains(search));
            return this;
        }
    }
}