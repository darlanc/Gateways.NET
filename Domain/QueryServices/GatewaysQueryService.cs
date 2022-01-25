using Gateways.NET.Contracts;
using Gateways.NET.ViewModels;
using Gateways.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.QueryServices
{
    public class GatewaysQueryService : BaseQueryService<Gateway>, IGatewaysQueryService
    {
        public GatewaysQueryService(IRepository<Gateway> repository)
            : base(repository)
        {

        }

        public async Task<bool> Exists(int id)
        {
            var source = await _repository.FindFirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            return source != null;
        }

        public async Task<Gateway> FindById(int id)
        {
            var source = await _repository.FindFirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id, nameof(Gateway.Peripherals));
            return source;
        }

        public async Task<IEnumerable<Gateway>> GetAll(IPaginationModel pagination)
        {
            var source = _repository.Find(x => !x.IsDeleted, nameof(Gateway.Peripherals));
            return await QueryHelper.ApplyPagging(source, pagination.Page, pagination.PageSize).ToArrayAsync();
        }

        public async Task<IEnumerable<Peripheral>> GetPeripherals(int id)
        {
            var source = await _repository.FindFirstOrDefaultAsync(x=>!x.IsDeleted && x.Id == id, nameof(Gateway.Peripherals));
            return source?.Peripherals;
        }
    }
}
