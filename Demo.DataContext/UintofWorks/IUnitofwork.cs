using Demo.DataContext.InterfaceRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.DataContext.UintofWorks
{
   public interface IUnitofwork: IDisposable
    {   
        IItemMasterRepository ItemMasterRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IPurchaseOrderRepository PurchaseOrderRepository { get; }
        void Commit();
    }
}
