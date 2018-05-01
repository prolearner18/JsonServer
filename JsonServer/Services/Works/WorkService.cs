
             
           
 

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
    public class WorkService : Service< Work >, IWorkService
    {

        private readonly IRepositoryAsync<Work> _repository;
        public  WorkService(IRepositoryAsync< Work> repository)
            : base(repository)
        {
            _repository=repository;
        }
        
                  
        
    }
}



