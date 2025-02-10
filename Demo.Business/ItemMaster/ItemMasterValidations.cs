using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Business.ItemMaster
{
    public class SaveItemMasterValidation : AbstractValidator<SaveItemMasterService>
    {
    }
    public class ListItemMasterValidation : AbstractValidator<ListItemMasterService>
    {

    }
    public class FetchItemMasterValidation : AbstractValidator<FetchItemMasterService>
    {

    }
    public class ItemMasterActiveUpdateValidation : AbstractValidator<ItemMasterActiveUpdateService>
    {
    }
}
