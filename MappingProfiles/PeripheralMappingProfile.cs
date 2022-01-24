using AutoMapper;
using Gateways.NET.Domain.Commands;
using Gateways.NET.DTOs;
using Gateways.NET.Models;

namespace Gateways.NET.MappingProfiles
{
    public class PeripheralMappingProfile : Profile
    {
        public PeripheralMappingProfile()
        {
            CreateMap<PeripheralViewModel, CreatePeripheralCommand>();
            CreateMap<PeripheralViewModel, UpdatePeripheralCommand>().ForMember(x => x.Id, op => op.Ignore());
            CreateMap<Peripheral, FullPeripheralViewModel>();
            CreateMap<AddPeripheralToGatewayCommand, CreatePeripheralCommand>();
            CreateMap<CreatePeripheralCommand,Peripheral>().ForMember(x => x.Id, op => op.Ignore()).ForMember(x=>x.GatewayId, op=>op.Ignore());
        }
    }
}
