using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Entities.FrameWork;

namespace PrivateTraining.BussinessLayer.Framework
{
    public static class Menus
    {
        public static string LayoutMenus(IList<Menu> Menus)
        {
            var TempMenu = Menus;
            if (Menus.Count() > 0)
            {
                string Menu = "[";
                foreach (var item in Menus.Where(c => c.ParentId == 0 && c.IsEnable == true))
                {
                    Menu += "{\"label\": \"" + item.Name + "\",\"iconClasses\": \"\"";

                    if (TempMenu.Where(c => c.ParentId == item.Id).Count() == 0)
                        Menu += ",\"url\": \"#/" + item.Action.Actionname +"\"";
                    else
                        Menu += ",\"hideOnHorizontal\": true";

                    Menu +=  LayoutMenusChilderen(TempMenu.Where(c => c.ParentId == item.Id).ToList(), TempMenu);

                    Menu += "}, ";


                }
                Menu = Menu.Substring(0, Menu.Length - 2);
                Menu += "]";
                return Menu;
            }
            else
            {
                return "";
            }

        }

        public static string LayoutMenusChilderen(IList<Menu> Menus, IList<Menu> TempMenus)
        {
            var tempSubMenu = TempMenus;
            if (Menus.Count() > 0)
            {
                string Menu = ",\"children\":[";
                foreach (var item in Menus.Where(c => c.ParentId != 0 && c.IsEnable == true))
                {
                    Menu += "{\"label\": \"" + item.Name + "\",\"iconClasses\": \"\",\"url\": \"#/" + item.Action.Area + "/" + item.Action.Controller + "/" + item.Action.Actionname +
                            "\"" + LayoutMenusChilderen(TempMenus.Where(c => c.ParentId == item.Id).ToList(), tempSubMenu) + "}, ";
                }
                Menu = Menu.Substring(0, Menu.Length - 2);
                Menu += "]";
                return Menu;
            }
            else
            {
                return "";
            }

        }

    }
}
