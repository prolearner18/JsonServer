using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JsonServer.Models;
using Service.Pattern;
using Repository.Pattern.Repositories;

namespace JsonServer.Services
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IRepositoryAsync<Order> _repository;
        public OrderService(IRepositoryAsync<Order> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}