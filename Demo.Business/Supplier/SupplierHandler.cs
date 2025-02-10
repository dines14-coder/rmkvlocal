using Demo.DataContext.UintofWorks;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Business.Supplier
{
    public class SaveSupplierHandler : IRequestHandler<SaveSupplierService, int>
    {
        private readonly IUnitofwork unitOfWork;
        public SaveSupplierHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(SaveSupplierService request, CancellationToken cancellationToken)
        {

            var result = await unitOfWork.SupplierRepository.SaveSupplier(request);
            unitOfWork.Commit();
            return result;
        }
    }

    public class GetSupplierHandler : IRequestHandler<ListSupplierService, string>
    {
        private readonly IUnitofwork unitOfWork;
        public GetSupplierHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(ListSupplierService request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.SupplierRepository.ListSupplier(request.active);
            //unitOfWork.Commit();
            return result;
        }
    }
    public class FetchSupplierHandler : IRequestHandler<FetchSupplierService, string>
    {
        private readonly IUnitofwork unitOfWork;
        public FetchSupplierHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(FetchSupplierService request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.SupplierRepository.FetchSupplier(request.supplier_code);
            //unitOfWork.Commit();
            return result;
        }
    }
    public class SupplierActiveUpdateHandler : IRequestHandler<SupplierActiveUpdateService, int>
    {
        private readonly IUnitofwork unitOfWork;
        public SupplierActiveUpdateHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(SupplierActiveUpdateService request, CancellationToken cancellationToken)
        {

            var result = await unitOfWork.SupplierRepository.SupplierActiveUpdate(request);
            unitOfWork.Commit();
            return result;
        }
    }
}
