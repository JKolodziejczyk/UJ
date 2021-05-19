using System;
using System.Numerics;
using System.Text.Json;
namespace Fabryka
{
    public interface Furniture
    {
        public Vector3 getDim();  //szerokosc dlugosc wysokosc
        public string getName();
    }

    public static class Serializer
    {
        class Info
        {
            public string Name { get; set; }
            public float Width { get; set; }
            public float Length { get; set; }
            public float Height { get; set; }

            public Info(string n, Vector3 dim)
            {
                Name = n;
                Width = dim.X;
                Length = dim.Y;
                Height = dim.Z;
            }
        }
        public static string Serialize(Furniture f)
        {
            return JsonSerializer.Serialize(new Info(f.getName(), f.getDim()));
        }
    }

    public interface Chair
    {
        public void sitOn();
        public bool hasLegs();
    }

    class VictorianChair : Chair, Furniture
    {
        public void sitOn()
        {
            Console.WriteLine("Somebody sat down on Victorian Chair!");
        }

        public bool hasLegs()
        {
            return true;
        }

        public Vector3 getDim()
        {
            return new Vector3(6, 4, 5);
        }

        public string getName()
        {
            return $"Victorian Chair";
        }
    }

    class ModernChair : Chair, Furniture
    {
        public void sitOn()
        {
            Console.WriteLine("Somebody sat down on Modern Chair!");
        }

        public bool hasLegs()
        {
            return false;
        }

        public Vector3 getDim()
        {
            return new Vector3(4, 5, 5);
        }

        public string getName()
        {
            return $"Modern Chair";
        }
    }

    class ArtDecoChair : Chair, Furniture
    {
        public void sitOn()
        {
            Console.WriteLine("Somebody sat down on Art Deco Chair!");
        }

        public bool hasLegs()
        {
            return true;
        }

        public Vector3 getDim()
        {
            return new Vector3(7, 4, 4);
        }

        public string getName()
        {
            return $"Art Deco Chair";
        }
    }

    public interface Sofa
    {
        public void sitOn();
        public bool hasLegs();
    }

    class VictorianSofa : Sofa, Furniture
    {
        public void sitOn()
        {
            Console.WriteLine("Somebody sat down on Victorian Sofa!");
        }

        public bool hasLegs()
        {
            return true;
        }

        public Vector3 getDim()
        {
            return new Vector3(16, 4, 5);
        }

        public string getName()
        {
            return $"Victorian Sofa";
        }
    }

    class ModernSofa : Sofa, Furniture
    {
        public void sitOn()
        {
            Console.WriteLine("Somebody sat down on Modern Sofa!");
        }

        public bool hasLegs()
        {
            return false;
        }

        public Vector3 getDim()
        {
            return new Vector3(14, 5, 5);
        }

        public string getName()
        {
            return $"Modern Sofa";
        }
    }

    class ArtDecoSofa : Sofa, Furniture
    {
        public void sitOn()
        {
            Console.WriteLine("Somebody sat down on Art Deco Sofa!");
        }

        public bool hasLegs()
        {
            return true;
        }

        public Vector3 getDim()
        {
            return new Vector3(17, 4, 4);
        }

        public string getName()
        {
            return $"Art Deco Sofa";
        }
    }

    public interface CoffeeTable
    {
        public void putThing();
        public bool hasLegs();
    }

    class VictorianCoffeTable : CoffeeTable, Furniture
    {
        public void putThing()
        {
            Console.WriteLine("Somebody put something on Victorian Coffee Table!");
        }

        public bool hasLegs()
        {
            return true;
        }

        public Vector3 getDim()
        {
            return new Vector3(10, 4, 3);
        }

        public string getName()
        {
            return $"Victorian Coffee Table";
        }
    }
    class ModernCoffeeTable : CoffeeTable, Furniture
    {
        public void putThing()
        {
            Console.WriteLine("Somebody put something on Modern Coffee Table!");
        }

        public bool hasLegs()
        {
            return true;
        }
        public Vector3 getDim()
        {
            return new Vector3(6, 3, 3);
        }

        public string getName()
        {
            return $"Modern Coffee Table";
        }
    }
    class ArtDecoCoffeeTable : CoffeeTable, Furniture
    {
        public void putThing()
        {
            Console.WriteLine("Somebody put something on Art Deco Coffee Table!");
        }

        public bool hasLegs()
        {
            return false;
        }
        public Vector3 getDim()
        {
            return new Vector3(7, 3, 5);
        }

        public string getName()
        {
            return $"Art Deco Coffee Table";
        }
    }

    interface FurnitureFactory
    {
        public Chair createChair();
        public Sofa createSofa();
        public CoffeeTable createCoffeeTable();
    }

    public class VictorianFurnitureFactory : FurnitureFactory
    {
        public Chair createChair()
        {
            return new VictorianChair();
        }

        public Sofa createSofa()
        {
            return new VictorianSofa();
        }

        public CoffeeTable createCoffeeTable()
        {
            return new VictorianCoffeTable();
        }
    }

    public class ModernFurnitureFactory : FurnitureFactory
    {
        public Chair createChair()
        {
            return new ModernChair();
        }

        public Sofa createSofa()
        {
            return new ModernSofa();
        }

        public CoffeeTable createCoffeeTable()
        {
            return new ModernCoffeeTable();
        }

    }
    public class ArtDecoFurnitureFactory : FurnitureFactory
    {
        public Chair createChair()
        {
            return new ArtDecoChair();
        }

        public Sofa createSofa()
        {
            return new ArtDecoSofa();
        }

        public CoffeeTable createCoffeeTable()
        {
            return new ArtDecoCoffeeTable();
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var ArtDecoFactory = new ArtDecoFurnitureFactory();
            var VictorianFactory = new VictorianFurnitureFactory();
            var ModernFactory = new ModernFurnitureFactory();

            var chair1 = ArtDecoFactory.createChair();
            var sofa1 = ArtDecoFactory.createSofa();
            var coffee1 = ArtDecoFactory.createCoffeeTable();

            Console.WriteLine(Serializer.Serialize((Furniture)chair1) + "\n" + Serializer.Serialize((Furniture)sofa1) + "\n" + Serializer.Serialize((Furniture)coffee1)+"\n");

            var chair2 = VictorianFactory.createChair();
            var sofa2 = VictorianFactory.createSofa();
            var coffee2 = VictorianFactory.createCoffeeTable();

            Console.WriteLine(Serializer.Serialize((Furniture)chair2) + "\n" + Serializer.Serialize((Furniture)sofa2) + "\n" + Serializer.Serialize((Furniture)coffee2) + "\n");

            var chair3 = ModernFactory.createChair();
            var sofa3 = ModernFactory.createSofa();
            var coffee3 = ModernFactory.createCoffeeTable();

            Console.WriteLine(Serializer.Serialize((Furniture)chair3) + "\n" + Serializer.Serialize((Furniture)sofa3) + "\n" + Serializer.Serialize((Furniture)coffee3) + "\n");
        }
    }
}
