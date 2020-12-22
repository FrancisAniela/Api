using System.Threading.Tasks;

namespace WebApi.Core.Services.ClientApplications
{
    public interface IClientApplicationService
    {
        Task<ClientApplicationDto> Authenticate(string username, string password);
        ClientApplicationDto GetById(int id);
    }
}
