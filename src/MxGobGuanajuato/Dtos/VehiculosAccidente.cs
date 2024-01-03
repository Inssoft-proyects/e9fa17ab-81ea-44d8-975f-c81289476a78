
using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class VehiculosAccidente
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VehiculosAccidente));

        public required Int32 IdVehiculoAccidente {get; set;}

        public Int32 IdVehiculo {get; set;}

        public Int32 IdAccidente {get; set;}

        public Int32? IdPersona {get; set;}

        public Double? MontoVehiculo {get; set;}

        public String? Placa {get; set;}

        public String? Serie {get; set;}

        public Int32? Estatus {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idVehiculoAccidente");
            str.Append("\": ");
            str.Append(IdVehiculoAccidente);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idVehiculo");
            str.Append("\": ");
            str.Append(IdVehiculo);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAccidente");
            str.Append("\": ");
            str.Append(IdAccidente);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idPersona");
            str.Append("\": ");
            str.Append(IdPersona);
            
            str.Append(", ");

            str.Append('"');
            str.Append("montoVehiculo");
            str.Append("\": ");
            str.Append(MontoVehiculo);

            str.Append(", ");

            str.Append('"');
            str.Append("placa");
            str.Append("\": ");
            str.Append('"');
            str.Append(Placa);
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
            str.Append("estatus");
            str.Append("\": ");
            str.Append(Estatus);

            str.Append('}');
            
            return str.ToString();
        }
    }
}