using System;
using System.Data;
using DVLDDataAccess;

namespace DVLDBusinessLayer
{
    public class clsCountry
    {
        public int ID { get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            this.CountryName = "";
        }

        private clsCountry(int ID, string CountryName)
        {
            this.ID = ID;
            this.CountryName = CountryName;
        }

        public static DataTable _GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries();
        }

        public static clsCountry Find(int ID)
        {
            string CountryName = "";

            if (clsCountryDataAccess.GetCountryInfoByID(ID, ref CountryName))
            {
                return new clsCountry(ID, CountryName);
            }
            else
                return null;
        }

        public static clsCountry Find(string CountryName)
        {
            int ID = -1;

            if (clsCountryDataAccess.GetCountryInfoByName(ref ID, CountryName))
            {
                return new clsCountry(ID, CountryName);
            }
            else
                return null;
        }
    }
}