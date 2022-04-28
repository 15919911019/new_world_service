using Business.AreaModels;
using Public.DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AreaService.Dal
{
    public class CityDal
    {
        private AbsDatabaseClient _database;

        private string _tabName;

        public CityDal(AbsDatabaseClient database, string tabName)
        {
            _database = database;
            _tabName = tabName;
        }

        public bool Create(string name, string proRecID)
        {
            CityModel model = new CityModel() { Name = name, Parent = proRecID };
            var count = _database.Insert(model, _tabName);
            return count == 1;
        }

        public bool Update(string name, string proRecID, string recordID)
        {
            var sql = $"update {_tabName} set " +
                $"Name = '{name}', Parent = '{proRecID}' " +
                $"where " +
                $"RecordID = '{recordID}'";

            var count = _database.ExecuteSql(sql);
            return count == 1;
        }

        public List<CityMapModel> Search(string proRecID = null)
        {
            var sql = $"select city.*, pro.name as Province from {_tabName} city " +
                $"left join area_province pro on city.parent = pro.recordID " +
                $"where " +
                $"{(string.IsNullOrWhiteSpace(proRecID) ? "1=1" : $"pro.recordID = '{proRecID}'")}";
            var data = _database.Query<CityMapModel>(sql);
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
