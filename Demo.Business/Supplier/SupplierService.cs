using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Business.Supplier
{
    public class SaveSupplierService : IRequest<int>
    {
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string supplier_gst_no { get; set; }
        public int entered_by { get; set; }
    }
    public class ListSupplierService : IRequest<string>
    {
        public int active { get; set; }
    }
    public class FetchSupplierService : IRequest<string>
    {
        public string supplier_code { get; set; }
    }
    public class SupplierActiveUpdateService : IRequest<int>
    {
        public string supplier_code { get; set; }
        public bool active { get; set; }
        public int entered_by { get; set; }
    }
}
