using AutoMapper;
using Gateways.NET.Domain.Commands;
using Gateways.NET.DTOs;
using Gateways.NET.Models;

namespace Gateways.NET.MappingProfiles
{
    public class GatewayMappingProfile : Profile
    {
        public GatewayMappingProfile()
        {
            CreateMap<GatewayViewModel, CreateGatewayCommand>();
            CreateMap<GatewayViewModel, UpdateGatewayCommand>().ForMember(x => x.Id, op => op.Ignore());
            CreateMap<PeripheralViewModel, AddPeripheralToGatewayCommand>().ForMember(x => x.GatewayId, op => op.Ignore());
            CreateMap<Gateway, FullGatewayViewModel>();
        }
    }
}
