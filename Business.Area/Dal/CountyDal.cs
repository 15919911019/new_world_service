using Business.AreaModels;
using Public.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AreaService.Dal
{
    public class CountyDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public CountyDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        public bool Create(string name, string cityRecID)
        {
            CountyModel model = new CountyModel() { Name = name, Parent = cityRecID };
            var count = _database.Insert(model, _tabName);
            return count == 1;
        }

        public bool Update(string name, string cityRecID, string recordID)
        {
            var sql = $"update {_tabName} set " +
                $"Name = '{name}', Parent = '{cityRecID}' " +
                $"where " +
                $"RecordID = '{recordID}'";

            var count = _database.ExecuteSql(sql);
            return count == 1;
        }

        public List<CountyMapModel> Search(string cityRecID = null)
        {
            var sql = $"select county.*, city.name as city, " +
                $"pro.name as province, pro.recordID as provinceID " +
                $"from {_tabName} county " +
                $"left join area_city city on county.parent = city.recordID " +
                $"left join area_province pro on city.parent = pro.recordID " +
                $"where " +
                $"{(string.IsNullOrWhiteSpace(cityRecID) ? "1=1" : $"city.RecordID = '{cityRecID}'")}";
            var data = _database.Query<CountyMapModel>(sql);
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
