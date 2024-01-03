using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class AccidenteCausas
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccidenteCausas));

        public required Int32 IdAccidenteCausa {get; set;}

        public Int32? IdAccidente {get; set;}

        public Int32? IdCausaAccidente {get; set;}

        public Int32? Indice {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idAccidenteCausa");
            str.Append("\": ");
            str.Append(IdAccidenteCausa);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAccidente");
            str.Append("\": ");
            str.Append(IdAccidente);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCausaAccidente");
            str.Append("\": ");
            str.Append(IdCausaAccidente);
            
            str.Append(", ");

            str.Append('"');
            str.Append("indice");
            str.Append("\": ");
            str.Append(Indice);

            str.Append('}');

            return str.ToString();
        }
    }
}