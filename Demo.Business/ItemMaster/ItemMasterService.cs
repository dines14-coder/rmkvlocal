using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Business.ItemMaster
{
    public class SaveItemMasterService : IRequest<int>
    {
        public string item_code { get; set; }
        public string item_description { get; set; }
        public string uom { get; set; }
        public int entered_by { get; set; }
    }
    public class ListItemMasterService : IRequest<string>
    {
        public int active { get; set; }
    }
    public class FetchItemMasterService : IRequest<string>
    {
        public string item_code { get; set; }
    }
    public class ItemMasterActiveUpdateService : IRequest<int>
    {
        public string item_code { get; set; }
        public bool active { get; set; }
        public int entered_by { get; set; }
    }
}
