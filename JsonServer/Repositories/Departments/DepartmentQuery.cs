
                    
      
     
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
   public class DepartmentQuery:QueryObject<Department>
    {
        public DepartmentQuery WithAnySearch(string search)
        {
            if (!string.IsNullOrEmpty(search))
                And( x =>  x.Id.ToString().Contains(search) || x.Name.Contains(search) || x.Manager.Contains(search) || x.CompanyId.ToString().Contains(search) );
            return this;
        }
    }
}



