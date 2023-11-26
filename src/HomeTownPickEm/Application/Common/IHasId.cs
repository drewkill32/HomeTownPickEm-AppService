namespace HomeTownPickEm.Application.Common
{
    public interface IHasId: IHasId<int>
    {
    }
    
    public interface IHasId<out T>
    {
        public T Id { get; }
    }
}