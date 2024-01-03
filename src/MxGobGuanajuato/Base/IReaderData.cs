namespace MxGobGuanajuato.Base
{
    public interface IReaderData<T>
    {
        public abstract List<T>? Get(IDictionary<string, object> p);
    }
}