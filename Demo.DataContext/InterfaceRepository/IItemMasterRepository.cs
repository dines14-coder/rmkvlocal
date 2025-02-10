using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataContext.InterfaceRepository
{
    public interface IItemMasterRepository
    {
        Task<int> SaveItemMaster(dynamic saveobj);
        Task<string> ListItemMaster(int active);
        Task<string> FetchItemMaster(string item_code);
        Task<int> ItemMasterActiveUpdate(dynamic objactive);
    }
}
