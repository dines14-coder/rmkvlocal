using Demo.DataModel.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Business.PurchaseOrder
{
    public class SavePurchaseOrderService : IRequest<int>
    {
        public string po_no { get; set; }
        public string po_date { get; set; }
        public decimal? grand_total { get; set; }
        public string supplier_code { get; set; }
        public List<PurchaseOrderDetailsEntity> po_details { get; set; }
        public int entered_by { get; set; }
    }
    public class ListPurchaseOrderService : IRequest<string>
    {
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string supplier_code { get; set; }
    }
    public class FetchPurchaseOrderService : IRequest<Table4>
    {
        public string po_no { get; set; }
    }
    public class PurchaseOrderActiveUpdateService : IRequest<int>
    {
        public string po_no { get; set; }
        public bool active { get; set; }
        public int entered_by { get; set; }
    }
    public class GetLookupDetailsService: IRequest<string>
    {
        public string item_code { get; set; }
        public string supplier_code { get; set; }
    }
}
