using System.Threading.Tasks;
using TabangayTa.Models;
using TabangayTa.Models.Request;
using TabangayTa.Models.Resp;

namespace TabangayTa.Services.ResourcePins
{
    public interface IResourcePinService
    {
        Task<ResourcePinModel> GetResourcePins(int limit, int cursor = 0, bool descending = false);
        Task<ResourceResponseModel> AddResourcePins(ResourceRequestModel resource);
    }
}
