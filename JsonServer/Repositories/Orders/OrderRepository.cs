
                    
      
    
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;

using JsonServer.Models;

namespace JsonServer.Repositories
{
  public static class OrderRepository  
    {
 
        
        public static IEnumerable<OrderDetail>   GetOrderDetailsByOrderId (this IRepositoryAsync<Order> repository,int orderid)
        {
            return repository.Query(n => n.Id == orderid).Include(x => x.OrderDetails).Include(x=>x.OrderDetails.Select(y=>y.Product)).Select().First().OrderDetails;
        }
         
	}
}



