





















                    

      
     
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;
using Repository.Pattern.Ef6;
using JsonServer.Models;

namespace JsonServer.Repositories
{
   public class OrderQuery:QueryObject<Order>
    {
        public OrderQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.Contains(search) || x.Orderkey.Contains(search) || x.Supplier.Contains(search) );
            return this;
        }

		public OrderQuery WithPopupSearch(string search,string para="")
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.Contains(search) || x.Orderkey.Contains(search) || x.Supplier.Contains(search) );
            return this;
        }
    }
}



