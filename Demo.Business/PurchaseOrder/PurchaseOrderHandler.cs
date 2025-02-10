using Demo.DataContext.UintofWorks;
using Demo.DataModel.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Business.PurchaseOrder
{
    public class SavePurchaseOrderHandler : IRequestHandler<SavePurchaseOrderService, int>
    {
        private readonly IUnitofwork unitOfWork;
        public SavePurchaseOrderHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(SavePurchaseOrderService request, CancellationToken cancellationToken)
        {

            var result = await unitOfWork.PurchaseOrderRepository.SavePurchaseOrder(request);
            unitOfWork.Commit();
            return result;
        }
    }

    public class GetPurchaseOrderHandler : IRequestHandler<ListPurchaseOrderService, string>
    {
        private readonly IUnitofwork unitOfWork;
        public GetPurchaseOrderHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(ListPurchaseOrderService request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.PurchaseOrderRepository.ListPurchaseOrder(request);
            //unitOfWork.Commit();
            return result;
        }
    }
    public class FetchPurchaseOrderHandler : IRequestHandler<FetchPurchaseOrderService, Table4>
    {
        private readonly IUnitofwork unitOfWork;
        public FetchPurchaseOrderHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Table4> Handle(FetchPurchaseOrderService request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.PurchaseOrderRepository.FetchPurchaseOrder(request.po_no);
            //unitOfWork.Commit();
            return result;
        }
    }
    public class PurchaseOrderActiveUpdateHandler : IRequestHandler<PurchaseOrderActiveUpdateService, int>
    {
        private readonly IUnitofwork unitOfWork;
        public PurchaseOrderActiveUpdateHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(PurchaseOrderActiveUpdateService request, CancellationToken cancellationToken)
        {

            var result = await unitOfWork.PurchaseOrderRepository.PurchaseOrderActiveUpdate(request);
            unitOfWork.Commit();
            return result;
        }
    }
    public class GetLookupDetailsHandler : IRequestHandler<GetLookupDetailsService, string>
    {
        private readonly IUnitofwork unitOfWork;
        public GetLookupDetailsHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(GetLookupDetailsService request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.PurchaseOrderRepository.GetLookupDetails(request);
            unitOfWork.Commit();
            return result;
        }
    }
}
