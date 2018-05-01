
                    
      
    
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;

using JsonServer.Models;

namespace JsonServer.Repositories
{
  public static class DepartmentRepository  
    {
 
                 public static IEnumerable<Department> GetByCompanyId(this IRepositoryAsync<Department> repository, int companyid)
         {
             var query= repository
                .Queryable()
                .Where(x => x.CompanyId==companyid);
             return query;

         }
             
        
         
	}
}



