using Microsoft.Extensions.Localization;

namespace CoindeskApi.Resources
{
    public class SharedResource
    {
        private readonly IStringLocalizer<SharedResource> _sharelocalizer;
        public SharedResource(IStringLocalizer<SharedResource> sharelocalizer)
        {
            _sharelocalizer = sharelocalizer; 
        }

    }
}
