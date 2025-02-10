using Demo.DataContext.UintofWorks;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Business.ItemMaster
{
    public class SaveItemMasterHandler : IRequestHandler<SaveItemMasterService, int>
    {
        private readonly IUnitofwork unitOfWork;
        public SaveItemMasterHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(SaveItemMasterService request, CancellationToken cancellationToken)
        {

            var result = await unitOfWork.ItemMasterRepository.SaveItemMaster(request);
            unitOfWork.Commit();
            return result;
        }
    }

    public class GetItemMasterHandler : IRequestHandler<ListItemMasterService, string>
    {
        private readonly IUnitofwork unitOfWork;
        public GetItemMasterHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(ListItemMasterService request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.ItemMasterRepository.ListItemMaster(request.active);
            //unitOfWork.Commit();
            return result;
        }
    }
    public class FetchItemMasterHandler : IRequestHandler<FetchItemMasterService, string>
    {
        private readonly IUnitofwork unitOfWork;
        public FetchItemMasterHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<string> Handle(FetchItemMasterService request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.ItemMasterRepository.FetchItemMaster(request.item_code);
            //unitOfWork.Commit();
            return result;
        }
    }
    public class ItemMasterActiveUpdateHandler : IRequestHandler<ItemMasterActiveUpdateService, int>
    {
        private readonly IUnitofwork unitOfWork;
        public ItemMasterActiveUpdateHandler(IUnitofwork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(ItemMasterActiveUpdateService request, CancellationToken cancellationToken)
        {

            var result = await unitOfWork.ItemMasterRepository.ItemMasterActiveUpdate(request);
            unitOfWork.Commit();
            return result;
        }
    }
}
