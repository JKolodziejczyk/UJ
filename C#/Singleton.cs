//Arkadiusz Pospieszny Jakub Kołodziejczyk Mateusz Basiak
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;

namespace Singleton
{
	public class City
	{
		public string Name { get; set; }
		public string Ascii { get; set; }
		public double Lat { get; set; }
		public double Lng { get; set; }
		public string Country { get; set; }
		public string Iso2 { get; set; }
		public string Iso3 { get; set; }
		public string AdminName { get; set; }
		public string Capital { get; set; }
		public int Population { get; set; }
		public long Id { get; set; }

		public override string ToString()
		{
			return $"Id: {Id} Nazwa: {Name} Nazwa Asci: {Ascii} Lat: {Lat} Lng: {Lng} Kraj: {Country} Iso2: {Iso2} Iso3: {Iso3} Nazwa administracyjna: {AdminName} Stolica: {Capital} Populacja: {Population}";
		}
	}

	public class Country
	{
		public string Continent { get; set; }
		public string Name { get; set; }
		public string Iso2 { get; set; }
		public string Iso3 { get; set; }
		public int Id { get; set; }

		public override string ToString()
		{
			return $"Id: {Id} Nazwa: {Name} Kontynent: {Continent} Iso2: {Iso2} Iso3: {Iso3}";
		}
	}

	public class Continent
	{
		public string Name { get; set; }
		public string Code { get; set; }

		public override string ToString()
		{
			return $"Nazwa: {Name} Kod: {Code}";
		}
	}

	public class Dao
    {
		private static Dao _instance;
		private static readonly List<City> Cities = new List<City>();
		private static readonly List<Country> Countries = new List<Country>();
		private static readonly List<Continent> Continents = new List<Continent>();

		private Dao() { }

		public void ReadCities()
        {
			string[] lines = File.ReadAllLines(@"C:\Users\Matbas\Desktop\studia\II rok\IO\Singleton\Singleton\Singleton\Singleton\city.csv");
			for(int i=1; i<lines.Length; i++)
            {
				City temp = new City();
				string[] temp2 = lines[i].Split('\"');
				temp.Name = temp2[1];
				temp.Ascii = temp2[3];
				temp.Lat = Convert.ToDouble(temp2[5], CultureInfo.InvariantCulture.NumberFormat);
				temp.Lng = Convert.ToDouble(temp2[7], CultureInfo.InvariantCulture.NumberFormat);
				temp.Country = temp2[9];
				temp.Iso2 = temp2[11];
				temp.Iso3 = temp2[13];
				temp.Capital = temp2[15];
				temp.AdminName = temp2[17];
				if(temp2[19].Length != 0)
                {
					int idx = temp2[19].IndexOf('.');
					if (idx != -1) temp2[19] = temp2[19].Substring(0, idx);
					temp.Population = Convert.ToInt32(temp2[19]);
				}
				temp.Id = Convert.ToInt32(temp2[21]);
				Cities.Add(temp);
            }
        }

		public void ReadCountriesAndContinents()
		{
			string[] lines = File.ReadAllLines(@"C:\Users\Matbas\Desktop\studia\II rok\IO\Singleton\Singleton\Singleton\Singleton\country.csv");
			for (int i = 1; i < lines.Length; i++)
			{
				Country country = new Country();
				Continent continent = new Continent();
				string[] temp2 = lines[i].Split('\"');
				if(temp2.Length == 1)
                {
					string[] temp3 = temp2[0].Split(',');
					continent.Name = temp3[0];
					continent.Code = temp3[1];
					country.Continent = temp3[0];
					country.Name = temp3[2];
					country.Iso2 = temp3[3];
					country.Iso3 = temp3[4];
					if(temp3[5].Length != 0)
                    {
						country.Id = Convert.ToInt32(temp3[5]);
					}
                }
                else
                {
					string[] temp3 = temp2[0].Split(',');
					string[] temp4 = temp2[2].Split(',');
					continent.Name = temp3[0];
					continent.Code = temp3[1];
					country.Continent = temp3[0];
					country.Name = temp2[1];
					country.Iso2 = temp4[1];
					country.Iso3 = temp4[2];
					if(temp4[3].Length != 0)
                    {
						country.Id = Convert.ToInt32(temp4[3]);
                    }
                }
				bool found = false;
				foreach(Continent temp in Continents)
                {
                    if (temp.Name == continent.Name)
                    {
						found = true;
						break;
                    }
                }
				if (!found) Continents.Add(continent);
				Countries.Add(country);
			}
		}

		public static List<City> Search(string Substring)
		{
			List<City> CitiesContaining = new List<City>();
			foreach (City x in Cities)
			{
				if (x.Ascii.ToUpper().Contains(Substring.ToUpper()))
					CitiesContaining.Add(x);
			}
			return CitiesContaining;
		}

		public static List<City> Max10()
		{
			return Cities.OrderByDescending(temp => temp.Population).Take(10).ToList();
		}

		public static double AvgPopulation()
        {
			return Cities.Select(city => city.Population).Average();
        }

		public static List<string> AvgPopulationContinent()
        {
			List<string> res = new List<string>();
			foreach(Continent continent in Continents)
            {
				List<string> temp = Countries.Where(country => country.Continent == continent.Name).Select(country => country.Iso2).ToList();
				int count = 0;
				double sum = 0;
				foreach(City city in Cities)
                {
					foreach(string country in temp)
                    {
                        if (country == city.Iso2)
                        {
							sum += city.Population;
							count++;
							break;
                        }
                    }
                }
				if (count != 0)
					res.Add(continent.Name + ": " + sum / count);
				else
					res.Add(continent.Name + ": 0");
            }
			return res.OrderBy(x => x).ToList();
        }

		public static List<City> MaxCityByCountry()
        {
			List<City> res = new List<City>();
			foreach (Country country in Countries)
            {
				List<City> temp = Cities.Where(x => country.Iso2 == x.Iso2).OrderByDescending(x => x.Population).ToList();
				if(temp.Count > 0)
                {
					bool found = false;
					for(int i=0; i<res.Count; i++)
                    {
						if(res[i].Name == temp[0].Name)
                        {
							found = true;
							break;
                        }
                    }
					if(!found)
						res.Add(temp[0]);
				}
					
            }
			return res.OrderByDescending(x => x.Population).ToList();
        }

		public static List<string> MaxPopulationContinent()
		{
			List<string> res = new List<string>();
			foreach (Continent continent in Continents)
			{
				List<string> temp = Countries.Where(country => country.Continent == continent.Name).Select(country => country.Iso2).ToList();
				var cities = Cities.Where(x => temp.Contains(x.Iso2)).OrderByDescending(x => x.Population).ToList();
				if(cities.Count > 0)
					res.Add(continent.Name+": "+cities[0].Name+" Populacja: "+cities[0].Population);
			}
			return res.OrderBy(x=>x).ToList();
		}

		public static Dao Instance
		 {
			    get
			    {
					if(_instance == null)
			        {
						_instance = new Dao();
			        }
					return _instance;
		        }
        }
    }

	public class CityPresenter
	{
		public static void GetData(string Substring)
		{
			var Cities = Dao.Search(Substring);
			if(Cities.Count > 0)
            {
				Console.WriteLine($"Znaleziono następujące miasta:");
				foreach (City city in Cities)
				{
					Console.WriteLine(city);
				}
			}
            else
            {
				Console.WriteLine($"Nie znaleziono żadnych miast");
            }
		}
	}

	public class CityStatistic
	{
		public static void Top10()
		{
			Console.WriteLine("Top 10 miast na świecie:");
			List<City> Max10 = new List<City>();
			Max10 = Dao.Max10();
			for(int i=0; i<10; i++)
			{
				Console.WriteLine("Nr "+(i+1)+". "+Max10[i].Name+" Populacja: "+Max10[i].Population);
			}
		}

		public static void AvgPopulation()
        {
			double res = Dao.AvgPopulation();
			Console.WriteLine("Średnia populacja na świecie: " + res);
        }

		public static void AvgPopulationContinents()
        {
			Console.WriteLine("Średnia populacja na danym kontynencie:");
			List<string> res = Dao.AvgPopulationContinent();
			foreach(string line in res)
            {
				Console.WriteLine(line);
            }
        }

		public static void MaxCityByCountry()
        {
			Console.WriteLine("Najwieksze miasto dla danego kraju:");
			List<City> res = Dao.MaxCityByCountry();
			foreach(City city in res)
            {
				Console.WriteLine(city.Country + ": " + city.Name + " " + city.Population);
            }
        }

		public static void TopPopulationContinents()
		{
			Console.WriteLine("Największe miasta na kontynentach:");
			List<string> res = Dao.MaxPopulationContinent();
			foreach (string line in res)
			{
				Console.WriteLine(line);
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			Dao dao = Dao.Instance;
			dao.ReadCities();
			dao.ReadCountriesAndContinents();
			CityPresenter.GetData($"Mnich");
			Console.WriteLine();
			CityStatistic.Top10();
			Console.WriteLine();
			CityStatistic.AvgPopulation();
			Console.WriteLine();
			CityStatistic.AvgPopulationContinents();
			Console.WriteLine();
			CityStatistic.MaxCityByCountry();
			Console.WriteLine();
			CityStatistic.TopPopulationContinents();
			Console.ReadKey();
		}
	}
}
