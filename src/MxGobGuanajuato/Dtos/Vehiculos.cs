
using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class Vehiculos
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Vehiculos));

        public required Int32 IdVehiculo {get; set;}

        public string? Placas {get; set;}

        public string? Serie {get; set;}

        public string? Tarjeta {get; set;}

        public DateTime? VigenciaTarjeta {get; set;}

        public Int32? IdMarcaVehiculo {get; set;}

        public Int32? IdSubmarca {get; set;}

        public Int32? IdTipoVehiculo {get; set;}

        public string? Modelo {get; set;}

        public Int32? IdColor {get; set;}

        public Int32? IdEntidad {get; set;}

        public Int32? IdCatTipoServicio {get; set;}

        public Int32? IdSubtipoServicio {get; set;}

        public string? Propietario {get; set;}

        public string? NumeroEconomico {get; set;}

        public string? PaisManufactura {get; set;}

        public Int32? IdPersona {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public string? Motor {get; set;}

        public Int32? Capacidad {get; set;}

        public string? Poliza {get; set;}

        public Boolean? Carga {get; set;}

        public String? Otros {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idVehiculo");
            str.Append("\": ");
            str.Append(IdVehiculo);

            str.Append(", ");

            str.Append('"');
            str.Append("placas");
            str.Append("\": ");
            str.Append('"');
            str.Append(Placas);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("serie");
            str.Append("\": ");
            str.Append('"');
            str.Append(Serie);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("tarjeta");
            str.Append("\": ");
            str.Append('"');
            str.Append(Tarjeta);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("vigenciaTarjeta");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", VigenciaTarjeta));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idMarcaVehiculo");
            str.Append("\": ");
            str.Append(IdMarcaVehiculo);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idSubmarca");
            str.Append("\": ");
            str.Append(IdSubmarca);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idTipoVehiculo");
            str.Append("\": ");
            str.Append(IdTipoVehiculo);

            str.Append(", ");

            str.Append('"');
            str.Append("modelo");
            str.Append("\": ");
            str.Append('"');
            str.Append(Modelo);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idColor");
            str.Append("\": ");
            str.Append(IdColor);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idEntidad");
            str.Append("\": ");
            str.Append(IdEntidad);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCatTipoServicio");
            str.Append("\": ");
            str.Append(IdCatTipoServicio);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idSubtipoServicio");
            str.Append("\": ");
            str.Append(IdSubtipoServicio);

            str.Append(", ");

            str.Append('"');
            str.Append("propietario");
            str.Append("\": ");
            str.Append('"');
            str.Append(Propietario);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("numeroEconomico");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumeroEconomico);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("paisManufactura");
            str.Append("\": ");
            str.Append('"');
            str.Append(PaisManufactura);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idPersona");
            str.Append("\": ");
            str.Append(IdPersona);
            
            str.Append(", ");

            str.Append('"');
            str.Append("fechaActualizacion");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", FechaActualizacion));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("actualizadoPor");
            str.Append("\": ");
            str.Append(ActualizadoPor);
            
            str.Append(", ");

            str.Append('"');
            str.Append("estatus");
            str.Append("\": ");
            str.Append(Estatus);

            str.Append(", ");

            str.Append('"');
            str.Append("motor");
            str.Append("\": ");
            str.Append('"');
            str.Append(Motor);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("capacidad");
            str.Append("\": ");
            str.Append(Capacidad);

            str.Append(", ");

            str.Append('"');
            str.Append("poliza");
            str.Append("\": ");
            str.Append('"');
            str.Append(Poliza);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("carga");
            str.Append("\": ");
            str.Append(Carga);

            str.Append(", ");

            str.Append('"');
            str.Append("otros");
            str.Append("\": ");
            str.Append('"');
            str.Append(Otros);
            str.Append('"');

            str.Append('}');
            
            return str.ToString();
        }
    }
}