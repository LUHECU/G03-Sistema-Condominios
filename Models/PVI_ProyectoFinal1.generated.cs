//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1573, 1591

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace DataModels
{
	/// <summary>
	/// Database       : PVI_ProyectoFinal
	/// Data Source    : Camila
	/// Server Version : 16.00.1000
	/// </summary>
	public partial class PviProyectoFinalDB : LinqToDB.Data.DataConnection
	{
		public ITable<Bitacora>     Bitacoras      { get { return this.GetTable<Bitacora>(); } }
		public ITable<Casa>         Casas          { get { return this.GetTable<Casa>(); } }
		public ITable<Categoria>    Categorias     { get { return this.GetTable<Categoria>(); } }
		public ITable<Cobro>        Cobros         { get { return this.GetTable<Cobro>(); } }
		public ITable<DetalleCobro> DetalleCobroes { get { return this.GetTable<DetalleCobro>(); } }
		public ITable<Persona>      Personas       { get { return this.GetTable<Persona>(); } }
		public ITable<Servicio>     Servicios      { get { return this.GetTable<Servicio>(); } }

		public PviProyectoFinalDB()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public PviProyectoFinalDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		public PviProyectoFinalDB(DataOptions options)
			: base(options)
		{
			InitDataContext();
			InitMappingSchema();
		}

		public PviProyectoFinalDB(DataOptions<PviProyectoFinalDB> options)
			: base(options.Options)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	[Table(Schema="dbo", Name="Bitacora")]
	public partial class Bitacora
	{
		[Column("id_bitacora"), PrimaryKey, Identity] public int      IdBitacora { get; set; } // int
		[Column("detalle"),     NotNull             ] public string   Detalle    { get; set; } // varchar(255)
		[Column("id_cobro"),    NotNull             ] public int      IdCobro    { get; set; } // int
		[Column("id_user"),     NotNull             ] public int      IdUser     { get; set; } // int
		[Column("fecha"),       NotNull             ] public DateTime Fecha      { get; set; } // datetime

		#region Associations

		/// <summary>
		/// fk_id_cobro_Bitacora (dbo.Cobros)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=false)]
		public Cobro Fkidcobro { get; set; }

		/// <summary>
		/// fk_id_user (dbo.Persona)
		/// </summary>
		[Association(ThisKey="IdUser", OtherKey="IdPersona", CanBeNull=false)]
		public Persona Fkiduser { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Casas")]
	public partial class Casa
	{
		[Column("id_casa"),             PrimaryKey,  Identity] public int      IdCasa             { get; set; } // int
		[Column("nombre_casa"),         NotNull              ] public string   NombreCasa         { get; set; } // nvarchar(255)
		[Column("metros_cuadrados"),    NotNull              ] public int      MetrosCuadrados    { get; set; } // int
		[Column("numero_habitaciones"), NotNull              ] public int      NumeroHabitaciones { get; set; } // int
		[Column("numero_banos"),        NotNull              ] public int      NumeroBanos        { get; set; } // int
		[Column("precio"),              NotNull              ] public decimal  Precio             { get; set; } // decimal(15, 2)
		[Column("id_persona"),          NotNull              ] public int      IdPersona          { get; set; } // int
		[Column("fecha_construccion"),  NotNull              ] public DateTime FechaConstruccion  { get; set; } // date
		[Column("estado"),                 Nullable          ] public bool?    Estado             { get; set; } // bit

		#region Associations

		/// <summary>
		/// fk_id_casa_BackReference (dbo.Cobros)
		/// </summary>
		[Association(ThisKey="IdCasa", OtherKey="IdCasa", CanBeNull=true)]
		public IEnumerable<Cobro> Fkidcasas { get; set; }

		/// <summary>
		/// fk_id_persona (dbo.Persona)
		/// </summary>
		[Association(ThisKey="IdPersona", OtherKey="IdPersona", CanBeNull=false)]
		public Persona Fkidpersona { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Categorias")]
	public partial class Categoria
	{
		[Column("id_categoria"), PrimaryKey,  Identity] public int    IdCategoria { get; set; } // int
		[Column("nombre"),       NotNull              ] public string Nombre      { get; set; } // varchar(100)
		[Column("descripcion"),     Nullable          ] public string Descripcion { get; set; } // text
		[Column("estado"),       NotNull              ] public bool   Estado      { get; set; } // bit

		#region Associations

		/// <summary>
		/// FK__Servicios__id_ca__3F466844_BackReference (dbo.Servicios)
		/// </summary>
		[Association(ThisKey="IdCategoria", OtherKey="IdCategoria", CanBeNull=true)]
		public IEnumerable<Servicio> Serviciosidca3F { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Cobros")]
	public partial class Cobro
	{
		[Column("id_cobro"),     PrimaryKey,  Identity] public int       IdCobro     { get; set; } // int
		[Column("id_casa"),      NotNull              ] public int       IdCasa      { get; set; } // int
		[Column("mes"),          NotNull              ] public int       Mes         { get; set; } // int
		[Column("anno"),         NotNull              ] public int       Anno        { get; set; } // int
		[Column("estado"),          Nullable          ] public string    Estado      { get; set; } // varchar(50)
		[Column("monto"),        NotNull              ] public decimal   Monto       { get; set; } // decimal(15, 2)
		[Column("fecha_pagada"),    Nullable          ] public DateTime? FechaPagada { get; set; } // date

		#region Associations

		/// <summary>
		/// fk_id_casa (dbo.Casas)
		/// </summary>
		[Association(ThisKey="IdCasa", OtherKey="IdCasa", CanBeNull=false)]
		public Casa Fkidcasa { get; set; }

		/// <summary>
		/// fk_id_cobro_Bitacora_BackReference (dbo.Bitacora)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=true)]
		public IEnumerable<Bitacora> FkidcobroBitacoras { get; set; }

		/// <summary>
		/// fk_id_cobro_Detalle_BackReference (dbo.DetalleCobro)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=true)]
		public IEnumerable<DetalleCobro> FkidcobroDetalles { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="DetalleCobro")]
	public partial class DetalleCobro
	{
		[Column("id_servicio"), NotNull] public int IdServicio { get; set; } // int
		[Column("id_cobro"),    NotNull] public int IdCobro    { get; set; } // int

		#region Associations

		/// <summary>
		/// fk_id_cobro_Detalle (dbo.Cobros)
		/// </summary>
		[Association(ThisKey="IdCobro", OtherKey="IdCobro", CanBeNull=false)]
		public Cobro FkidcobroDetalle { get; set; }

		/// <summary>
		/// fk_id_servicio (dbo.Servicios)
		/// </summary>
		[Association(ThisKey="IdServicio", OtherKey="IdServicio", CanBeNull=false)]
		public Servicio Fkidservicio { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Persona")]
	public partial class Persona
	{
		[Column("id_persona"),       PrimaryKey,  Identity] public int       IdPersona       { get; set; } // int
		[Column("nombre"),           NotNull              ] public string    Nombre          { get; set; } // varchar(100)
		[Column("apellido"),         NotNull              ] public string    Apellido        { get; set; } // varchar(100)
		[Column("email"),            NotNull              ] public string    Email           { get; set; } // varchar(150)
		[Column("telefono"),            Nullable          ] public string    Telefono        { get; set; } // varchar(15)
		[Column("direccion"),           Nullable          ] public string    Direccion       { get; set; } // varchar(255)
		[Column("fecha_nacimiento"),    Nullable          ] public DateTime? FechaNacimiento { get; set; } // date
		[Column("contrasena"),          Nullable          ] public string    Contrasena      { get; set; } // varchar(255)
		[Column("estado"),              Nullable          ] public bool?     Estado          { get; set; } // bit
		[Column("tipo_persona"),        Nullable          ] public string    TipoPersona     { get; set; } // varchar(50)

		#region Associations

		/// <summary>
		/// fk_id_persona_BackReference (dbo.Casas)
		/// </summary>
		[Association(ThisKey="IdPersona", OtherKey="IdPersona", CanBeNull=true)]
		public IEnumerable<Casa> Fkidpersonas { get; set; }

		/// <summary>
		/// fk_id_user_BackReference (dbo.Bitacora)
		/// </summary>
		[Association(ThisKey="IdPersona", OtherKey="IdUser", CanBeNull=true)]
		public IEnumerable<Bitacora> Fkidusers { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Servicios")]
	public partial class Servicio
	{
		[Column("id_servicio"),  PrimaryKey,  Identity] public int     IdServicio  { get; set; } // int
		[Column("nombre"),       NotNull              ] public string  Nombre      { get; set; } // varchar(100)
		[Column("descripcion"),     Nullable          ] public string  Descripcion { get; set; } // text
		[Column("precio"),       NotNull              ] public decimal Precio      { get; set; } // decimal(10, 2)
		[Column("id_categoria"), NotNull              ] public int     IdCategoria { get; set; } // int
		[Column("estado"),       NotNull              ] public bool    Estado      { get; set; } // bit

		#region Associations

		/// <summary>
		/// fk_id_servicio_BackReference (dbo.DetalleCobro)
		/// </summary>
		[Association(ThisKey="IdServicio", OtherKey="IdServicio", CanBeNull=true)]
		public IEnumerable<DetalleCobro> Fkidservicios { get; set; }

		/// <summary>
		/// FK__Servicios__id_ca__3F466844 (dbo.Categorias)
		/// </summary>
		[Association(ThisKey="IdCategoria", OtherKey="IdCategoria", CanBeNull=false)]
		public Categoria Idca3F { get; set; }

		#endregion
	}

	public static partial class PviProyectoFinalDBStoredProcedures
	{
		#region SpAlterdiagram

		public static int SpAlterdiagram(this PviProyectoFinalDB dataConnection, string @diagramname, int? @ownerId, int? @version, byte[] @definition)
		{
			var parameters = new []
			{
				new DataParameter("@diagramname", @diagramname, LinqToDB.DataType.NVarChar)
				{
					Size = 128
				},
				new DataParameter("@owner_id",    @ownerId,     LinqToDB.DataType.Int32),
				new DataParameter("@version",     @version,     LinqToDB.DataType.Int32),
				new DataParameter("@definition",  @definition,  LinqToDB.DataType.VarBinary)
				{
					Size = -1
				}
			};

			return dataConnection.ExecuteProc("[dbo].[sp_alterdiagram]", parameters);
		}

		#endregion

		#region SpConsultarBitacora

		public static IEnumerable<SpConsultarBitacoraResult> SpConsultarBitacora(this PviProyectoFinalDB dataConnection, int? @idCobro)
		{
			var parameters = new []
			{
				new DataParameter("@id_cobro", @idCobro, LinqToDB.DataType.Int32)
			};

			return dataConnection.QueryProc<SpConsultarBitacoraResult>("[dbo].[SpConsultarBitacora]", parameters);
		}

		public partial class SpConsultarBitacoraResult
		{
			[Column("fecha")  ] public DateTime Fecha   { get; set; }
			[Column("detalle")] public string   Detalle { get; set; }
			                    public string   Usuario { get; set; }
		}

		#endregion

		#region SpConsultarCasas

		public static IEnumerable<SpConsultarCasasResult> SpConsultarCasas(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<SpConsultarCasasResult>("[dbo].[SpConsultarCasas]");
		}

		public partial class SpConsultarCasasResult
		{
			[Column("id_casa")            ] public int      Id_casa             { get; set; }
			[Column("nombre_casa")        ] public string   Nombre_casa         { get; set; }
			[Column("metros_cuadrados")   ] public int      Metros_cuadrados    { get; set; }
			[Column("numero_habitaciones")] public int      Numero_habitaciones { get; set; }
			[Column("numero_banos")       ] public int      Numero_banos        { get; set; }
			                                public string   Propietario         { get; set; }
			[Column("fecha_construccion") ] public DateTime Fecha_construccion  { get; set; }
			[Column("estado")             ] public bool?    Estado              { get; set; }
			[Column("precio")             ] public decimal  Precio              { get; set; }
		}

		#endregion

		#region SpConsultarCasasActivasPorCliente

		public static IEnumerable<SpConsultarCasasActivasPorClienteResult> SpConsultarCasasActivasPorCliente(this PviProyectoFinalDB dataConnection, int? @idPersona)
		{
			var parameters = new []
			{
				new DataParameter("@id_persona", @idPersona, LinqToDB.DataType.Int32)
			};

			return dataConnection.QueryProc<SpConsultarCasasActivasPorClienteResult>("[dbo].[SpConsultarCasasActivasPorCliente]", parameters);
		}

		public partial class SpConsultarCasasActivasPorClienteResult
		{
			[Column("id_casa")    ] public int    Id_casa     { get; set; }
			[Column("nombre_casa")] public string Nombre_casa { get; set; }
		}

		#endregion

		#region SpConsultarCasasPorID

		public static IEnumerable<SpConsultarCasasPorIDResult> SpConsultarCasasPorID(this PviProyectoFinalDB dataConnection, int? @idCasa)
		{
			var parameters = new []
			{
				new DataParameter("@id_casa", @idCasa, LinqToDB.DataType.Int32)
			};

			var ms = dataConnection.MappingSchema;

			return dataConnection.QueryProc(dataReader =>
				new SpConsultarCasasPorIDResult
				{
					Id_casa             = Converter.ChangeTypeTo<int>     (dataReader.GetValue(0), ms),
					Nombre_casa         = Converter.ChangeTypeTo<string>  (dataReader.GetValue(1), ms),
					Metros_cuadrados    = Converter.ChangeTypeTo<int>     (dataReader.GetValue(2), ms),
					Numero_habitaciones = Converter.ChangeTypeTo<int>     (dataReader.GetValue(3), ms),
					Numero_banos        = Converter.ChangeTypeTo<int>     (dataReader.GetValue(4), ms),
					Precio              = Converter.ChangeTypeTo<decimal> (dataReader.GetValue(5), ms),
					Id_persona          = Converter.ChangeTypeTo<int>     (dataReader.GetValue(6), ms),
					Fecha_construccion  = Converter.ChangeTypeTo<DateTime>(dataReader.GetValue(7), ms),
					Estado              = Converter.ChangeTypeTo<bool?>   (dataReader.GetValue(8), ms),
					Column10            = Converter.ChangeTypeTo<decimal> (dataReader.GetValue(9), ms),
				},
				"[dbo].[SpConsultarCasasPorID]", parameters);
		}

		public partial class SpConsultarCasasPorIDResult
		{
			[Column("id_casa")            ] public int      Id_casa             { get; set; }
			[Column("nombre_casa")        ] public string   Nombre_casa         { get; set; }
			[Column("metros_cuadrados")   ] public int      Metros_cuadrados    { get; set; }
			[Column("numero_habitaciones")] public int      Numero_habitaciones { get; set; }
			[Column("numero_banos")       ] public int      Numero_banos        { get; set; }
			[Column("precio")             ] public decimal  Precio              { get; set; }
			[Column("id_persona")         ] public int      Id_persona          { get; set; }
			[Column("fecha_construccion") ] public DateTime Fecha_construccion  { get; set; }
			[Column("estado")             ] public bool?    Estado              { get; set; }
			[Column("precio")             ] public decimal  Column10            { get; set; }
		}

		#endregion

		#region SpConsultarCategoriasServicios

		public static IEnumerable<SpConsultarCategoriasServiciosResult> SpConsultarCategoriasServicios(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<SpConsultarCategoriasServiciosResult>("[dbo].[spConsultarCategoriasServicios]");
		}

		public partial class SpConsultarCategoriasServiciosResult
		{
			[Column("id_categoria")] public int    Id_categoria { get; set; }
			[Column("nombre")      ] public string Nombre       { get; set; }
		}

		#endregion

		#region SpConsultarClientesActivos

		public static IEnumerable<SpConsultarClientesActivosResult> SpConsultarClientesActivos(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<SpConsultarClientesActivosResult>("[dbo].[SpConsultarClientesActivos]");
		}

		public partial class SpConsultarClientesActivosResult
		{
			[Column("id_persona")] public int    Id_persona { get; set; }
			                       public string Cliente    { get; set; }
		}

		#endregion

		#region SpConsultarCobroPorCliente

		public static IEnumerable<SpConsultarCobroPorClienteResult> SpConsultarCobroPorCliente(this PviProyectoFinalDB dataConnection, int? @idPersona)
		{
			var parameters = new []
			{
				new DataParameter("@id_persona", @idPersona, LinqToDB.DataType.Int32)
			};

			return dataConnection.QueryProc<SpConsultarCobroPorClienteResult>("[dbo].[SpConsultarCobroPorCliente]", parameters);
		}

		public partial class SpConsultarCobroPorClienteResult
		{
			[Column("id_cobro")    ] public int       Id_cobro     { get; set; }
			[Column("id_casa")     ] public int       Id_casa      { get; set; }
			[Column("nombre_casa") ] public string    Nombre_casa  { get; set; }
			                         public string    Persona      { get; set; }
			                         public int?      Periodo      { get; set; }
			[Column("estado")      ] public string    Estado       { get; set; }
			[Column("monto")       ] public decimal   Monto        { get; set; }
			[Column("fecha_pagada")] public DateTime? Fecha_pagada { get; set; }
		}

		#endregion

		#region SpConsultarCobros

		public static IEnumerable<SpConsultarCobrosResult> SpConsultarCobros(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<SpConsultarCobrosResult>("[dbo].[SpConsultarCobros]");
		}

		public partial class SpConsultarCobrosResult
		{
			[Column("id_cobro")    ] public int       Id_cobro     { get; set; }
			[Column("id_casa")     ] public int       Id_casa      { get; set; }
			[Column("nombre_casa") ] public string    Nombre_casa  { get; set; }
			                         public string    Persona      { get; set; }
			                         public int?      Periodo      { get; set; }
			[Column("estado")      ] public string    Estado       { get; set; }
			[Column("monto")       ] public decimal   Monto        { get; set; }
			[Column("fecha_pagada")] public DateTime? Fecha_pagada { get; set; }
		}

		#endregion

		#region SpConsultarServicios

		public static IEnumerable<Servicio> SpConsultarServicios(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.QueryProc<Servicio>("[dbo].[SpConsultarServicios]");
		}

		#endregion

		#region SpConsultarServiciosPorID

		public static IEnumerable<Servicio> SpConsultarServiciosPorID(this PviProyectoFinalDB dataConnection, int? @idServicio)
		{
			var parameters = new []
			{
				new DataParameter("@id_servicio", @idServicio, LinqToDB.DataType.Int32)
			};

			return dataConnection.QueryProc<Servicio>("[dbo].[spConsultarServiciosPorID]", parameters);
		}

		#endregion

		#region SpCreaCasa

		public static int SpCreaCasa(this PviProyectoFinalDB dataConnection, string @NombreCasa, int? @MetrosCuadrados, int? @NumeroHabitaciones, int? @NumeroBanos, int? @idPersona, DateTime? @Fecha)
		{
			var parameters = new []
			{
				new DataParameter("@Nombre_casa",         @NombreCasa,         LinqToDB.DataType.NVarChar)
				{
					Size = 255
				},
				new DataParameter("@Metros_cuadrados",    @MetrosCuadrados,    LinqToDB.DataType.Int32),
				new DataParameter("@Numero_habitaciones", @NumeroHabitaciones, LinqToDB.DataType.Int32),
				new DataParameter("@Numero_banos",        @NumeroBanos,        LinqToDB.DataType.Int32),
				new DataParameter("@id_persona",          @idPersona,          LinqToDB.DataType.Int32),
				new DataParameter("@Fecha",               @Fecha,              LinqToDB.DataType.Date)
			};

			return dataConnection.ExecuteProc("[dbo].[SpCreaCasa]", parameters);
		}

		#endregion

		#region SpCreaServicios

		public static int SpCreaServicios(this PviProyectoFinalDB dataConnection, string @Nombre, string @Descripcion, decimal? @Precio, int? @IdCategoria)
		{
			var parameters = new []
			{
				new DataParameter("@Nombre",       @Nombre,      LinqToDB.DataType.NVarChar)
				{
					Size = 50
				},
				new DataParameter("@Descripcion",  @Descripcion, LinqToDB.DataType.Text)
				{
					Size = 2147483647
				},
				new DataParameter("@Precio",       @Precio,      LinqToDB.DataType.Decimal),
				new DataParameter("@Id_Categoria", @IdCategoria, LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[SpCreaServicios]", parameters);
		}

		#endregion

		#region SpCreatediagram

		public static int SpCreatediagram(this PviProyectoFinalDB dataConnection, string @diagramname, int? @ownerId, int? @version, byte[] @definition)
		{
			var parameters = new []
			{
				new DataParameter("@diagramname", @diagramname, LinqToDB.DataType.NVarChar)
				{
					Size = 128
				},
				new DataParameter("@owner_id",    @ownerId,     LinqToDB.DataType.Int32),
				new DataParameter("@version",     @version,     LinqToDB.DataType.Int32),
				new DataParameter("@definition",  @definition,  LinqToDB.DataType.VarBinary)
				{
					Size = -1
				}
			};

			return dataConnection.ExecuteProc("[dbo].[sp_creatediagram]", parameters);
		}

		#endregion

		#region SpDropdiagram

		public static int SpDropdiagram(this PviProyectoFinalDB dataConnection, string @diagramname, int? @ownerId)
		{
			var parameters = new []
			{
				new DataParameter("@diagramname", @diagramname, LinqToDB.DataType.NVarChar)
				{
					Size = 128
				},
				new DataParameter("@owner_id",    @ownerId,     LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[sp_dropdiagram]", parameters);
		}

		#endregion

		#region SpEliminarCobroMensualidades

		public static int SpEliminarCobroMensualidades(this PviProyectoFinalDB dataConnection, int? @idCobro, int? @idUser)
		{
			var parameters = new []
			{
				new DataParameter("@id_cobro", @idCobro, LinqToDB.DataType.Int32),
				new DataParameter("@id_user",  @idUser,  LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[spEliminarCobroMensualidades]", parameters);
		}

		#endregion

		#region SpHelpdiagramdefinition

		public static IEnumerable<SpHelpdiagramdefinitionResult> SpHelpdiagramdefinition(this PviProyectoFinalDB dataConnection, string @diagramname, int? @ownerId)
		{
			var parameters = new []
			{
				new DataParameter("@diagramname", @diagramname, LinqToDB.DataType.NVarChar)
				{
					Size = 128
				},
				new DataParameter("@owner_id",    @ownerId,     LinqToDB.DataType.Int32)
			};

			return dataConnection.QueryProc<SpHelpdiagramdefinitionResult>("[dbo].[sp_helpdiagramdefinition]", parameters);
		}

		public partial class SpHelpdiagramdefinitionResult
		{
			[Column("version")   ] public int?   Version    { get; set; }
			[Column("definition")] public byte[] Definition { get; set; }
		}

		#endregion

		#region SpHelpdiagrams

		public static IEnumerable<SpHelpdiagramsResult> SpHelpdiagrams(this PviProyectoFinalDB dataConnection, string @diagramname, int? @ownerId)
		{
			var parameters = new []
			{
				new DataParameter("@diagramname", @diagramname, LinqToDB.DataType.NVarChar)
				{
					Size = 128
				},
				new DataParameter("@owner_id",    @ownerId,     LinqToDB.DataType.Int32)
			};

			return dataConnection.QueryProc<SpHelpdiagramsResult>("[dbo].[sp_helpdiagrams]", parameters);
		}

		public partial class SpHelpdiagramsResult
		{
			public string Database { get; set; }
			public string Name     { get; set; }
			public int    ID       { get; set; }
			public string Owner    { get; set; }
			public int    OwnerID  { get; set; }
		}

		#endregion

		#region SpInactivarCasa

		public static int SpInactivarCasa(this PviProyectoFinalDB dataConnection, int? @idCasa)
		{
			var parameters = new []
			{
				new DataParameter("@id_casa", @idCasa, LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[spInactivarCasa]", parameters);
		}

		#endregion

		#region SpInactivarServicio

		public static int SpInactivarServicio(this PviProyectoFinalDB dataConnection, int? @idServicio)
		{
			var parameters = new []
			{
				new DataParameter("@id_servicio", @idServicio, LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[spInactivarServicio]", parameters);
		}

		#endregion

		#region SpModificarCasa

		public static int SpModificarCasa(this PviProyectoFinalDB dataConnection, int? @idCasa, string @NombreCasa, int? @MetrosCuadrados, int? @NumeroHabitaciones, int? @NumeroBanos, int? @idPersona, DateTime? @Fecha)
		{
			var parameters = new []
			{
				new DataParameter("@id_casa",             @idCasa,             LinqToDB.DataType.Int32),
				new DataParameter("@Nombre_casa",         @NombreCasa,         LinqToDB.DataType.NVarChar)
				{
					Size = 255
				},
				new DataParameter("@Metros_cuadrados",    @MetrosCuadrados,    LinqToDB.DataType.Int32),
				new DataParameter("@Numero_habitaciones", @NumeroHabitaciones, LinqToDB.DataType.Int32),
				new DataParameter("@Numero_banos",        @NumeroBanos,        LinqToDB.DataType.Int32),
				new DataParameter("@id_persona",          @idPersona,          LinqToDB.DataType.Int32),
				new DataParameter("@Fecha",               @Fecha,              LinqToDB.DataType.Date)
			};

			return dataConnection.ExecuteProc("[dbo].[spModificarCasa]", parameters);
		}

		#endregion

		#region SpModificarServicios

		public static int SpModificarServicios(this PviProyectoFinalDB dataConnection, int? @idServicio, string @nombre, string @descripcion, decimal? @precio, int? @idCategoria)
		{
			var parameters = new []
			{
				new DataParameter("@id_servicio",  @idServicio,  LinqToDB.DataType.Int32),
				new DataParameter("@nombre",       @nombre,      LinqToDB.DataType.VarChar)
				{
					Size = 50
				},
				new DataParameter("@descripcion",  @descripcion, LinqToDB.DataType.Text)
				{
					Size = 2147483647
				},
				new DataParameter("@precio",       @precio,      LinqToDB.DataType.Decimal),
				new DataParameter("@id_categoria", @idCategoria, LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[spModificarServicios]", parameters);
		}

		#endregion

		#region SpPagarCobroMensualidades

		public static int SpPagarCobroMensualidades(this PviProyectoFinalDB dataConnection, int? @idCobro, int? @idUser)
		{
			var parameters = new []
			{
				new DataParameter("@id_cobro", @idCobro, LinqToDB.DataType.Int32),
				new DataParameter("@id_user",  @idUser,  LinqToDB.DataType.Int32)
			};

			return dataConnection.ExecuteProc("[dbo].[spPagarCobroMensualidades]", parameters);
		}

		#endregion

		#region SpRenamediagram

		public static int SpRenamediagram(this PviProyectoFinalDB dataConnection, string @diagramname, int? @ownerId, string @newDiagramname)
		{
			var parameters = new []
			{
				new DataParameter("@diagramname",     @diagramname,    LinqToDB.DataType.NVarChar)
				{
					Size = 128
				},
				new DataParameter("@owner_id",        @ownerId,        LinqToDB.DataType.Int32),
				new DataParameter("@new_diagramname", @newDiagramname, LinqToDB.DataType.NVarChar)
				{
					Size = 128
				}
			};

			return dataConnection.ExecuteProc("[dbo].[sp_renamediagram]", parameters);
		}

		#endregion

		#region SpUpgraddiagrams

		public static int SpUpgraddiagrams(this PviProyectoFinalDB dataConnection)
		{
			return dataConnection.ExecuteProc("[dbo].[sp_upgraddiagrams]");
		}

		#endregion

		#region SpVerificarPendientesServicio

		public static IEnumerable<SpVerificarPendientesServicioResult> SpVerificarPendientesServicio(this PviProyectoFinalDB dataConnection, int? @idServicio)
		{
			var parameters = new []
			{
				new DataParameter("@id_servicio", @idServicio, LinqToDB.DataType.Int32)
			};

			var ms = dataConnection.MappingSchema;

			return dataConnection.QueryProc(dataReader =>
				new SpVerificarPendientesServicioResult
				{
					Column1 = Converter.ChangeTypeTo<int>(dataReader.GetValue(0), ms),
				},
				"[dbo].[SpVerificarPendientesServicio]", parameters);
		}

		public partial class SpVerificarPendientesServicioResult
		{
			[Column("")] public int Column1 { get; set; }
		}

		#endregion

		#region SpVerificarServiciosCobrosPendientes

		public static IEnumerable<SpVerificarServiciosCobrosPendientesResult> SpVerificarServiciosCobrosPendientes(this PviProyectoFinalDB dataConnection, int? @idCasa)
		{
			var parameters = new []
			{
				new DataParameter("@id_casa", @idCasa, LinqToDB.DataType.Int32)
			};

			var ms = dataConnection.MappingSchema;

			return dataConnection.QueryProc(dataReader =>
				new SpVerificarServiciosCobrosPendientesResult
				{
					Column1 = Converter.ChangeTypeTo<int>(dataReader.GetValue(0), ms),
				},
				"[dbo].[spVerificarServiciosCobrosPendientes]", parameters);
		}

		public partial class SpVerificarServiciosCobrosPendientesResult
		{
			[Column("")] public int Column1 { get; set; }
		}

		#endregion
	}

	public static partial class SqlFunctions
	{
		#region FnDiagramobjects

		[Sql.Function(Name="[dbo].[fn_diagramobjects]", ServerSideOnly=true)]
		public static int? FnDiagramobjects()
		{
			throw new InvalidOperationException();
		}

		#endregion
	}

	public static partial class TableExtensions
	{
		public static Bitacora Find(this ITable<Bitacora> table, int IdBitacora)
		{
			return table.FirstOrDefault(t =>
				t.IdBitacora == IdBitacora);
		}

		public static Casa Find(this ITable<Casa> table, int IdCasa)
		{
			return table.FirstOrDefault(t =>
				t.IdCasa == IdCasa);
		}

		public static Categoria Find(this ITable<Categoria> table, int IdCategoria)
		{
			return table.FirstOrDefault(t =>
				t.IdCategoria == IdCategoria);
		}

		public static Cobro Find(this ITable<Cobro> table, int IdCobro)
		{
			return table.FirstOrDefault(t =>
				t.IdCobro == IdCobro);
		}

		public static Persona Find(this ITable<Persona> table, int IdPersona)
		{
			return table.FirstOrDefault(t =>
				t.IdPersona == IdPersona);
		}

		public static Servicio Find(this ITable<Servicio> table, int IdServicio)
		{
			return table.FirstOrDefault(t =>
				t.IdServicio == IdServicio);
		}
	}
}
