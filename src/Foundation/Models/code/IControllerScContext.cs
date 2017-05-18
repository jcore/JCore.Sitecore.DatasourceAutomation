using Glass.Mapper.Sc;

namespace Car.Foundation.Models
{
    public interface IControllerScContext : ISitecoreContext
    {
        T GetDataSource<T>() where T : class;
        T GetRenderingParameters<T>() where T : class;
    }
}
