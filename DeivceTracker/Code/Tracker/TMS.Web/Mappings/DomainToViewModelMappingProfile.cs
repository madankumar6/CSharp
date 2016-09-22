using AutoMapper;
using TMS.Model;
using TMS.Web.Models.ViewModels;

namespace TMS.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            CreateMap<Address, AddressViewModel>();
            CreateMap<User, UserViewModel>().ForSourceMember(src => src.Password, dest => dest.Ignore());
            CreateMap<Admin, AdminViewModel>().ForSourceMember(src => src.Password, dest => dest.Ignore());
            CreateMap<Distributor, DistributorViewModel>().ForSourceMember(src => src.Password, dest => dest.Ignore());
            CreateMap<Dealer, DealerViewModel>().ForSourceMember(src => src.Password, dest => dest.Ignore()).ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.Address));
            CreateMap<Customer, CustomerViewModel>().ForSourceMember(src => src.Password, dest => dest.Ignore());
            CreateMap<Vehicle, VehicleViewModel>();
            CreateMap<Device, DeviceViewModel>();
            CreateMap<DeviceModels, DeviceModelViewModel>();
            CreateMap<DeviceType, DeviceTypeViewModel>();
        }
    }
}