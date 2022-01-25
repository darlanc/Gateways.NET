using AutoMapper;
using Gateways.NET.Domain.Commands;
using Gateways.NET.CoreViewModels;
using Gateways.NET.Models;

namespace Gateways.NET.MappingProfiles
{
    public class GatewayMappingProfile : Profile
    {
        public GatewayMappingProfile()
        {
            CreateMap<GatewayViewModel, CreateGatewayCommand>();
            CreateMap<CreateGatewayCommand, Gateway>().ForMember(x => x.Id, op => op.Ignore()).ForMember(x => x.Peripherals, op => op.Ignore());
            CreateMap<GatewayViewModel, UpdateGatewayCommand>().ForMember(x => x.Id, op => op.Ignore());
            CreateMap<PeripheralViewModel, AddPeripheralToGatewayCommand>().ForMember(x => x.GatewayId, op => op.Ignore());
            CreateMap<Gateway, FullGatewayViewModel>();
        }
    }
}
