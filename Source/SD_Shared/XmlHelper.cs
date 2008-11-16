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
            internal const string playerid = "playerid";
            internal const string routeid = "routeid";
            internal const string lastmoved = "lastmoved";
            internal const string distancetravelled = "distancetravelled";
            internal const string capacity = "capacity";
            internal const string transporttypeid = "transporttypeid";
            internal const string boughtprice = "boughtprice";
            internal const string routes = "routes";
            internal const string route = "route";
            internal const string fromlocationid = "fromlocationid";
            internal const string tolocationid = "tolocationid";
            internal const string speed = "speed";
            internal const string cost = "cost";
            internal const string state = "state";
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
																									new LocationInfo((int) xe.Attribute(xNames.id),
																											Convert.ToDecimal(xe.Attribute(xNames.latitude).Value),
																											Convert.ToDecimal(xe.Attribute(xNames.longitude).Value),
																											(string) xe.Attribute(xNames.name),
																											new List<StockInfo>(from xs in xe.Element(xNames.stocks).Elements(xNames.stock)
																																					select
																																							new StockInfo((ResourceEnum) Enum.Parse(typeof(ResourceEnum), (string) xs.Attribute(xNames.resourcetype)),
																																									Convert.ToInt32(xs.Attribute(xNames.quantity).Value),
																																									Convert.ToInt32(xs.Attribute(xNames.unitprice).Value)
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

        public static void SerialiseRouteList(List<RouteInfo> routeList, Stream stream)
        {
           if (routeList == null)
                return;

            XElement tree = new XElement(xNames.routes,
                from l in routeList
                select
                    new XElement(xNames.route,
                        new XAttribute(xNames.id, l.Id),
                        new XAttribute(xNames.speed, l.Speed),
                        new XAttribute(xNames.cost, l.Cost),
                        new XAttribute(xNames.state, l.State),
                        new XElement(xNames.locations,
                            new XElement(xNames.location,
                                new XAttribute(xNames.id, l.FromLocationId),
                                new XAttribute(xNames.latitude, l.FromLocation.Latitude),
                                new XAttribute(xNames.longitude, l.FromLocation.Longitude),
                                new XAttribute(xNames.name, l.FromLocation.Name)
                            ),
                            new XElement(xNames.location,
                                new XAttribute(xNames.id, l.ToLocationId),
                                new XAttribute(xNames.latitude, l.ToLocation.Latitude),
                                new XAttribute(xNames.longitude, l.ToLocation.Longitude),
                                new XAttribute(xNames.name, l.ToLocation.Name)
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

        public static void SerialiseTransporterList(List<TransporterInfo> transporterList, Stream stream)
        {
            if (transporterList == null)
                return;

            XElement tree = new XElement(xNames.transporters,
                from l in transporterList
                select
                    new XElement(xNames.transporter,
                        new XAttribute(xNames.id, l.Id),
                        new XAttribute(xNames.playerid, l.PlayerId),
                        new XAttribute(xNames.routeid, l.RouteId),
                        new XAttribute(xNames.lastmoved, l.LastMoved),
                        new XAttribute(xNames.distancetravelled, l.DistanceTravelled),
                        new XAttribute(xNames.capacity, l.Capacity),
                        new XElement(xNames.stocks,
                            from s in l.Stocks
                            select
                                new XElement(xNames.stock,
                                    new XAttribute(xNames.resourcetype, s.ResourceType),
                                    new XAttribute(xNames.quantity, s.Quantity),
                                    new XAttribute(xNames.boughtprice, s.UnitPrice)
                                )
                            ),
                        new XAttribute(xNames.transporttypeid, l.TransportTypeId)
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
