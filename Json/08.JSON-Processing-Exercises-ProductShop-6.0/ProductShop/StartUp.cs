using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Data;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new();
            //1.Import users
            //string usersJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usersJson));

            //2.Import products
            //string productsJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productsJson));

            //3.Import categories
            //string categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesJson));

            //4. Import categoryProducts 
            //string categoryProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductsJson));

            //5. Export products
            //Console.WriteLine(GetProductsInRange(context));

            //6.Export sold products 
            //Console.WriteLine(GetSoldProducts(context));

            //7.Export categories by products count
            // Console.WriteLine(GetCategoriesByProductsCount(context));

            //8.Get users with products 
            Console.WriteLine(GetUsersWithProducts(context));
        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<User[]>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges(); 
            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            if (products != null)
            {
                context.Products.AddRange(products);
                context.SaveChanges();           
            }
           
            return $"Successfully imported {products?.Length}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson);
            var validCategories = categories?.Where(c => c.Name is not null).ToArray();
            
            
                      
            if(validCategories != null)
            {
                context.Categories.AddRange(validCategories);
                context.SaveChanges();
                return $"Successfully imported {validCategories.Length}";
            }
            return $"Successfully imported 0";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryproducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);
            context.CategoriesProducts.AddRange(categoryproducts);
            context.SaveChanges();
            
            return $"Successfully imported {categoryproducts.Length}";

        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .Where(p => p.price >= 500 && p.price <= 1000)
                .OrderBy(p => p.price).ToArray();

            string productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
            return productsJson;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersWithSoldProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold.Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    }).ToArray()

                }).ToArray();

            string jsonOutput = JsonConvert.SerializeObject(usersWithSoldProducts, Formatting.Indented);
            return jsonOutput;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories                
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = c.CategoriesProducts.Average(cp => cp.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoriesProducts.Sum(cp => cp.Product.Price).ToString("F2"),
                })
                .OrderByDescending(c => c.productsCount)
                .ToArray();

            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return json;
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.BuyerId != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,  
                    age = u.Age,
                    soldProducts = u.ProductsSold
                    .Where(sp => sp.BuyerId != null)
                    .Select(ps => new 
                    {
                       name = ps.Name,
                       price = ps.Price
                    })
                    .ToArray()
                })
                .OrderByDescending(u => u.soldProducts.Count())
                .ToArray();

            var output = new
            {
                usersCount = users.Count(),
                users = users.Select(u => new
                {
                    u.firstName,
                    u.lastName,
                    u.age,
                    soldProducts = new
                    {
                        count = u.soldProducts.Count(),
                        products = u.soldProducts
                    }
                })

            };
            string usersJson = JsonConvert.SerializeObject(output, new JsonSerializerSettings
            { 
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return usersJson;   
        }
    }
}