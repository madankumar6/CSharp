using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TMS.DAL;
using System;
using Tracker.Common;
using System.Linq.Expressions;
using TMS.Model;
using System.Reflection;

namespace TMS.Console
{
    class Program
    {
        static TMSEntities entities;

        static void Main(string[] args)
        {
            entities = new TMSEntities();
            //var menus = (from m in entities.Menus
            //    join r in entities.RoleMenus on m.MenuId equals r.MenuId
            //    where r.RoleId == 1 && r.Enabled == true && m.ParentMenuId == null
            //    select m).Include(i => i.Children).ToList();

            entities.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            var result = entities.Menus.Join(entities.RoleMenus, m => m.MenuId, r => r.MenuId, (menu, role) => new { menuList = menu }).SelectMany(i => i.menuList.MenuItems).Where(i => i.ParentMenuId == null).Include(j => j.Children).ToList();

            var pagedResult = GetMany(1, 10);
            pagedResult = GetMany(2, 10);
            pagedResult = GetMany(3, 10);

            pagedResult = GetMany(m => m.MenuItemId > 20,1, 10);
            pagedResult = GetMany(m => m.MenuItemId > 30, 1, 10);
            pagedResult = GetMany(m => m.MenuItemId > 10, 1, 10);
            System.Console.ReadLine();
        }

        public static CollectionPage<MenuItem> GetMany(int page, int itemsPerPage)
        {
            return GetMany(null, page, itemsPerPage);


            //var pageOfResult = new CollectionPage<MenuItem>()
            //{
            //    TotalItems = entities.MenuItems.Count(),
            //    ItemsPerPage = itemsPerPage,
            //    Items = entities.MenuItems.OrderBy(m => m.MenuItemId).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
            //};
            //return pageOfResult;
        }


        public static CollectionPage<MenuItem> GetMany(Expression<Func<MenuItem, bool>> where, int page, int itemsPerPage)
        {
            IQueryable<MenuItem> query = entities.MenuItems;

            if (where != null)
            {
               query = query.Where(where);
            }
            
            query = ((IOrderedQueryable<MenuItem>)query).OrderBy(item => (true));

            var pageOfResult = new CollectionPage<MenuItem>()
            {
                TotalItems = query.Count(),
                ItemsPerPage = itemsPerPage,
                Items = query.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList()
            };
            return pageOfResult;
        }
    }
}
