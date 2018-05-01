
     
 
 
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
    public interface IRoleMenuService:IService<RoleMenu>
    {

                  IEnumerable<RoleMenu> GetByMenuId(int  menuid);

                  IEnumerable<RoleMenu> GetByRoleName(string roleName);
                  void UpdateMenus(RoleMenusView[] list);

                  IEnumerable<MenuItem> RenderMenus(string[] roleNames);
      
 
	}
}