using Gateways.NET.Contracts;
using Gateways.NET.ViewModels;
using Gateways.NET.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateways.NET.Domain.QueryServices
{
    public class PeripheralsQueryService : BaseQueryService<Peripheral>, IPeripheralsQueryService
    {
        public PeripheralsQueryService(IRepository<Peripheral> repository)
            : base(repository)
        {

        }

        public async Task<bool> Exists(int id)
        {
            var source = await _repository.FindByIdAsync(id);
            return source != null;
        }

        public async Task<Peripheral> FindById(int id)
        {
            var source = await _repository.FindByIdAsync(id);
            return source;
        }

        public async Task<IEnumerable<Peripheral>> GetAll(IPaginationModel pagination)
        {
            var source = _repository.Find(x => !x.IsDeleted);
            return await QueryHelper.ApplyPagging(source, pagination.Page, pagination.PageSize).ToArrayAsync();
        }
    }
}
