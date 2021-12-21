using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TabangayTa.Models;

namespace TabangayTa.Services.ResourcePins
{
    public interface IResourcePinService
    {
        Task<ResourcePinModel> GetResourcePins(int limit, int cursor = 0, bool descending = false);
    }
}
