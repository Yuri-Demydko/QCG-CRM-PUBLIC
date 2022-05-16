using System.Threading.Tasks;

namespace CRM.ServiceCommon.Clients
{
    public interface ISiaPriceClient
    {
        public Task<decimal> GetSiaPriceAsync();
    }
}