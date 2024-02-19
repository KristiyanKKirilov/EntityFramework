using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new();
            //9.Import suppliers
            //string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, suppliersJson));

            //10.Import parts
            //string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, partsJson));

            //11.Import cars
           // string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, carsJson));

            //12.
            //string customersJson = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, customersJson));

            //13.
            //string salesJson = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context, salesJson));

            //14.Export Ordered Customers
            //Console.WriteLine(GetOrderedCustomers(context));

            //15.Export Cars from Make Toyota
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            //16.Export Local Suppliers
            //Console.WriteLine(GetLocalSuppliers(context));

            //17.Export Cars with Their List of Parts
            Console.WriteLine(GetCarsWithTheirListOfParts(context));
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            //var suppliers = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            //context.AddRange(suppliers);
            //context.SaveChanges();
            //return $"Successfully imported {suppliers.Length}.";
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);
            SupplierDto[] supplierDto = JsonConvert.DeserializeObject<SupplierDto[]>(inputJson);
            Supplier[] suppliers = mapper.Map<Supplier[]>(supplierDto);
           context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Length}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);

            PartDto[] partsDto = JsonConvert.DeserializeObject<PartDto[]>(inputJson);
            Part[] parts = mapper.Map<Part[]>(partsDto);

            int[] suppliersIds = context.Suppliers
                .Select(x => x.Id)
                .ToArray();

            Part[] validParts = parts.Where(p => suppliersIds.Contains(p.SupplierId)).ToArray();
            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Length}.";
        }
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
           var cars = JsonConvert.DeserializeObject<List<CarDto>>(inputJson);

            foreach (var car in cars)
            {
                Car currentCar = new Car()
                {
                    Make = car.Make,
                    Model = car.Model,
                    TraveledDistance = car.TraveledDistance
                };

                foreach (var part in car.PartsId)
                {
                    bool isValid = currentCar.PartsCars.FirstOrDefault(x => x.PartId == part) == null;
                    bool isPartValid = context.Parts.FirstOrDefault(p => p.Id == part) != null;

                    if (isValid && isPartValid)
                    {
                        currentCar.PartsCars.Add(new PartCar()
                        {
                            PartId = part
                        });
                    }
                }
                context.Cars.Add(currentCar);
            }
                //Car[] cars = mapper.Map<Car[]>(carsDto);
                //context.Cars.AddRange(cars);
                context.SaveChanges();

                return $"Successfully imported {context.Cars.Count()}.";
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);

            CustomerDto[] customersDto = JsonConvert.DeserializeObject<CustomerDto[]>(inputJson);
            Customer[] customers = mapper.Map<Customer[]>(customersDto);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Length}.";
        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);

            SaleDto[] saleDto = JsonConvert.DeserializeObject<SaleDto[]>(inputJson);
            Sale[] sales = mapper.Map<Sale[]>(saleDto);

            context.Sales.AddRange(sales);

            context.SaveChanges();
            return $"Successfully imported {sales.Length}.";
        }
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);


            var customersDto = context.Customers
                .ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ToArray();
            var converter = new IsoDateTimeConverter()
            {
                DateTimeFormat = "dd/MM/yyyy"
            };
            string customersJson = JsonConvert.SerializeObject(customersDto, Formatting.Indented, converter);

            return customersJson;

        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);

            CarDto[] cars = context.Cars
                .ProjectTo<CarDto>(mapper.ConfigurationProvider)
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                 .ThenByDescending(c => c.TraveledDistance) 
                 .ToArray();

            var json = JsonConvert.SerializeObject(cars, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            });
            return json;
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers= context.Suppliers                
                .Where(s => s.IsImporter == false)
                .Select(s => new 
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return json;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(config);
            var carsAndParts = context.Cars
                .ProjectTo<CarPartsDto>(mapper.ConfigurationProvider)
                .ToList();
                
            string json = JsonConvert.SerializeObject(carsAndParts, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            });
            return json;


        }
    }
}