using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataContext.InterfaceRepository
{
    public interface ISupplierRepository
    {
        Task<int> SaveSupplier(dynamic saveobj);
        Task<string> ListSupplier(int active);
        Task<string> FetchSupplier(string supplier_code);
        Task<int> SupplierActiveUpdate(dynamic objactive);
    }
}
