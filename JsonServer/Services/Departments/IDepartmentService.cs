

     
 
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;
using JsonServer.Models;
using JsonServer.Repositories;

namespace JsonServer.Services
{
    public interface IDepartmentService:IService<Department>
    {

                  IEnumerable<Department> GetByCompanyId(int  companyid);
        
         
 
	}
}