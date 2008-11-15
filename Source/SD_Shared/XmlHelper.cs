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
            internal const string players = "players";
            internal const string player = "player";
            internal const string email = "email";
            internal const string password = "password";
            internal const string joined = "joined";
            internal const string last_login = "last_login";
            internal const string balance = "balance";
            internal const string transporters = "tranporters";
            internal const string transporter = "tranporter";
            internal const string player_id = "player_id";
            internal const string route_id = "route_id";
            internal const string last_moved = "last_moved";
            internal const string distance_travelled = "distance_travelled";
            internal const string commodity_id = "commodity_id";
            internal const string capacity = "capacity";
            internal const string load = "load";
            internal const string transport_type_id = "transport_type_id";
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

        public static void SerialisePlayerList(List<PlayerInfo> playerList, Stream stream)
        {
            if (playerList == null)
                return;

            XElement tree = new XElement(xNames.players,
                from l in playerList
                select
                    new XElement(xNames.player,
                        new XAttribute(xNames.id, l.Id),
                        new XAttribute(xNames.email, l.Email),
                        new XAttribute(xNames.password, l.Password),
                        new XAttribute(xNames.name, l.Name),
                        new XAttribute(xNames.joined, l.Joined),
                        new XAttribute(xNames.last_login, l.LastLogin),
                        new XAttribute(xNames.balance, l.Balance)
                        )
                    );

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(stream, settings);
            tree.WriteTo(writer);
            writer.Flush();
        }

        public static void SerialiseTransporterList(List<TransporterInfo> transporterList, Stream stream)
        {
            if (transporterList == null)
                return;

            XElement tree = new XElement(xNames.transporters,
                from l in transporterList
                select
                    new XElement(xNames.transporter,
                        new XAttribute(xNames.id, l.Id),
                        new XAttribute(xNames.player_id, l.PlayerId),
                        new XAttribute(xNames.route_id, l.RouteId),
                        new XAttribute(xNames.last_moved, l.LastMoved),
                        new XAttribute(xNames.distance_travelled, l.DistanceTravelled),
                        new XAttribute(xNames.commodity_id, l.CommodityId),
                        new XAttribute(xNames.capacity, l.Capacity),
                        new XAttribute(xNames.load, l.Load),
                        new XAttribute(xNames.transport_type_id, l.TransportTypeId)
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
