using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class InvolucradosAccidente
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InvolucradosAccidente));

        public required Int32 IdAccidente {get; set;}

        public required Int32 IdPersona {get; set;}

        public required Int32 IdVehiculo {get; set;}

        public Int32? IdTipoInvolucrado {get; set;}

        public Int32? IdEstadoVictima {get; set;}

        public Int32? IdHospital {get; set;}

        public Int32? IdInstitucionTraslado {get; set;}

        public Int32? IdAsiento {get; set;}

        public Int32? IdCinturon {get; set;}

        public DateTime? FechaIngreso {get; set;}

        public TimeSpan? HoraIngreso {get; set;}

        public Int32? Estatus {get; set;}

        public override string ToString()
        {
            StringBuilder str = new();

            str.Append('{');

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
            str.Append("idVehiculo");
            str.Append("\": ");
            str.Append(IdVehiculo);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idTipoInvolucrado");
            str.Append("\": ");
            str.Append(IdTipoInvolucrado);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idEstadoVictima");
            str.Append("\": ");
            str.Append(IdEstadoVictima);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idHospital");
            str.Append("\": ");
            str.Append(IdHospital);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idInstitucionTraslado");
            str.Append("\": ");
            str.Append(IdInstitucionTraslado);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAsiento");
            str.Append("\": ");
            str.Append(IdAsiento);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCinturon");
            str.Append("\": ");
            str.Append(IdCinturon);

            str.Append(", ");

            str.Append('"');
            str.Append("fechaIngreso");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", FechaIngreso));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("horaIngreso");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:g}", HoraIngreso));
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