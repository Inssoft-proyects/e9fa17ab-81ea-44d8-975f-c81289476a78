
using System.Text;
using log4net;

namespace MxGobGuanajuato.Dtos
{
    public sealed class Accidentes
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Accidentes));

        public required Int32 IdAccidente {get; set;}

        public DateTime? Fecha {get; set;}

        public TimeSpan? Hora {get; set;}

        public Int32? IdMunicipio {get; set;}

        public Int32? IdCarretera {get; set;}

        public Int32? IdTramo {get; set;}

        public String? Kilometro {get; set;}

        public Int32? EstatusReporte {get; set;}

        public Int32? IdClasificacionAccidente {get; set;}

        public Int32? IdCausaAccidente {get; set;}

        public String? DescripcionCausas {get; set;}

        public Int32? IdFactorAccidente {get; set;}

        public Int32? IdFactorOpcionAccidente {get; set;}

        public DateTime? FechaActualizacion {get; set;}

        public Int32? ActualizadoPor {get; set;}

        public Int32? Estatus {get; set;}

        public Double? MontoCamino {get; set;}

        public Double? MontoCarga {get; set;}

        public Double? MontoPropietarios {get; set;}

        public Double? MontoOtros {get; set;}

        public Double? MontoVehiculo {get; set;}

        public Int32? IdElabora {get; set;}

        public Int32? IdSupervisa {get; set;}

        public Int32? IdAutoriza {get; set;}

        public Int32? IdElaboraConsignacion {get; set;}

        public Double? Latitud {get; set;}

        public Double? Longitud {get; set;}

        public Int32? IdCiudad {get; set;}

        public Int32? IdCertificado {get; set;}

        public String? EntregaOtros {get; set;}

        public String? EntregaObjetos {get; set;}

        public String? ConsignacionHechos {get; set;}

        public String? NumeroOficio {get; set;}

        public Int32? IdAgenciaMinisterio {get; set;}

        public String? RecibeMinisterio {get; set;}

        public Int32? IdAutoridadEntrega {get; set;}

        public Int32? IdAutoridadDisposicion {get; set;}

        public Int32? Armas {get; set;}

        public Int32? Drogas {get; set;}

        public Int32? Valores {get; set;}

        public Int32? Prendas {get; set;}

        public Int32? Otros {get; set;}

        public Int32? IdEstatusReporte {get; set;}

        public String? NumeroReporte {get; set;}

        public Int32? IdOficinaDelegacion {get; set;}

        public String? ArmasTexto {get; set;}

        public String? DrogasTexto {get; set;}

        public String? ValoresTexto {get; set;}

        public String? PrendasTexto {get; set;}

        public String? OtrosTexto {get; set;}

        public Int32? Convenio {get; set;}

        public String? ObservacionesConvenio {get; set;}

        public Int32? IdEntidadCompetencia {get; set;}

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
            str.Append("fecha");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:dd/MM/yyyy HH:mm:ss}", Fecha));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("hora");
            str.Append("\": ");
            str.Append('"');

            try {
                str.Append(String.Format("{0:g}", Hora));
            } catch(ArgumentNullException ex) {
                log.Error(ex);
            }

            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idMunicipio");
            str.Append("\": ");
            str.Append(IdMunicipio);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCarretera");
            str.Append("\": ");
            str.Append(IdCarretera);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idTramo");
            str.Append("\": ");
            str.Append(IdTramo);

            str.Append(", ");

            str.Append('"');
            str.Append("kilometro");
            str.Append("\": ");
            str.Append('"');
            str.Append(Kilometro);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("estatusReporte");
            str.Append("\": ");
            str.Append(EstatusReporte);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idClasificacionAccidente");
            str.Append("\": ");
            str.Append(IdClasificacionAccidente);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCausaAccidente");
            str.Append("\": ");
            str.Append(IdCausaAccidente);

            str.Append(", ");

            str.Append('"');
            str.Append("descripcionCausas");
            str.Append("\": ");
            str.Append('"');
            str.Append(DescripcionCausas);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idFactorAccidente");
            str.Append("\": ");
            str.Append(IdFactorAccidente);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idFactorOpcionAccidente");
            str.Append("\": ");
            str.Append(IdFactorOpcionAccidente);
            
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
            str.Append("montoCamino");
            str.Append("\": ");
            str.Append(MontoCamino);
            
            str.Append(", ");

            str.Append('"');
            str.Append("montoCarga");
            str.Append("\": ");
            str.Append(MontoCarga);
            
            str.Append(", ");

            str.Append('"');
            str.Append("montoPropietarios");
            str.Append("\": ");
            str.Append(MontoPropietarios);
            
            str.Append(", ");

            str.Append('"');
            str.Append("montoOtros");
            str.Append("\": ");
            str.Append(MontoOtros);
            
            str.Append(", ");

            str.Append('"');
            str.Append("montoVehiculo");
            str.Append("\": ");
            str.Append(MontoVehiculo);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idElabora");
            str.Append("\": ");
            str.Append(IdElabora);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idSupervisa");
            str.Append("\": ");
            str.Append(IdSupervisa);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAutoriza");
            str.Append("\": ");
            str.Append(IdAutoriza);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idElaboraConsignacion");
            str.Append("\": ");
            str.Append(IdElaboraConsignacion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("latitud");
            str.Append("\": ");
            str.Append(Latitud);
            
            str.Append(", ");

            str.Append('"');
            str.Append("longitud");
            str.Append("\": ");
            str.Append(Longitud);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCiudad");
            str.Append("\": ");
            str.Append(IdCiudad);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idCertificado");
            str.Append("\": ");
            str.Append(IdCertificado);

            str.Append(", ");

            str.Append('"');
            str.Append("entregaOtros");
            str.Append("\": ");
            str.Append('"');
            str.Append(EntregaOtros);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("entregaObjetos");
            str.Append("\": ");
            str.Append('"');
            str.Append(EntregaObjetos);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("consignacionHechos");
            str.Append("\": ");
            str.Append('"');
            str.Append(ConsignacionHechos);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("numeroOficio");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumeroOficio);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAgenciaMinisterio");
            str.Append("\": ");
            str.Append(IdAgenciaMinisterio);

            str.Append(", ");

            str.Append('"');
            str.Append("recibeMinisterio");
            str.Append("\": ");
            str.Append('"');
            str.Append(RecibeMinisterio);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAutoridadEntrega");
            str.Append("\": ");
            str.Append(IdAutoridadEntrega);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idAutoridadDisposicion");
            str.Append("\": ");
            str.Append(IdAutoridadDisposicion);
            
            str.Append(", ");

            str.Append('"');
            str.Append("armas");
            str.Append("\": ");
            str.Append(Armas);
            
            str.Append(", ");

            str.Append('"');
            str.Append("drogas");
            str.Append("\": ");
            str.Append(Drogas);
            
            str.Append(", ");

            str.Append('"');
            str.Append("valores");
            str.Append("\": ");
            str.Append(Valores);
            
            str.Append(", ");

            str.Append('"');
            str.Append("prendas");
            str.Append("\": ");
            str.Append(Prendas);
            
            str.Append(", ");

            str.Append('"');
            str.Append("otros");
            str.Append("\": ");
            str.Append(Otros);
            
            str.Append(", ");

            str.Append('"');
            str.Append("idEstatusReporte");
            str.Append("\": ");
            str.Append(IdEstatusReporte);

            str.Append(", ");

            str.Append('"');
            str.Append("numeroReporte");
            str.Append("\": ");
            str.Append('"');
            str.Append(NumeroReporte);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idOficinaDelegacion");
            str.Append("\": ");
            str.Append(IdOficinaDelegacion);

            str.Append(", ");

            str.Append('"');
            str.Append("armasTexto");
            str.Append("\": ");
            str.Append('"');
            str.Append(ArmasTexto);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("drogasTexto");
            str.Append("\": ");
            str.Append('"');
            str.Append(DrogasTexto);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("valoresTexto");
            str.Append("\": ");
            str.Append('"');
            str.Append(ValoresTexto);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("prendasTexto");
            str.Append("\": ");
            str.Append('"');
            str.Append(PrendasTexto);
            str.Append('"');

            str.Append(", ");

            str.Append('"');
            str.Append("otrosTexto");
            str.Append("\": ");
            str.Append('"');
            str.Append(OtrosTexto);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("convenio");
            str.Append("\": ");
            str.Append(Convenio);

            str.Append(", ");

            str.Append('"');
            str.Append("observacionesConvenio");
            str.Append("\": ");
            str.Append('"');
            str.Append(ObservacionesConvenio);
            str.Append('"');
            
            str.Append(", ");

            str.Append('"');
            str.Append("idEntidadCompetencia");
            str.Append("\": ");
            str.Append(IdEntidadCompetencia);

            str.Append('}');

            return str.ToString();
        }
    }
}