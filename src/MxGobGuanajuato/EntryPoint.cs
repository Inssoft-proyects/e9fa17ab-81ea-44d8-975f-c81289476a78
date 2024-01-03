
using MxGobGuanajuato.Base;
using Spring.Context;
using Spring.Context.Support;

namespace MxGobGuanajuato
{
    public sealed class EntryPoint
    {
        public static void Main(String[] args)
        {
            using IApplicationContext ctx = ContextRegistry.GetContext();
            
            IDictionary<string, object> p = new Dictionary<string, object>(){
                {"modalidad", args[0]}
            };

            IFlowData cmif = (IFlowData)ctx.GetObject("catMotivosInfraccionFlow");

            cmif.Inside(p);

            IFlowData inff = (IFlowData)ctx.GetObject("infraccionesFlow");

            inff.Inside(p);

            IFlowData mif = (IFlowData)ctx.GetObject("motivosInfraccionFlow");
            
            mif.Inside(p);

            IFlowData pif = (IFlowData)ctx.GetObject("personasInfraccionesFlow");
            
            pif.Inside(p);

            IFlowData pef = (IFlowData)ctx.GetObject("personasFlow");
            
            pef.Inside(p);

            IFlowData pdf = (IFlowData)ctx.GetObject("personasDireccionesFlow");
            
            pdf.Inside(p);

            IFlowData vf = (IFlowData)ctx.GetObject("vehiculosFlow");
            
            vf.Inside(p);

            IFlowData accf = (IFlowData)ctx.GetObject("accidentesFlow");
            
            accf.Inside(p);
            
            IFlowData accaf = (IFlowData)ctx.GetObject("accidenteCausasFlow");
            
            accaf.Inside(p);

            IFlowData vaccf = (IFlowData)ctx.GetObject("vehiculosAccidenteFlow");
            
            vaccf.Inside(p);

            IFlowData iaccf = (IFlowData)ctx.GetObject("involucradosAccidenteFlow");
            
            iaccf.Inside(p);
        }
    }
}