using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;

namespace Population_Database
{
    public class DB
    {
        //connect to database
        const string CONNECT_STRING = @"Server=.\SQLEXPRESS;Database=Population;Trusted_Connection=True;";
        SqlConnection conn;

        static DB _db;

        private DB()
        {
            conn = new SqlConnection(CONNECT_STRING);
            conn.Open();
        }

        public static DB GetInstance()
        {
            if (_db == null)
                _db = new DB();
            return _db;
        }

        // generate the rowID for new row
        public int GetNextID()
        {
            int id = 0;

            string cmdString = "SELECT MAX(rowID) FROM City";
            SqlCommand cmd = new SqlCommand(cmdString, conn);
            object obj = cmd.ExecuteScalar();
            id = (int)obj;

            return id + 1;
        }

        //  check the changes and update to database
        public void Save(List<CityInfo> cities)
        {
            List<CityInfo> deletedCity = new List<CityInfo>();

            foreach (CityInfo city in cities)
            {
                if (city.IsDeleted)
                {
                    Delete(city);
                    deletedCity.Add(city);
                }
                else if (city.RowID == 0)
                    Add(city);
                else Update(city);
            }

            foreach (CityInfo city in deletedCity)
                cities.Remove(city);
        }

        //function to insert new row into database
        public void Add(CityInfo city)
        {
            if (city.RowID == 0)
                city.RowID = GetNextID();

            string cmdString = "INSERT INTO City(City, Population, rowID) " +
                               "VALUES(@CITY, @POPULATION, @ROWID)";

            SqlCommand cmd = new SqlCommand(cmdString, conn);
            cmd.Parameters.AddWithValue("@CITY", city.City);
            cmd.Parameters.AddWithValue("@POPULATION", city.Population);
            cmd.Parameters.AddWithValue("@ROWID", city.RowID);

            cmd.ExecuteNonQuery();
        }

        //function to update new data to database
        public void Update(CityInfo city)
        {
            if (city.IsChanged)
            {
                string cmdString = "UPDATE City SET " +
                                                "City = @CITY, " +
                                                "Population = @POPULATION " +
                                   "WHERE rowID = @ROWID";

                SqlCommand cmd = new SqlCommand(cmdString, conn);
                cmd.Parameters.AddWithValue("@CITY", city.City);
                cmd.Parameters.AddWithValue("@POPULATION", city.Population);
                cmd.Parameters.AddWithValue("@ROWID", city.RowID);

                cmd.ExecuteNonQuery();

                city.IsChanged = false;
            }
        }

        //function to delete data from database
        public void Delete(CityInfo city)
        {
            string cmdString = "DELETE FROM City WHERE rowID = @ROWID";

            SqlCommand cmd = new SqlCommand(cmdString, conn);
            cmd.Parameters.AddWithValue("@ROWID", city.RowID);

            cmd.ExecuteNonQuery();
        }

        //read data from database and send to CityInfo class
        public BindingList<CityInfo> Get(string cmdString)
        {
            BindingList<CityInfo> cities = new BindingList<CityInfo>();
            SqlCommand cmd = new SqlCommand(cmdString, conn);
            SqlDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
                cities.Add(new CityInfo()
                {
                    City = rd.GetString(rd.GetOrdinal("City")),
                    Population = rd.GetDouble(rd.GetOrdinal("Population")),
                    RowID = rd.GetInt32(rd.GetOrdinal("rowID"))
                });
            rd.Close();

            return cities;
        }
    }
}