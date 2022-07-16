using AutoMapper;

namespace HomeTownPickEm.Abstract.Interfaces
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }
    }
}