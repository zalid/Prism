
namespace Prism.Interfaces
{
    public interface IRegionManagerService
    {
        void Register(string regionName, IRegion region);
        IRegion GetRegion(string regionName);
        bool HasRegion(string regionName);
    }
}
