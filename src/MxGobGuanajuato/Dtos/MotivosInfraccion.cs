using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class MotivosInfraccion
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MotivosInfraccion));

        public required Int32 IdMotivoInfraccion {get; set;}

        public required Int32 CalificacionMinima {get; set;}

        public required Int32 CalificacionMaxima {get; set;}

        public Int32? Calificacion {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public Int32? IdCatMotivosInfraccion {get; set;}

        public Int32? IdInfraccion {get; set;}

        public Int32? IdConcepto {get; set;}

        public Int32? IdSubConcepto {get; set;}

        public Int32? Prioridad {get; set;}

        public override string? ToString()
        {
            StringBuilder str = new();
            
            str.Append('{');

            str.Append('"');
            str.Append("idMotivoInfraccion");
            str.Append("\": ");
            str.Append(IdMotivoInfraccion);
            
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
            str.Append("calificacion");
            str.Append("\": ");
            str.Append(Calificacion);
            
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
            str.Append("idCatMotivosInfraccion");
            str.Append("\": ");
            str.Append(IdCatMotivosInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idInfraccion");
            str.Append("\": ");
            str.Append(IdInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idConcepto");
            str.Append("\": ");
            str.Append(IdConcepto);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idSubConcepto");
            str.Append("\": ");
            str.Append(IdSubConcepto);
            
            str.Append(", ");

            str.Append('"');
            str.Append("prioridad");
            str.Append("\": ");
            str.Append(Prioridad);

            str.Append('}');

            return str.ToString();
        }
    }
}