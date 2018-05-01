
                    
      
    
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;

using JsonServer.Models;

namespace JsonServer.Repositories
{
  public static class CategoryRepository  
    {
 
        
                public static IEnumerable<JsonServer.Models.Product>   GetProductsByCategoryId (this IRepositoryAsync<Category> repository,int categoryid)
        {
			var productRepository = repository.GetRepository<JsonServer.Models.Product>(); 
            return productRepository.Queryable().Include(x => x.Category).Where(n => n.CategoryId == categoryid);
        }
         
	}
}



