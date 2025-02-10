using Dapper;
using Demo.DataContext.InterfaceRepository;
using Demo.DataContext.RepositoryBases;
using Demo.DataModel.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataContext.SqlRepository
{
    public class PurchaseOrderRepository : RepositoryBase, IPurchaseOrderRepository
    {

        public PurchaseOrderRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public async Task<int> SavePurchaseOrder(dynamic saveobj)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.po_no = saveobj.po_no;
            objects.po_date = saveobj.po_date;
            objects.supplier_code = saveobj.supplier_code;
            objects.grand_total = saveobj.grand_total;
            objects.entered_by = saveobj.entered_by;
            objects.po_details = saveobj.po_details;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var result = await Connection.ExecuteScalarAsync<int>($"demo.fn_purchase_order_save", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return result;
        }
        public async Task<string> ListPurchaseOrder(dynamic objList)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.from_date = objList.from_date;
            objects.to_date = objList.to_date;
            objects.supplier_code = objList.supplier_code;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var response = await Connection.QueryFirstOrDefaultAsync<Table>($"demo.fn_purchase_order_list", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return response.Records;
        }
        public async Task<Table4> FetchPurchaseOrder(string po_no)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.po_no = po_no;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var response = await Connection.QueryFirstOrDefaultAsync<Table4>($"demo.fn_purchase_order_fetch",
                values, commandType: CommandType.StoredProcedure, transaction: Transaction);
            return response;
        }
        public async Task<int> PurchaseOrderActiveUpdate(dynamic objactive)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.po_no = objactive.po_no;
            objects.active = objactive.active;
            objects.entered_by = objactive.entered_by;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var result = await Connection.ExecuteScalarAsync<int>($"demo.fn_purchase_order_active_update", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return result;
        }

        public async Task<string> GetLookupDetails(dynamic objactive)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.item_code = objactive.item_code;
            objects.supplier_code = objactive.supplier_code;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var result = await Connection.QueryFirstOrDefaultAsync<Table>($"demo.fn_get_lookup_details_by_code", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return result.Records;
        }
    }
}
