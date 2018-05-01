
             
           
 

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
    public class CompanyService : Service< Company >, ICompanyService
    {

        private readonly IRepositoryAsync<Company> _repository;
        public  CompanyService(IRepositoryAsync< Company> repository)
            : base(repository)
        {
            _repository=repository;
        }
        
                         public IEnumerable<Department>   GetDepartmentsByCompanyId (int companyid)
        {
            return _repository.GetDepartmentsByCompanyId(companyid);
        }
                public IEnumerable<Employee>   GetEmployeeByCompanyId (int companyid)
        {
            return _repository.GetEmployeeByCompanyId(companyid);
        }
         
        
    }
}



