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
    public class ItemMasterRepository : RepositoryBase, IItemMasterRepository
    {

        public ItemMasterRepository(IDbTransaction transaction) : base(transaction)
        {
        }
        public async Task<int> SaveItemMaster(dynamic saveobj)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.item_code = saveobj.item_code;
            objects.item_description = saveobj.item_description;
            objects.uom = saveobj.uom;
            objects.entered_by = saveobj.entered_by;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var result = await Connection.ExecuteScalarAsync<int>($"demo.fn_item_master_save", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return result;
        }
        public async Task<string> ListItemMaster(int active)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.active = active;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var response = await Connection.QueryFirstOrDefaultAsync<Table>($"demo.fn_item_master_list", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return response.Records;
        }
        public async Task<string> FetchItemMaster(string item_code)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.item_code = item_code;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var response = await Connection.QueryFirstOrDefaultAsync<Table>($"demo.fn_item_master_fetch",
                values, commandType: CommandType.StoredProcedure, transaction: Transaction);
            return response.Records;
        }
        public async Task<int> ItemMasterActiveUpdate(dynamic objactive)
        {
            DynamicParameters values = new DynamicParameters();
            var array = new List<dynamic>();
            dynamic objects = new System.Dynamic.ExpandoObject();
            objects.item_code = objactive.item_code;
            objects.active = objactive.active;
            objects.entered_by = objactive.entered_by;
            array.Add(objects);
            var param = JsonConvert.SerializeObject(array);
            values.Add("@v_txt", param);
            var result = await Connection.ExecuteScalarAsync<int>($"demo.fn_item_master_active_update", values,
                 commandType: CommandType.StoredProcedure, transaction: Transaction);
            return result;
        }

    }
}
