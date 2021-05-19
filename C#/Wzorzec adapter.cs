//Mateusz Basiak Arkadiusz Pospieszny Jakub Kołodziejczyk
using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
namespace Adapter
{
    public interface IXmlFile
    {
        public Company[] sendData();

    }

    public interface ILibrary
    {
        public void getData(string s);
    }

    public class Company
    {
        public string Name { get; set; }
        public double Last { get; set; }
        public double Change { get; set; }
        public double ChangePerc { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
    }

    public class XmlFile : IXmlFile {
        private readonly Company[] companies;

        public XmlFile()
        {
            string[] Lines = File.ReadAllLines(@"C:\Users\Matbas\Desktop\studia\II rok\IO\Adapter\data.xml");
            List<Company> tmpCompanies = new List<Company>();
            for( int i = 1; i<Lines.Length-1; i++)
            {
                string[] tmp = Lines[i].Split('"');
                if (tmp[1] != "N/A" && tmp[3] != "N/A" && tmp[5] != "N/A" && tmp[7] != "N/A" && tmp[9] != "N/A" && tmp[11] != "N/A")
                {
                    Company newC = new Company();
                    newC.Name = tmp[1];
                    newC.Last = Convert.ToDouble(tmp[3], CultureInfo.InvariantCulture.NumberFormat);
                    newC.Change = Convert.ToDouble(tmp[5], CultureInfo.InvariantCulture.NumberFormat);
                    newC.ChangePerc = Convert.ToDouble(tmp[7], CultureInfo.InvariantCulture.NumberFormat);
                    newC.High = Convert.ToDouble(tmp[9], CultureInfo.InvariantCulture.NumberFormat);
                    newC.Low = Convert.ToDouble(tmp[11], CultureInfo.InvariantCulture.NumberFormat);
                    tmpCompanies.Add(newC);
                }
            }
            companies = tmpCompanies.ToArray();
        }

        public void AverageVals()
        {
            double sumChange = 0;
            double sumHigh = 0;
            double sumLow = 0;
            foreach(Company company in companies)
            {
                sumChange += company.ChangePerc;
                sumHigh += company.High;
                sumLow += company.Low;
            }

            Console.WriteLine("Srednia zmiana w procentach: " + (sumChange / companies.Length) * 100);
            Console.WriteLine("Srednia oferta High: " + sumHigh / companies.Length);
            Console.WriteLine("Srednia oferta Low: " + sumLow / companies.Length);
        }

        public Company[] sendData()
        {
            return companies;
        }
    }

    public class Adapter: IXmlFile
    {
        private readonly Company[] companies;
        private readonly ILibrary library;

        public Adapter(ILibrary lib, XmlFile file)
        {
            companies = file.sendData();
            library = lib;
        }

        public Company[] sendData()
        {
            library.getData(JsonSerializer.Serialize(companies));
            return companies;
        }
    }

    public class Library: ILibrary
    {
        private string jsonData;
        
        public void VariancePositive()
        {
            Company[] tmp = JsonSerializer.Deserialize<Company[]>(jsonData);
            
            double aveChange = tmp.Select(x => x.Change).Average();
            double aveChangePerc = tmp.Select(x => x.ChangePerc).Average();
            double sumChange = 0;
            double sumChangePerc = 0;
            
            foreach(Company company in tmp)
            {
                sumChange += Math.Pow((company.Change - aveChange), 2);
                sumChangePerc += Math.Pow((company.ChangePerc - aveChangePerc), 2);
            }

            Console.WriteLine("Wariancja dla change: " + sumChange / tmp.Length);
            Console.WriteLine("Wariancja dla change%: " + sumChangePerc / tmp.Length);
        }

        public void getData(string data)
        {
            Company[] tmp = JsonSerializer.Deserialize<Company[]>(data);
            List<Company> positive = new List<Company>();
            foreach(Company company in tmp)
            {
                if(company.Change >= 0 && company.ChangePerc >= 0)
                {
                    positive.Add(company);
                }
            }
            jsonData = JsonSerializer.Serialize(positive.ToArray());
            Console.WriteLine("Dane w bibliotece: " + jsonData);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var file = new XmlFile();
            file.AverageVals();

            var lib = new Library();

            var adapter = new Adapter(lib, file);
            adapter.sendData();
            lib.VariancePositive();

        }
    }
}
