using Aimrank.Cluster.Core.Entities;
using Aimrank.Cluster.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aimrank.Cluster.Core.Clients
{
    public class PodClient : IPodClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPodRepository _podRepository;

        public PodClient(IHttpClientFactory httpClientFactory, IPodRepository podRepository)
        {
            _httpClientFactory = httpClientFactory;
            _podRepository = podRepository;
        }

        public async Task<IEnumerable<Pod>> GetInactivePodsAsync()
        {
            using var httpClient = _httpClientFactory.CreateClient();
            
            var inactivePods = new List<Pod>();
            
            var pods = await _podRepository.BrowseAsync();

            foreach (var pod in pods)
            {
                if (await IsPodAliveAsync(httpClient, pod) is false)
                {
                    inactivePods.Add(pod);
                }
            }

            return inactivePods;
        }

        public async Task StopServerAsync(Server server)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            
            await httpClient.DeleteAsync($"http://{server.Pod.IpAddress}/server/{server.Id}");
        }

        public async Task<string> StartServerAsync(Server server, string map, IEnumerable<string> whitelist)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsJsonAsync($"http://{server.Pod.IpAddress}/server", new
            {
                server.Id,
                SteamToken = server.SteamToken.Token,
                Map = map,
                Whitelist = whitelist
            });

            return response.IsSuccessStatusCode
                ? response.Headers.FirstOrDefault(x => x.Key == "x-resource").Value.FirstOrDefault()
                : null;
        }
        
        private async Task<bool> IsPodAliveAsync(HttpClient httpClient, Pod pod)
        {
            try
            {
                var result = await httpClient.GetAsync($"http://{pod.IpAddress}");
                return result.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}