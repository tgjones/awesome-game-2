#region Auto-generated classes for sd database on 2008-11-15 03:12:32Z

//
//  ____  _     __  __      _        _
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from sd on 2008-11-15 03:12:32Z
// Please visit http://linq.to/db for more information

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using DbLinq.Data.Linq;
//using DbLinq.Linq;
//using DbLinq.Linq.Mapping;

namespace SdLinq
{
	public partial class Sd : DbLinq.Data.Linq.DataContext
	{
		public Sd(System.Data.IDbConnection connection)
		: base(connection, new DbLinq.MySql.MySqlVendor())
		{
		}

		public Sd(System.Data.IDbConnection connection, DbLinq.Vendor.IVendor vendor)
		: base(connection, vendor)
		{
		}

		public Table<Commodities> Commodities { get { return GetTable<Commodities>(); } }
		public Table<LocationProcess> LocationProcess { get { return GetTable<LocationProcess>(); } }
		public Table<Locations> Locations { get { return GetTable<Locations>(); } }
		public Table<LocationStock> LocationStock { get { return GetTable<LocationStock>(); } }
		public Table<Players> Players { get { return GetTable<Players>(); } }
		public Table<ProcessConsumption> ProcessConsumption { get { return GetTable<ProcessConsumption>(); } }
		public Table<ProcessProduction> ProcessProduction { get { return GetTable<ProcessProduction>(); } }
		public Table<Transporters> Transporters { get { return GetTable<Transporters>(); } }
		public Table<TransportRoutes> TransportRoutes { get { return GetTable<TransportRoutes>(); } }
		public Table<TransportTypes> TransportTypes { get { return GetTable<TransportTypes>(); } }

	}

	[Table(Name = "sd.commodities")]
	public partial class Commodities : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint ID

		private uint _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public uint ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					OnPropertyChanged("ID");
				}
			}
		}

		#endregion

		#region string Name

		private string _name;
		[DebuggerNonUserCode]
		[Column(Storage = "_name", Name = "name", DbType = "varchar(255)", CanBeNull = false)]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (value != _name)
				{
					_name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.location_process")]
	public partial class LocationProcess : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint ID

		private uint _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public uint ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					OnPropertyChanged("ID");
				}
			}
		}

		#endregion

		#region int Interval

		private int _interval;
		[DebuggerNonUserCode]
		[Column(Storage = "_interval", Name = "interval", DbType = "int(10)", CanBeNull = false)]
		public int Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				if (value != _interval)
				{
					_interval = value;
					OnPropertyChanged("Interval");
				}
			}
		}

		#endregion

		#region System.DateTime LastProduced

		private System.DateTime _lastProduced;
		[DebuggerNonUserCode]
		[Column(Storage = "_lastProduced", Name = "last_produced", DbType = "datetime", CanBeNull = false)]
		public System.DateTime LastProduced
		{
			get
			{
				return _lastProduced;
			}
			set
			{
				if (value != _lastProduced)
				{
					_lastProduced = value;
					OnPropertyChanged("LastProduced");
				}
			}
		}

		#endregion

		#region uint LocationID

		private uint _locationID;
		[DebuggerNonUserCode]
		[Column(Storage = "_locationID", Name = "location_id", DbType = "int unsigned", CanBeNull = false)]
		public uint LocationID
		{
			get
			{
				return _locationID;
			}
			set
			{
				if (value != _locationID)
				{
					_locationID = value;
					OnPropertyChanged("LocationID");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.locations")]
	public partial class Locations : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint ID

		private uint _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public uint ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					OnPropertyChanged("ID");
				}
			}
		}

		#endregion

		#region decimal Latitude

		private decimal _latitude;
		[DebuggerNonUserCode]
		[Column(Storage = "_latitude", Name = "latitude", DbType = "decimal(13,10)", CanBeNull = false)]
		public decimal Latitude
		{
			get
			{
				return _latitude;
			}
			set
			{
				if (value != _latitude)
				{
					_latitude = value;
					OnPropertyChanged("Latitude");
				}
			}
		}

		#endregion

		#region decimal Longitude

		private decimal _longitude;
		[DebuggerNonUserCode]
		[Column(Storage = "_longitude", Name = "longitude", DbType = "decimal(13,10)", CanBeNull = false)]
		public decimal Longitude
		{
			get
			{
				return _longitude;
			}
			set
			{
				if (value != _longitude)
				{
					_longitude = value;
					OnPropertyChanged("Longitude");
				}
			}
		}

		#endregion

		#region string Name

		private string _name;
		[DebuggerNonUserCode]
		[Column(Storage = "_name", Name = "name", DbType = "varchar(255)", CanBeNull = false)]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (value != _name)
				{
					_name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.location_stock")]
	public partial class LocationStock : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint CommodityID

		private uint _commodityID;
		[DebuggerNonUserCode]
		[Column(Storage = "_commodityID", Name = "commodity_id", DbType = "int unsigned", CanBeNull = false)]
		public uint CommodityID
		{
			get
			{
				return _commodityID;
			}
			set
			{
				if (value != _commodityID)
				{
					_commodityID = value;
					OnPropertyChanged("CommodityID");
				}
			}
		}

		#endregion

		#region uint LocationID

		private uint _locationID;
		[DebuggerNonUserCode]
		[Column(Storage = "_locationID", Name = "location_id", DbType = "int unsigned", CanBeNull = false)]
		public uint LocationID
		{
			get
			{
				return _locationID;
			}
			set
			{
				if (value != _locationID)
				{
					_locationID = value;
					OnPropertyChanged("LocationID");
				}
			}
		}

		#endregion

		#region uint Maximum

		private uint _maximum;
		[DebuggerNonUserCode]
		[Column(Storage = "_maximum", Name = "maximum", DbType = "int unsigned", CanBeNull = false)]
		public uint Maximum
		{
			get
			{
				return _maximum;
			}
			set
			{
				if (value != _maximum)
				{
					_maximum = value;
					OnPropertyChanged("Maximum");
				}
			}
		}

		#endregion

		#region uint Price

		private uint _price;
		[DebuggerNonUserCode]
		[Column(Storage = "_price", Name = "price", DbType = "int unsigned", CanBeNull = false)]
		public uint Price
		{
			get
			{
				return _price;
			}
			set
			{
				if (value != _price)
				{
					_price = value;
					OnPropertyChanged("Price");
				}
			}
		}

		#endregion

		#region uint Quantity

		private uint _quantity;
		[DebuggerNonUserCode]
		[Column(Storage = "_quantity", Name = "quantity", DbType = "int unsigned", CanBeNull = false)]
		public uint Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				if (value != _quantity)
				{
					_quantity = value;
					OnPropertyChanged("Quantity");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.players")]
	public partial class Players : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint Balance

		private uint _balance;
		[DebuggerNonUserCode]
		[Column(Storage = "_balance", Name = "balance", DbType = "int unsigned", CanBeNull = false)]
		public uint Balance
		{
			get
			{
				return _balance;
			}
			set
			{
				if (value != _balance)
				{
					_balance = value;
					OnPropertyChanged("Balance");
				}
			}
		}

		#endregion

		#region string Email

		private string _email;
		[DebuggerNonUserCode]
		[Column(Storage = "_email", Name = "email", DbType = "varchar(255)", CanBeNull = false)]
		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				if (value != _email)
				{
					_email = value;
					OnPropertyChanged("Email");
				}
			}
		}

		#endregion

		#region uint ID

		private uint _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public uint ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					OnPropertyChanged("ID");
				}
			}
		}

		#endregion

		#region System.DateTime Joined

		private System.DateTime _joined;
		[DebuggerNonUserCode]
		[Column(Storage = "_joined", Name = "joined", DbType = "datetime", CanBeNull = false)]
		public System.DateTime Joined
		{
			get
			{
				return _joined;
			}
			set
			{
				if (value != _joined)
				{
					_joined = value;
					OnPropertyChanged("Joined");
				}
			}
		}

		#endregion

		#region System.DateTime? LastLogin

		private System.DateTime? _lastLogin;
		[DebuggerNonUserCode]
		[Column(Storage = "_lastLogin", Name = "last_login", DbType = "datetime")]
		public System.DateTime? LastLogin
		{
			get
			{
				return _lastLogin;
			}
			set
			{
				if (value != _lastLogin)
				{
					_lastLogin = value;
					OnPropertyChanged("LastLogin");
				}
			}
		}

		#endregion

		#region string Name

		private string _name;
		[DebuggerNonUserCode]
		[Column(Storage = "_name", Name = "name", DbType = "varchar(255)", CanBeNull = false)]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (value != _name)
				{
					_name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		#endregion

		#region string Password

		private string _password;
		[DebuggerNonUserCode]
		[Column(Storage = "_password", Name = "password", DbType = "varchar(255)", CanBeNull = false)]
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				if (value != _password)
				{
					_password = value;
					OnPropertyChanged("Password");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.process_consumption")]
	public partial class ProcessConsumption : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint CommodityID

		private uint _commodityID;
		[DebuggerNonUserCode]
		[Column(Storage = "_commodityID", Name = "commodity_id", DbType = "int unsigned", CanBeNull = false)]
		public uint CommodityID
		{
			get
			{
				return _commodityID;
			}
			set
			{
				if (value != _commodityID)
				{
					_commodityID = value;
					OnPropertyChanged("CommodityID");
				}
			}
		}

		#endregion

		#region uint ProcessID

		private uint _processID;
		[DebuggerNonUserCode]
		[Column(Storage = "_processID", Name = "process_id", DbType = "int unsigned", CanBeNull = false)]
		public uint ProcessID
		{
			get
			{
				return _processID;
			}
			set
			{
				if (value != _processID)
				{
					_processID = value;
					OnPropertyChanged("ProcessID");
				}
			}
		}

		#endregion

		#region uint? Quantity

		private uint? _quantity;
		[DebuggerNonUserCode]
		[Column(Storage = "_quantity", Name = "quantity", DbType = "int unsigned")]
		public uint? Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				if (value != _quantity)
				{
					_quantity = value;
					OnPropertyChanged("Quantity");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.process_production")]
	public partial class ProcessProduction : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint CommodityID

		private uint _commodityID;
		[DebuggerNonUserCode]
		[Column(Storage = "_commodityID", Name = "commodity_id", DbType = "int unsigned", CanBeNull = false)]
		public uint CommodityID
		{
			get
			{
				return _commodityID;
			}
			set
			{
				if (value != _commodityID)
				{
					_commodityID = value;
					OnPropertyChanged("CommodityID");
				}
			}
		}

		#endregion

		#region uint ProcessID

		private uint _processID;
		[DebuggerNonUserCode]
		[Column(Storage = "_processID", Name = "process_id", DbType = "int unsigned", CanBeNull = false)]
		public uint ProcessID
		{
			get
			{
				return _processID;
			}
			set
			{
				if (value != _processID)
				{
					_processID = value;
					OnPropertyChanged("ProcessID");
				}
			}
		}

		#endregion

		#region uint? Quantity

		private uint? _quantity;
		[DebuggerNonUserCode]
		[Column(Storage = "_quantity", Name = "quantity", DbType = "int unsigned")]
		public uint? Quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				if (value != _quantity)
				{
					_quantity = value;
					OnPropertyChanged("Quantity");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.transporters")]
	public partial class Transporters : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region int Capacity

		private int _capacity;
		[DebuggerNonUserCode]
		[Column(Storage = "_capacity", Name = "capacity", DbType = "int(10)", CanBeNull = false)]
		public int Capacity
		{
			get
			{
				return _capacity;
			}
			set
			{
				if (value != _capacity)
				{
					_capacity = value;
					OnPropertyChanged("Capacity");
				}
			}
		}

		#endregion

		#region int CommodityID

		private int _commodityID;
		[DebuggerNonUserCode]
		[Column(Storage = "_commodityID", Name = "commodity_id", DbType = "int(10)", CanBeNull = false)]
		public int CommodityID
		{
			get
			{
				return _commodityID;
			}
			set
			{
				if (value != _commodityID)
				{
					_commodityID = value;
					OnPropertyChanged("CommodityID");
				}
			}
		}

		#endregion

		#region decimal DistanceTravelLed

		private decimal _distanceTravelLed;
		[DebuggerNonUserCode]
		[Column(Storage = "_distanceTravelLed", Name = "distance_travelled", DbType = "decimal(13,10)", CanBeNull = false)]
		public decimal DistanceTravelLed
		{
			get
			{
				return _distanceTravelLed;
			}
			set
			{
				if (value != _distanceTravelLed)
				{
					_distanceTravelLed = value;
					OnPropertyChanged("DistanceTravelLed");
				}
			}
		}

		#endregion

		#region uint ID

		private uint _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
		public uint ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					OnPropertyChanged("ID");
				}
			}
		}

		#endregion

		#region int Load

		private int _load;
		[DebuggerNonUserCode]
		[Column(Storage = "_load", Name = "load", DbType = "int(10)", CanBeNull = false)]
		public int Load
		{
			get
			{
				return _load;
			}
			set
			{
				if (value != _load)
				{
					_load = value;
					OnPropertyChanged("Load");
				}
			}
		}

		#endregion

		#region uint PlayerID

		private uint _playerID;
		[DebuggerNonUserCode]
		[Column(Storage = "_playerID", Name = "player_id", DbType = "int unsigned", CanBeNull = false)]
		public uint PlayerID
		{
			get
			{
				return _playerID;
			}
			set
			{
				if (value != _playerID)
				{
					_playerID = value;
					OnPropertyChanged("PlayerID");
				}
			}
		}

		#endregion

		#region uint RouteID

		private uint _routeID;
		[DebuggerNonUserCode]
		[Column(Storage = "_routeID", Name = "route_id", DbType = "int unsigned", CanBeNull = false)]
		public uint RouteID
		{
			get
			{
				return _routeID;
			}
			set
			{
				if (value != _routeID)
				{
					_routeID = value;
					OnPropertyChanged("RouteID");
				}
			}
		}

		#endregion

		#region int TransportTypeID

		private int _transportTypeID;
		[DebuggerNonUserCode]
		[Column(Storage = "_transportTypeID", Name = "transport_type_id", DbType = "int(10)", CanBeNull = false)]
		public int TransportTypeID
		{
			get
			{
				return _transportTypeID;
			}
			set
			{
				if (value != _transportTypeID)
				{
					_transportTypeID = value;
					OnPropertyChanged("TransportTypeID");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.transport_routes")]
	public partial class TransportRoutes : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region int Cost

		private int _cost;
		[DebuggerNonUserCode]
		[Column(Storage = "_cost", Name = "cost", DbType = "int(10)", CanBeNull = false)]
		public int Cost
		{
			get
			{
				return _cost;
			}
			set
			{
				if (value != _cost)
				{
					_cost = value;
					OnPropertyChanged("Cost");
				}
			}
		}

		#endregion

		#region decimal Distance

		private decimal _distance;
		[DebuggerNonUserCode]
		[Column(Storage = "_distance", Name = "distance", DbType = "decimal(13,10)", CanBeNull = false)]
		public decimal Distance
		{
			get
			{
				return _distance;
			}
			set
			{
				if (value != _distance)
				{
					_distance = value;
					OnPropertyChanged("Distance");
				}
			}
		}

		#endregion

		#region uint FromLocationID

		private uint _fromLocationID;
		[DebuggerNonUserCode]
		[Column(Storage = "_fromLocationID", Name = "from_location_id", DbType = "int unsigned", CanBeNull = false)]
		public uint FromLocationID
		{
			get
			{
				return _fromLocationID;
			}
			set
			{
				if (value != _fromLocationID)
				{
					_fromLocationID = value;
					OnPropertyChanged("FromLocationID");
				}
			}
		}

		#endregion

		#region uint ID

		private uint _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
		public uint ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					OnPropertyChanged("ID");
				}
			}
		}

		#endregion

		#region uint? PlayerID

		private uint? _playerID;
		[DebuggerNonUserCode]
		[Column(Storage = "_playerID", Name = "player_id", DbType = "int unsigned")]
		public uint? PlayerID
		{
			get
			{
				return _playerID;
			}
			set
			{
				if (value != _playerID)
				{
					_playerID = value;
					OnPropertyChanged("PlayerID");
				}
			}
		}

		#endregion

		#region decimal Speed

		private decimal _speed;
		[DebuggerNonUserCode]
		[Column(Storage = "_speed", Name = "speed", DbType = "decimal(13,10)", CanBeNull = false)]
		public decimal Speed
		{
			get
			{
				return _speed;
			}
			set
			{
				if (value != _speed)
				{
					_speed = value;
					OnPropertyChanged("Speed");
				}
			}
		}

		#endregion

		#region decimal State

		private decimal _state;
		[DebuggerNonUserCode]
		[Column(Storage = "_state", Name = "state", DbType = "decimal(13,10)", CanBeNull = false)]
		public decimal State
		{
			get
			{
				return _state;
			}
			set
			{
				if (value != _state)
				{
					_state = value;
					OnPropertyChanged("State");
				}
			}
		}

		#endregion

		#region int ToLocationID

		private int _toLocationID;
		[DebuggerNonUserCode]
		[Column(Storage = "_toLocationID", Name = "to_location_id", DbType = "int(10)", CanBeNull = false)]
		public int ToLocationID
		{
			get
			{
				return _toLocationID;
			}
			set
			{
				if (value != _toLocationID)
				{
					_toLocationID = value;
					OnPropertyChanged("ToLocationID");
				}
			}
		}

		#endregion

		#region uint TransportTypeID

		private uint _transportTypeID;
		[DebuggerNonUserCode]
		[Column(Storage = "_transportTypeID", Name = "transport_type_id", DbType = "int unsigned", CanBeNull = false)]
		public uint TransportTypeID
		{
			get
			{
				return _transportTypeID;
			}
			set
			{
				if (value != _transportTypeID)
				{
					_transportTypeID = value;
					OnPropertyChanged("TransportTypeID");
				}
			}
		}

		#endregion

	}

	[Table(Name = "sd.transport_types")]
	public partial class TransportTypes : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged handling

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region uint ID

		private uint _id;
		[DebuggerNonUserCode]
		[Column(Storage = "_id", Name = "id", DbType = "int unsigned", IsPrimaryKey = true, CanBeNull = false)]
		public uint ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					OnPropertyChanged("ID");
				}
			}
		}

		#endregion

		#region string Medium

		private string _medium;
		[DebuggerNonUserCode]
		[Column(Storage = "_medium", Name = "medium", DbType = "varchar(255)", CanBeNull = false)]
		public string Medium
		{
			get
			{
				return _medium;
			}
			set
			{
				if (value != _medium)
				{
					_medium = value;
					OnPropertyChanged("Medium");
				}
			}
		}

		#endregion

		#region string Vehicle

		private string _vehicle;
		[DebuggerNonUserCode]
		[Column(Storage = "_vehicle", Name = "vehicle", DbType = "varchar(255)", CanBeNull = false)]
		public string Vehicle
		{
			get
			{
				return _vehicle;
			}
			set
			{
				if (value != _vehicle)
				{
					_vehicle = value;
					OnPropertyChanged("Vehicle");
				}
			}
		}

		#endregion

	}
}
