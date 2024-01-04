using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class CatConceptoInfraccion
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CatConceptoInfraccion));

        public required Int32 IdConcepto { get; set;}

        public required String Concepto {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idConcepto");
            str.Append("\": ");
            str.Append(IdConcepto);
            
            str.Append(", ");

            str.Append('"');
            str.Append("concepto");
            str.Append("\": ");
            str.Append('"');
            str.Append(Concepto);
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

            str.Append('}');

            return str.ToString();
        }
    }
}