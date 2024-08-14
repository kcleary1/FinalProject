using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MySql.Data.MySqlClient;
using System.Windows;

namespace AssetClass2
{
    public class AssetClassRepository
    {
        private string _connectionString;
        public AssetClassRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(_connectionString);
            }
        }
        public IEnumerable<AssetClass> GetLargeGrowthInfo()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<AssetClass>("SELECT * FROM uslargegrowth");
            }
        }

        public IEnumerable<AssetClass> GetSmallValueInfo()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<AssetClass>("SELECT * FROM ussmallvalue");
            }
        }
        public IEnumerable<AssetClass> GetTotalBondInfo()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<AssetClass>("SELECT * FROM ustotalbond");
            }
        }

       

    }
}
