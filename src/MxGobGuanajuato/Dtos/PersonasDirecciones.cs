
using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class PersonasDirecciones
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasDirecciones));

        public required Int32 IdPersonasDirecciones {get; set;}

        public Int32? IdEntidad {get; set;}

        public Int32? IdMunicipio {get; set;}

        public String? CodigoPostal {get; set;}

        public String? Colonia {get; set;}

        public String? Calle {get; set;}

        public String? Numero {get; set;}

        public Int64? Telefono {get; set;}

        public String? Correo {get; set;}

        public Int32? IdPersona {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? Estatus {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idPersonasDirecciones");
            str.Append("\": ");
            str.Append(IdPersonasDirecciones);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idEntidad");
            str.Append("\": ");
            str.Append(IdEntidad);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idMunicipio");
            str.Append("\": ");
            str.Append(IdMunicipio);

            str.Append(", ");

            str.Append('"');
            str.Append("codigoPostal");
            str.Append("\": ");
            str.Append('"');
            str.Append(CodigoPostal);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("colonia");
            str.Append("\": ");
            str.Append('"');
            str.Append(Colonia);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("calle");
            str.Append("\": ");
            str.Append('"');
            str.Append(Calle);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("numero");
            str.Append("\": ");
            str.Append('"');
            str.Append(Numero);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("telefono");
            str.Append("\": ");
            str.Append(Telefono);

            str.Append(", ");

            str.Append('"');
            str.Append("correo");
            str.Append("\": ");
            str.Append('"');
            str.Append(Correo);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idPersona");
            str.Append("\": ");
            str.Append(IdPersona);
            
            str.Append(", ");

            str.Append('"');
            str.Append("actualizadoPor");
            str.Append("\": ");
            str.Append(ActualizadoPor);
            
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
            str.Append("estatus");
            str.Append("\": ");
            str.Append(Estatus);

            str.Append('}');
            
            return str.ToString();
        }
    }
}