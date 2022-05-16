using System.Threading.Tasks;

namespace CRM.ServiceCommon.Clients
{
    public class SiaPriceApiClientMock:ISiaPriceClient
    {
        public async Task<decimal> GetSiaPriceAsync()
        {
            return (decimal) 0.1;
        }
    }
}