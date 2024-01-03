namespace MxGobGuanajuato.Base
{
    public interface IProcessorData<I, O>
    {
        public abstract O Process(I fd);
    }
}