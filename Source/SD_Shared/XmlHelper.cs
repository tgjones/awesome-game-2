using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SD.Shared
{
    public static class XmlHelper
    {
        static class xNames
        {
            // string literals for XML
            internal const string locations = "locations";
            internal const string id = "id";
            internal const string name = "name";
            internal const string location = "location";
            internal const string latitude = "latitude";
            internal const string longitude = "longitude";
            internal const string stocks = "stocks";
            internal const string stock = "stock";
            internal const string unitprice = "unitprice";
            internal const string quantity = "quantity";
            internal const string resourcetype = "resourcetype";
        }

        public static List<LocationInfo> DeserialiseLocationList(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            XElement tree = null;
            List<LocationInfo> result = null;

            using (XmlReader xr = XmlReader.Create(stream))
            {
                tree = XElement.Load(xr);
            }
            if (tree != null)
            {
                result = new List<LocationInfo>(from xe in tree.Elements(xNames.location)
                                                select
                                                    new LocationInfo((int)xe.Attribute(xNames.id),
                                                        (int)xe.Attribute(xNames.latitude),
                                                        (int)xe.Attribute(xNames.longitude),
                                                        (string)xe.Attribute(xNames.name),
                                                        new List<StockInfo>(from xs in xe.Element(xNames.stocks).Elements(xNames.stock)
                                                                            select
                                                                                new StockInfo((ResourceEnum)Enum.Parse(typeof(ResourceEnum),(string)xs.Attribute(xNames.resourcetype)),
                                                                                    (int)xs.Attribute(xNames.quantity),
                                                                                    (int)xs.Attribute(xNames.unitprice)
                                                                                    )
                                                                                )
                                                                            )
                                                                        );
            }

            return result;
        }

        public static void SerialiseLocationList(List<LocationInfo> locationList, Stream stream)
        {
            if (locationList == null)
                return;

            XElement tree = new XElement(xNames.locations,
                from l in locationList
                select
                    new XElement(xNames.location,
                        new XAttribute(xNames.id, l.Id),
                        new XAttribute(xNames.latitude, l.Latitude),
                        new XAttribute(xNames.longitude, l.Longitude),
                        new XAttribute(xNames.name, l.Name),
                        new XElement(xNames.stocks,
                            from s in l.Stocks
                            select
                                new XElement(xNames.stock,
                                    new XAttribute(xNames.resourcetype, s.ResourceType),
                                    new XAttribute(xNames.quantity, s.Quantity),
                                    new XAttribute(xNames.unitprice, s.UnitPrice)
                                )
                            )
                        )
                    );

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(stream, settings);
            tree.WriteTo(writer);
            writer.Flush();

        }

    }
}
