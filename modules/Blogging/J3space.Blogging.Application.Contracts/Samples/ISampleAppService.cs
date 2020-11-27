using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace J3space.Blogging.Samples
{
    public interface ISampleAppService : IApplicationService
    {
        Task<SampleDto> GetAsync();

        Task<SampleDto> GetAuthorizedAsync();
    }
}