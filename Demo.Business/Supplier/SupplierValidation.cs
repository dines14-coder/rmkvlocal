using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Business.Supplier
{
    public class SaveSupplierValidation : AbstractValidator<SaveSupplierService>
    {
    }
    public class ListSupplierValidation : AbstractValidator<ListSupplierService>
    {

    }
    public class FetchSupplierValidation : AbstractValidator<FetchSupplierService>
    {

    }
    public class SupplierActiveUpdateValidation : AbstractValidator<SupplierActiveUpdateService>
    {
    }
}
