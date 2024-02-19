using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new();

            //9.Import Suppliers
            //string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, inputXml));

            //10.Import Parts
            //string inputPartsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, inputPartsXml));

            //11.Import Cars
            //string importCarsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, importCarsXml));

            //12.Import Customers
            // string importCustomersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, importCustomersXml));

            //13.Import Sales
            //string importSales = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, importSales));

            //14.Get Cars With Distance
            //Console.WriteLine(GetCarsWithDistance(context));

            //18.Get Total Saves By Customer
            Console.WriteLine(GetTotalSalesByCustomer(context));
        }

        private static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            Mapper mapper = new Mapper(config);
            return mapper;
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));
            using var reader = new StringReader(inputXml);
            ImportSupplierDto[] importSupplierDtos = (ImportSupplierDto[])serializer.Deserialize(reader);

            var mapper = GetMapper();
            Supplier[] suppliers = mapper.Map<Supplier[]>(importSupplierDtos);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Length}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));
            using var reader = new StringReader(inputXml);
            ImportPartDto[] importPartDtos = (ImportPartDto[])serializer.Deserialize(reader);

            var mapper = GetMapper();
            var supplierIds = context.Suppliers.Select(s => s.Id).ToList();
            Part[] parts = mapper.Map<Part[]>(importPartDtos.Where(p => supplierIds.Contains(p.SupplierId)));

            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Length}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));
            using var reader = new StringReader(inputXml);
            var mapper = GetMapper();
            ImportCarDto[] importCarDtos = (ImportCarDto[])serializer.Deserialize(reader);


            List<Car> cars = new();

            foreach (var carDto in importCarDtos)
            {
                Car car = mapper.Map<Car>(carDto);

                int[] carPartIds = carDto.PartsIds
                    .Select(p => p.Id)
                    .Distinct()
                    .ToArray();

                List<PartCar> carParts = new();
                foreach (var id in carPartIds)
                {
                    carParts.Add(new PartCar
                    {
                        Car = car,
                        PartId = id
                    });
                }
                car.PartsCars = carParts;
                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count()}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            using StringReader reader = new(inputXml);
            Mapper mapper = GetMapper();
            ImportCustomerDto[] importCustomerDtos = (ImportCustomerDto[])xmlSerializer.Deserialize(reader);
            Customer[] customers = mapper.Map<Customer[]>(importCustomerDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Length}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));
            using StringReader reader = new(inputXml);
            Mapper mapper = GetMapper();
            ImportSaleDto[] importSaleDtos = (ImportSaleDto[])serializer.Deserialize(reader);
            int[] carIds = context.Cars.Select(c => c.Id).ToArray();
            Sale[] sales = mapper.Map<Sale[]>(importSaleDtos.Where(s => carIds.Contains(s.CarId)));
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Length}";
        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            Mapper mapper = GetMapper();
            ExportCarsWithDistanceDto[] carsToExport = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                    .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarsWithDistanceDto>(mapper.ConfigurationProvider)
                .ToArray();

            //string result = SerializeObject(carsToExport, "cars");

            var serializer = new XmlSerializer(typeof(ExportCarsWithDistanceDto[]), new XmlRootAttribute("cars"));
            var xmlNs = new XmlSerializerNamespaces();
            xmlNs.Add(string.Empty, string.Empty);
            StringBuilder sb = new();
            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, carsToExport, xmlNs);
            }
            //return result;
            return sb.ToString().TrimEnd();
        }
        private static string SerializeObject<T>(T data, string rootElement) where T : class        {
            
            XmlSerializer serializer = new(typeof(T), new XmlRootAttribute(rootElement));
            StringBuilder sb = new();
            using (StringWriter sw = new (sb, CultureInfo.InvariantCulture))
            {
                XmlSerializerNamespaces xmlNamespaces = new();
                xmlNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    serializer.Serialize(sw, data, xmlNamespaces);
                }
                catch (Exception)
                {

                    throw;
                }                
                
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var temp = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SalesInfo = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver
                                ? s.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price * 0.95, 2))
                                : s.Car.PartsCars.Sum(pc => (double)pc.Part.Price)
                    })
                        .ToArray(),
                })
                .ToArray();


            ExportSalesByCustomerDto[] totalSales = temp.OrderByDescending(x => x.SalesInfo.Sum(y => y.Prices))
                .Select(x => new ExportSalesByCustomerDto()
                {
                    FullName = x.FullName,
                    BoughtCars = x.BoughtCars,
                    SpentMoney = x.SalesInfo.Sum(y => (decimal)y.Prices)
                })
                .ToArray();

            return SerializeObject<ExportSalesByCustomerDto[]>(totalSales, "customers");
        }
    }
}