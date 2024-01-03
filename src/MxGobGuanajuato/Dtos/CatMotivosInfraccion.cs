using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class CatMotivosInfraccion
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CatMotivosInfraccion));

        public required Int32 IdCatMotivoInfraccion {get; set;}

        public required String Nombre {get; set;}

        public required Int32 IdSubConcepto {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public required Int32 IdConcepto {get; set;}

        public required Int32 CalificacionMinima {get; set;}

        public required Int32 CalificacionMaxima {get; set;}

        public required String Fundamento {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idCatMotivoInfraccion");
            str.Append("\": ");
            str.Append(IdCatMotivoInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("nombre");
            str.Append("\": ");
            str.Append('"');
            str.Append(Nombre);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idSubConcepto");
            str.Append("\": ");
            str.Append(IdSubConcepto);
            
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
            str.Append("idConcepto");
            str.Append("\": ");
            str.Append(IdConcepto);
            
            str.Append(", ");

            str.Append('"');
            str.Append("calificacionMinima");
            str.Append("\": ");
            str.Append(CalificacionMinima);
            
            str.Append(", ");

            str.Append('"');
            str.Append("calificacionMaxima");
            str.Append("\": ");
            str.Append(CalificacionMaxima);
            
            str.Append(", ");

            str.Append('"');
            str.Append("fundamento");
            str.Append("\": ");
            str.Append('"');
            str.Append(Fundamento);
            str.Append('"');

            str.Append('}');

            return str.ToString();
        }
    }
}