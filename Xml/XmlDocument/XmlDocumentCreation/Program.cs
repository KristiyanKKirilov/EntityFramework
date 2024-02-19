using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using XmlDocumentCreation;

List<Book> books = new List<Book>() 
{
    new Book()
    {
        Title = "I, Robot",
        Author = new Author() {Name = "Isaac Asimov",Age = 81 },
        Year = 1950,
        Price = 9.99m
    },
     new Book()
    {
        Title = "Not now",
        Author = new Author() {Name = "Same Asimov",Age = 71 },
        Year = 1950,
        Price = 9.99m
    }

};

string xml = SerializeObject(books, "Books");

Console.WriteLine(xml);

List<Book> book2 = DeserializeXml<List<Book>>(xml, "Books");
;
//Exam!!!
string SerializeObject<T>(T data, string rootElement) where T : class
{
    string result = null!;
    XmlSerializer serializer = new(typeof(T), new XmlRootAttribute(rootElement));

    using (MemoryStream ms = new MemoryStream())
    {
       
        serializer.Serialize(ms, data);
        result = Encoding.UTF8.GetString(ms.ToArray());
    }
        return result;
}

T DeserializeXml<T>(string xml, string rootElement) where T : class
{
    T result = default(T);
    XmlSerializer xmlSerializer = new(typeof(T), new XmlRootAttribute(rootElement));

    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
    {     
        result = (T)xmlSerializer.Deserialize(ms);
    }
    return result;
}