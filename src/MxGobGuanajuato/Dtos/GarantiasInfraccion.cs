
using System.Text;
using log4net;

namespace MxGobGuanajuato
{
    public sealed class GarantiasInfraccion
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GarantiasInfraccion));

        public Int32 IdGarantia {set; get;}

        public Int32 IdCatGarantia {set; get;}

        public Int32 IdTipoPlaca {set; get;}

        public Int32 IdTipoLicencia {set; get;}

        public required Int32 IdInfraccion {set; get;}

        public String? NumPlaca {set; get;}

        public String? NumLicencia {set; get;}

        public String? VehiculoDocumento {set; get;}

        public DateTime? FechaActualizacion {set; get;}

        public Int32? ActualizadoPor {set; get;}

        public Int32? Estatus {set; get;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idGarantia");
            str.Append("\": ");
            str.Append(IdGarantia);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCatGarantia");
            str.Append("\": ");
            str.Append(IdCatGarantia);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idTipoPlaca");
            str.Append("\": ");
            str.Append(IdTipoPlaca);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idTipoLicencia");
            str.Append("\": ");
            str.Append(IdTipoLicencia);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idInfraccion");
            str.Append("\": ");
            str.Append(IdInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("numPlaca");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumPlaca);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("numLicencia");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumLicencia);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("vehiculoDocumento");
            str.Append("\": ");
            str.Append('"');
            str.Append(VehiculoDocumento);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("fechaActualizacion");
            str.Append("\": ");
            str.Append('"');

            try{
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