﻿

     
 
 
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
    public interface ICompanyService:IService<Company>
    {

         
                 IEnumerable<Department>   GetDepartmentsByCompanyId (int companyid);
         
                 IEnumerable<Employee>   GetEmployeeByCompanyId (int companyid);
         
         
 
	}
}