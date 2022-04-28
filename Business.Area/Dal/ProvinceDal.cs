using Business.AreaModels;
using Public.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AreaService.Dal
{
    public class ProvinceDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public ProvinceDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        public bool Create(string name)
        {
            ProvinceModel model = new ProvinceModel() { Name = name };
            var count = _database.Insert(model, _tabName);
            return count == 1;
        }

        public bool Update(string name, string recordID)
        {
            var sql = $"update {_tabName} set " +
                $"Name = '{name}' where " +
                $"RecordID = '{recordID}'";

            var count = _database.ExecuteSql(sql);
            return count == 1;
        }

        public List<ProvinceModel> Search()
        {
            var sql = $"select * from {_tabName}";
            var data = _database.Query<ProvinceModel>(sql);
            return data;
        }

        public bool Delete(string reocrdID)
        {
            var sql = $"delete from {_tabName} where RecordID = '{reocrdID}'";
            var count = _database.ExecuteSql(sql);
            return count == 1;
        }
    }
}
