using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JsonServer.Models;
using Service.Pattern;

namespace JsonServer.Services
{
    public interface IOrderService :IService<Order>
    {
    }
}