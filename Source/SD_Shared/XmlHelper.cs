using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using System.Xml.Linq;

using System.Xml.Serialization;

namespace SD.Shared
{
    public static class XmlHelper
    {
        #region SerialiseLocationList
        public static List<LocationInfo> DeserialiseLocationList(Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<LocationInfo>));
            return (List<LocationInfo>)xs.Deserialize(stream);
        }

        public static void SerialiseLocationList(List<LocationInfo> locationList, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<LocationInfo>));
            xs.Serialize(stream, locationList);
        }
        #endregion //SerialiseLocationList

        #region Serialise PlayerList
        public static void SerialisePlayerList(List<PlayerInfo> playerList, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<PlayerInfo>));
            xs.Serialize(stream, playerList);     
        }

        public static List<PlayerInfo> DeserialisePlayerList(Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<PlayerInfo>));
            return (List<PlayerInfo>)xs.Deserialize(stream);
        }
        #endregion //Serialise PlayerList

        #region Serialise Player
        public static void SerialisePlayer(PlayerInfo player, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(PlayerInfo));
            xs.Serialize(stream, player);
        }

        public static PlayerInfo DeserialisePlayer(Stream stream)
        {
            PlayerInfo player;
            XmlSerializer xs = new XmlSerializer(typeof(PlayerInfo));
            player = (PlayerInfo)xs.Deserialize(stream);
            return player;
        }
        #endregion //Serialise Player

        #region Serialise RouteList
        public static void SerialiseRouteList(List<RouteInfo> routeList, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<RouteInfo>));
            xs.Serialize(stream, routeList); 
        }

        public static List<RouteInfo> DeserialiseRouteList(Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<RouteInfo>));
            return (List<RouteInfo>)xs.Deserialize(stream);
        }
        #endregion //Serialise RouteList

        #region Serialise TransporterList
        public static void SerialiseTransporterList(List<TransporterInfo> transporterList, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<TransporterInfo>));
            xs.Serialize(stream, transporterList); 
        }

        public static List<TransporterInfo> DeserialiseTransporterList(Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<TransporterInfo>));
            return (List<TransporterInfo>)xs.Deserialize(stream);
        }
        #endregion //Serialise TransporterList

        #region Serialise MessageList
        public static void SerialiseMessageList(List<MessageInfo> messageList, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<MessageInfo>));
            xs.Serialize(stream, messageList);
        }

        public static List<MessageInfo> DeserialiseMessageList(Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<MessageInfo>));
            return (List<MessageInfo>)xs.Deserialize(stream);
        }

        #endregion // Serialise MessageList

        #region Serialise LoginInfo
        public static void SerialiseLoginInfo(LoginInfo login, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(LoginInfo));
            xs.Serialize(stream, login);
        }

        public static LoginInfo DeserialiseLoginInfo(Stream stream)
        {
            LoginInfo login;
            XmlSerializer xs = new XmlSerializer(typeof(LoginInfo));
            login = (LoginInfo)xs.Deserialize(stream);
            return login;
        }
        #endregion //Serialise LoginInfo

        #region Serialise RequestReply
        public static void SerialiseRequestReply(RequestReply RequestReply, Stream stream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(RequestReply));
            xs.Serialize(stream, RequestReply);
        }

        public static RequestReply DeserialiseRequestReply(Stream stream)
        {
            RequestReply RequestReply;
            XmlSerializer xs = new XmlSerializer(typeof(RequestReply));
            RequestReply = (RequestReply)xs.Deserialize(stream);
            return RequestReply;
        }
        #endregion //Serialise Player

    }
}
