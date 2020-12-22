using AutoMapper;
using System.Threading.Tasks;
using WebApi.Core.Models;
using WebApi.Core.Repositories;

namespace WebApi.Core.Services.ClientApplications
{
    public class ClientApplicationService : IClientApplicationService
    {

        IWebApiRepository<ClientApplication> _clientApplicationRepository;
        IMapper _mapper;
        public ClientApplicationService(IMapper mapper
            , IWebApiRepository<ClientApplication> clientApplicationRepository)
        {
            _mapper = mapper;
            _clientApplicationRepository = clientApplicationRepository;
        }

        public async Task<ClientApplicationDto> Authenticate(string clientName, string clientSecret)
        {
            var clientApplication = await Task.Run(() => _clientApplicationRepository.FirstOrDefault(x => x.ClientName == clientName && x.ClientSecret == clientSecret));
            return _mapper.Map<ClientApplicationDto>(clientApplication);
        }

        public ClientApplicationDto GetById(int id)
        {
            var clientApplication = _clientApplicationRepository.FirstOrDefault(x => x.Id == id);
            return _mapper.Map<ClientApplicationDto>(clientApplication);
        }
    }
}
