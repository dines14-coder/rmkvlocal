using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Business.PurchaseOrder
{
    public class SavePurchaseOrderValidation : AbstractValidator<SavePurchaseOrderService>
    {
    }
    public class ListPurchaseOrderValidation : AbstractValidator<ListPurchaseOrderService>
    {

    }
    public class FetchPurchaseOrderValidation : AbstractValidator<FetchPurchaseOrderService>
    {

    }
    public class PurchaseOrderActiveUpdateValidation : AbstractValidator<PurchaseOrderActiveUpdateService>
    {
    }
    public class GetLookupDetailsValidation: AbstractValidator<GetLookupDetailsService>
    {

    }
}
