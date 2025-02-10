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
    class SupplierRepository : RepositoryBase, ISupplierRepository
    {

        public SupplierRepository(IDbTransaction transaction) : base(transaction)
        {
        }


        public async Task<int> SaveSupplier(dynamic saveobj)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.supplier_code = saveobj.supplier_code;
            objects.supplier_name = saveobj.supplier_name;
            objects.supplier_gst_no = saveobj.supplier_gst_no;
            objects.entered_by = saveobj.entered_by;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var result = await Connection.ExecuteScalarAsync<int>($"demo.fn_supplier_save", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return result;
        }
        public async Task<string> ListSupplier(int active)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.active = active;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var response = await Connection.QueryFirstOrDefaultAsync<Table>($"demo.fn_supplier_list", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return response.Records;
        }
        public async Task<string> FetchSupplier(string supplier_code)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.supplier_code = supplier_code;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var response = await Connection.QueryFirstOrDefaultAsync<Table>($"demo.fn_supplier_fetch",
                values, commandType: CommandType.StoredProcedure, transaction: Transaction);
            return response.Records;
        }
        public async Task<int> SupplierActiveUpdate(dynamic objactive)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.supplier_code = objactive.supplier_code;
            objects.active = objactive.active;
            objects.entered_by = objactive.entered_by;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var result = await Connection.ExecuteScalarAsync<int>($"demo.fn_supplier_active_update", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return result;
        }
    }
}
