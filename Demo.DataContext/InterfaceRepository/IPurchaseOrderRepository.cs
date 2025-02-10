using Demo.DataModel.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataContext.InterfaceRepository
{
    public interface IPurchaseOrderRepository
    {
        Task<int> SavePurchaseOrder(dynamic saveobj);
        Task<string> ListPurchaseOrder(dynamic objList);
        Task<Table4> FetchPurchaseOrder(string po_no);
        Task<int> PurchaseOrderActiveUpdate(dynamic objactive);
        Task<string> GetLookupDetails(dynamic objactive);
    }
}
