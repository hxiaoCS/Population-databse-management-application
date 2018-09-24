using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Population_Database
{
    public class VM : INotifyPropertyChanged
    {
        DB db = DB.GetInstance();// get a reference for repeated use

        const int INITIAL_NUMBER = 0;
        const int INDEX_FIRST = 1;
        const int INDEX_SECOND = 2;
        const string CMD_STRING_DEFAULT = "SELECT City, Population, rowID FROM City";
        const string CMD_STRING_POPULATION_DESC = "SELECT City, Population, rowID FROM City ORDER BY Population DESC";
        const string CMD_STRING_POPULATION_ASC = "SELECT City, Population, rowID FROM City ORDER BY Population ASC";

        #region properties
        // count rows of the list
        int cityCount;
        public int CityCount
        {
            get { return cityCount; }
            set { cityCount = value; propertyChange(); }
        }

        double totalPopulation;
        public double TotalPopulation
        {
            get { return totalPopulation; }
            set { totalPopulation = value; propertyChange(); }
        }

        double highestPopulation;
        public double HighestPopulation
        {
            get { return highestPopulation; }
            set { highestPopulation = value; propertyChange(); }
        }

        string highestCity;
        public string HighestCity
        {
            get { return highestCity; }
            set { highestCity = value; propertyChange(); }
        }

        // selected item in combobox
        public int SelectedSort { get; set; }

        // bindinglist to contain the data
        BindingList<CityInfo> cities;
        public BindingList<CityInfo> Cities
        {
            get { return cities; }
            set { cities = value; propertyChange(); }
        }

        // the selected item
        CityInfo selectedCity;
        public CityInfo SelectedCity
        {
            get { return selectedCity; }
            set { selectedCity = value; propertyChange(); }
        }

        // the copy of selected item or a new item
        CityInfo editCity;
        public CityInfo EditCity
        {
            get { return editCity; }
            set { editCity = value; propertyChange(); }
        }
        #endregion

        #region methods
        public VM()
        {
            Load();
            CalculateTotal();
            CalculateHighest();
        }

        // create cmdString based on sort selection and get data from database
        public void Load()
        {
            string cmdString;
            switch (SelectedSort)
            {
                case INITIAL_NUMBER:
                    cmdString = CMD_STRING_DEFAULT;
                    break;
                case INDEX_FIRST:
                    cmdString = CMD_STRING_POPULATION_DESC;
                    break;
                case INDEX_SECOND:
                    cmdString = CMD_STRING_POPULATION_ASC;
                    break;
                default:
                    cmdString = CMD_STRING_DEFAULT;
                    break;
            }
            // pass the cmdString to Get function in DB class
            Cities = new BindingList<CityInfo>(db.Get(cmdString));
        }

        public void Save()
        {
            if (editCity.RowID != INITIAL_NUMBER)
            {
                // find the place of the selected item in the list
                int index = Cities.IndexOf(selectedCity);
                // remove the selected item from list
                Cities.Remove(selectedCity);
                // add the edited one to list in the same place
                Cities.Insert(index, editCity);
            }
            else Cities.Add(editCity);

            // save the changes to database
            db.Save(Cities.ToList());
            // load data again and do calculations
            Load();
            CalculateTotal();
            CalculateHighest();
        }

        public void Delete()
        {
            if (selectedCity.IsDeleted)
            {
                // delete the selected item from database
                db.Delete(selectedCity);
                // delete the selected item from the list
                cities.Remove(selectedCity);
            }

            // do calculations again
            CalculateTotal();
            CalculateHighest();
        }

        // calculate the total population of all cities
        public void CalculateTotal()
        {
            TotalPopulation = INITIAL_NUMBER;
            foreach (CityInfo city in cities)
                TotalPopulation += city.Population;
            CityCount = cities.Count();
        }

        // find the city with the highest population
        public void CalculateHighest()
        {
            HighestPopulation = INITIAL_NUMBER;
            foreach (CityInfo city in cities)
            {
                if (HighestPopulation < city.Population)
                {
                    HighestPopulation = city.Population;
                    HighestCity = city.City;
                }
            }
        }
        #endregion

        #region propertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        private void propertyChange([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}