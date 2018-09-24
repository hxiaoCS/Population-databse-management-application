namespace Population_Database
{
    public class CityInfo
    {
        public string City { get; set; }
        public double Population { get; set; }
        public int RowID { get; set; }
        public bool IsChanged { get; set; }
        public bool IsDeleted { get; set; }

        // make a copy of the object for edit or scroll back
        public CityInfo Copy()
        {
            CityInfo city = new CityInfo
            {
                City = this.City,
                Population = this.Population,
                RowID = this.RowID,
                IsChanged = true,
                IsDeleted = false
            };

            return city;
        }
    }
}