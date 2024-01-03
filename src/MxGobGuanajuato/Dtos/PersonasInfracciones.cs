using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class PersonasInfracciones
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasInfracciones));

        public required Int32 IdInfraccion {get; set;}

        public Int32 IdPersonaInfraccion {get; set;}

        public String? NumeroLicencia {get; set;}

        public String? Curp {get; set;}

        public String? Rfc {get; set;}

        public String? Nombre {get; set;}

        public String? ApellidoPaterno {get; set;}

        public String? ApellidoMaterno {get; set;}

        public Int32? IdCatTipoPersona {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idPersonaInfraccion");
            str.Append("\": ");
            str.Append(IdPersonaInfraccion);

            str.Append(", ");

            str.Append('"');
            str.Append("idInfraccion");
            str.Append("\": ");
            str.Append(IdInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("numeroLicencia");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumeroLicencia);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("curp");
            str.Append("\": ");
            str.Append('"');
            str.Append(Curp);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("rfc");
            str.Append("\": ");
            str.Append('"');
            str.Append(Rfc);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("nombre");
            str.Append("\": ");
            str.Append('"');
            str.Append(Nombre);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("apellidoPaterno");
            str.Append("\": ");
            str.Append('"');
            str.Append(ApellidoPaterno);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("apellidoMaterno");
            str.Append("\": ");
            str.Append('"');
            str.Append(ApellidoMaterno);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCatTipoPersona");
            str.Append("\": ");
            str.Append(IdCatTipoPersona);
            
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

            str.Append('}');
            
            return str.ToString();
        }
    }
}