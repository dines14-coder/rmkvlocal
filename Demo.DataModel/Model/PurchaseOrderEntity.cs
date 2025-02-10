using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.DataModel.Model
{
    public class PurchaseOrderDetailsEntity
    {
        public string item_code { get; set; }
        public decimal? quantity { get; set; }
        public decimal? rate { get; set; }
        public decimal? total_amount { get; set; }

    }
}
