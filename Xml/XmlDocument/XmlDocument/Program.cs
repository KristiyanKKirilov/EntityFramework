using System.Xml;
using System.Xml.Linq;

string people = @"<root>
        <person>
                <name>John Doe</name>
                <age>42</age>
        </person>
        <person>
                <name>Jane Doe</name>
                 <age>21</age>
        </person>
        </root>";

string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<library>
    <book>
        <author name = ""Isaax Azimov""/>
        <title>I, Robot</title>
        <price>20</price>
    </book>
 <book>
        <author name = ""J.R.R Tolkien""/>
        <title>Silmarilion</title>
        <price>30</price>
    </book>
 <book>
        <author name = ""Hamilton""/>
        <title>Pandora Star</title>
        <price>40</price>
    </book>
</library>";

XDocument doc = new();
doc = XDocument.Parse(xml);

XElement bookToRemove = doc.Root.Elements()
    .First(b => b.Element("title").Value == "Silmarilion");

bookToRemove.SetElementValue("price", null);

foreach (var book in doc.Root.Elements())
{
    string author = book.
        Element("author")
        .Attribute("name")
        .Value;

    string title = book.Element("title").Value;
    string price = book.Element("price")?.Value;

    Console.WriteLine($"{title}, {author}, {price}");
}

bookToRemove.SetElementValue("quantity", "0");
Console.WriteLine(doc);