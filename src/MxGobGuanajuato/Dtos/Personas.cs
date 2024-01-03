
using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class Personas
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Personas));

        public required Int32 IdPersona {get; set;}

        public String? NumeroLicencia {get; set;}

        public String? CURP {get; set;}

        public String? RFC {get; set;}

        public String? Nombre {get; set;}

        public String? ApellidoPaterno {get; set;}

        public String? ApellidoMaterno {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public Int32? IdCatTipoPersona {get; set;}

        public Int32? IdGenero {get; set;}

        public DateTime? FechaNacimiento {get; set;}

        public Int32? IdTipoLicencia {get; set;}

        public DateTime? VigenciaLicencia {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();
            
            str.Append('{');

            str.Append('"');
            str.Append("idPersona");
            str.Append("\": ");
            str.Append(IdPersona);

            str.Append(", ");

            str.Append('"');
            str.Append("numeroLicencia");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumeroLicencia);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("CURP");
            str.Append("\": ");
            str.Append('"');
            str.Append(CURP);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("RFC");
            str.Append("\": ");
            str.Append('"');
            str.Append(RFC);
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
            str.Append("idCatTipoPersona");
            str.Append("\": ");
            str.Append(IdCatTipoPersona);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idGenero");
            str.Append("\": ");
            str.Append(IdGenero);
            
            str.Append(", ");

            str.Append('"');
            str.Append("fechaNacimiento");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", FechaNacimiento));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idTipoLicencia");
            str.Append("\": ");
            str.Append(IdTipoLicencia);
            
            str.Append(", ");

            str.Append('"');
            str.Append("vigenciaLicencia");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", VigenciaLicencia));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');

            str.Append('}');
            
            return str.ToString();
        }
    }
}