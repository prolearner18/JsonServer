﻿


                    
      
    
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Repository.Pattern.Repositories;

using JsonServer.Models;

namespace JsonServer.Repositories
{
  public static class RoleMenuRepository
    {

        public static IEnumerable<RoleMenu> GetByMenuId(this IRepositoryAsync<RoleMenu> repository, int menuid)
        {
            var query = repository
               .Queryable()
               .Where(x => x.MenuId == menuid);
            return query;

        }



    }
}



