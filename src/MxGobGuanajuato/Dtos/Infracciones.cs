using log4net;
using System.Text;

namespace MxGobGuanajuato.Dtos
{
    public sealed class Infracciones
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Infracciones));

        public required Int32 IdInfraccion {get; set;}

        public Int32? IdOficial {get; set;}

        public Int32? IdDependencia {get; set;}

        public Int32? IdDelegacion {get; set;}

        public Int32? IdVehiculo {get; set;}

        public Int32? IdAplicacion {get; set;}

        public Int32? IdGarantia {get; set;}

        public Int32? IdEstatusInfraccion {get; set;}

        public Int32? IdMunicipio {get; set;}

        public Int32? IdTramo {get; set;}

        public Int32? IdCarretera {get; set;}

        public Int32? IdPersona {get; set;}

        public Int32? IdPersonaInfraccion {get; set;}

        public String? PlacasVehiculo {get; set;}

        public String? FolioInfraccion {get; set;}

        public DateTime? FechaInfraccion {get; set;}

        public String? KmCarretera {get; set;}

        public String? Observaciones {get; set;}

        public String? LugarCalle {get; set;}

        public String? LugarNumero {get; set;}

        public String? LugarColonia {get; set;}

        public String? LugarEntreCalle {get; set;}

        public Boolean? InfraccionCortesia {get; set;}

        public String? NumTarjetaCirculacion {get; set;}

        public String? OficioRevocacion {get; set;}

        public Int32? EstatusProceso {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public String? ReciboPago {get; set;}

        public Double? Monto {get; set;}

        public Double? MontoPagado {get; set;}

        public DateTime? FechaPago {get; set;}

        public String? LugarPago {get; set;}

        public  Int32? IdEstatusEnvio {get; set;}

        public String? OficioEnvio {get; set;}

        public DateTime? FechaEnvio {get; set;}

        public Int32? IdOficinaRenta {get; set;}

        public Byte[]? Inventario {get; set;}

        public String? Partner {get; set;}

        public String? Cuenta {get; set;}

        public String? Objeto {get; set;}

        public String? Documento {get; set;}

        public Int32? Transito {get; set;}
        
        public override string? ToString()
        {
            StringBuilder str = new();

            str.Append('{');

            str.Append('"');
            str.Append("idInfraccion");
            str.Append("\": ");
            str.Append(IdInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idOficial");
            str.Append("\": ");
            str.Append(IdOficial);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idDependencia");
            str.Append("\": ");
            str.Append(IdDependencia);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idDelegacion");
            str.Append("\": ");
            str.Append(IdDelegacion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idVehiculo");
            str.Append("\": ");
            str.Append(IdVehiculo);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAplicacion");
            str.Append("\": ");
            str.Append(IdAplicacion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idGarantia");
            str.Append("\": ");
            str.Append(IdGarantia);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idEstatusInfraccion");
            str.Append("\": ");
            str.Append(IdEstatusInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idMunicipio");
            str.Append("\": ");
            str.Append(IdMunicipio);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idTramo");
            str.Append("\": ");
            str.Append(IdTramo);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCarretera");
            str.Append("\": ");
            str.Append(IdCarretera);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idPersona");
            str.Append("\": ");
            str.Append(IdPersona);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idPersonaInfraccion");
            str.Append("\": ");
            str.Append(IdPersonaInfraccion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("placasVehiculo");
            str.Append("\": ");
            str.Append('"');
            str.Append(PlacasVehiculo);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("folioInfraccion");
            str.Append("\": ");
            str.Append('"');
            str.Append(FolioInfraccion);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("fechaInfraccion");
            str.Append("\": ");
            str.Append('"');

            try{
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", FechaInfraccion));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("kmCarretera");
            str.Append("\": ");
            str.Append('"');
            str.Append(KmCarretera);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("observaciones");
            str.Append("\": ");
            str.Append('"');
            str.Append(Observaciones);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("lugarCalle");
            str.Append("\": ");
            str.Append('"');
            str.Append(LugarCalle);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("lugarNumero");
            str.Append("\": ");
            str.Append('"');
            str.Append(LugarNumero);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("lugarColonia");
            str.Append("\": ");
            str.Append('"');
            str.Append(LugarColonia);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("lugarEntreCalle");
            str.Append("\": ");
            str.Append('"');
            str.Append(LugarEntreCalle);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("infraccionCortesia");
            str.Append("\": ");
            str.Append('"');
            str.Append(InfraccionCortesia);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("numTarjetaCirculacion");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumTarjetaCirculacion);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("oficioRevocacion");
            str.Append("\": ");
            str.Append('"');
            str.Append(OficioRevocacion);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("estatusProceso");
            str.Append("\": ");
            str.Append(EstatusProceso);

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
            str.Append("reciboPago");
            str.Append("\": ");
            str.Append('"');
            str.Append(ReciboPago);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("monto");
            str.Append("\": ");
            str.Append(Monto);

            str.Append(", ");

            str.Append('"');
            str.Append("montoPagado");
            str.Append("\": ");
            str.Append(MontoPagado);

            str.Append(", ");

            str.Append('"');
            str.Append("fechaPago");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", FechaPago));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("lugarPago");
            str.Append("\": ");
            str.Append('"');
            str.Append(LugarPago);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idEstatusEnvio");
            str.Append("\": ");
            str.Append(IdEstatusEnvio);

            str.Append(", ");

            str.Append('"');
            str.Append("oficioEnvio");
            str.Append("\": ");
            str.Append('"');
            str.Append(OficioEnvio);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("fechaEnvio");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", FechaEnvio));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idOficinaRenta");
            str.Append("\": ");
            str.Append(IdOficinaRenta);

            str.Append(", ");

            str.Append('"');
            str.Append("inventario");
            str.Append("\": ");
            str.Append('"');
            
            if(Inventario != null && Inventario.Length > 0)
                str.Append(Convert.ToBase64String(Inventario));

            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("partner");
            str.Append("\": ");
            str.Append('"');
            str.Append(Partner);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("cuenta");
            str.Append("\": ");
            str.Append('"');
            str.Append(Cuenta);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("objeto");
            str.Append("\": ");
            str.Append('"');
            str.Append(Objeto);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("documento");
            str.Append("\": ");
            str.Append('"');
            str.Append(Documento);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("transito");
            str.Append("\": ");
            str.Append(Transito);

            str.Append('}');

            return str.ToString();
        }
    }
}