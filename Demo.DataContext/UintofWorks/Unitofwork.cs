using Npgsql;
using Demo.DataContext.InterfaceRepository;
using Demo.DataContext.SqlRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Demo.DataContext.UintofWorks
{
    public class Unitofwork : IUnitofwork
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        private bool disposed;
        private IPurchaseOrderRepository purchaseOrderRepository;
        private IItemMasterRepository itemMasterRepository;
        private ISupplierRepository supplierRepository;
        public Unitofwork(string ConnectionString)
        {
            connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        public IItemMasterRepository ItemMasterRepository
        {
            get { return itemMasterRepository ?? (itemMasterRepository = new ItemMasterRepository(transaction)); }
        }
        public ISupplierRepository SupplierRepository
        {
            get { return supplierRepository ?? (supplierRepository = new SupplierRepository(transaction)); }
        }
        public IPurchaseOrderRepository PurchaseOrderRepository
        {
            get { return purchaseOrderRepository ?? (purchaseOrderRepository = new PurchaseOrderRepository(transaction)); }
        }
        

        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch (Exception Ex)
            {
                transaction.Rollback();
                throw Ex;
            }
            finally
            {
                transaction.Dispose();
                transaction = connection.BeginTransaction();
                resetRepositories();
            }
        }

        private void resetRepositories()
        {
           
            itemMasterRepository = null;
            supplierRepository = null;
            purchaseOrderRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }
        private void dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }
                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }
            }
            disposed = true;
        }
        ~Unitofwork()
        {
            dispose(false);
        }
    }
}
