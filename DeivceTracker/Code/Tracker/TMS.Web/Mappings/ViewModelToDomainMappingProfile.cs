using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TMS.Model;
using TMS.Web.Models.ViewModels;

namespace TMS.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            CreateMap<AdminViewModel, Admin>();
            CreateMap<DealerViewModel, Dealer>();
            CreateMap<CustomerViewModel, Customer>();
            CreateMap<VehicleViewModel, Vehicle>();
            CreateMap<DeviceViewModel, Device>();
            CreateMap<AddressViewModel, Address>();
            CreateMap<DeviceModelViewModel, DeviceModels>();
            CreateMap<DeviceTypeViewModel, DeviceType>();

            CreateMap<RegisterViewModel, Admin>()
                .ForMember(mem => mem.FirstName, map => map.MapFrom(vm => vm.FirstName))
                .ForMember(mem => mem.LastName, map => map.MapFrom(vm => vm.LastName))
                .ForMember(mem => mem.Username, map => map.MapFrom(vm => vm.Username))
                .ForMember(mem => mem.Password, map => map.MapFrom(vm => vm.Password))
                .ForMember(mem => mem.Email, map => map.MapFrom(vm => vm.Email));

            CreateMap<DistributorViewModel, Distributor>()
                .ForMember(mem => mem.FirstName, map => map.MapFrom(vm => vm.FirstName))
                .ForMember(mem => mem.LastName, map => map.MapFrom(vm => vm.LastName))
                .ForMember(mem => mem.Username, map => map.MapFrom(vm => vm.Username))
                .ForMember(mem => mem.Address, map => map.MapFrom(vm => vm.Address))
                //.ForMember(mem => mem.Address.AddressLine1, map => map.MapFrom(vm => vm.Address.AddressLine1))
                //.ForMember(mem => mem.Address.AddressLine2, map => map.MapFrom(vm => vm.Address.AddressLine2))
                //.ForMember(mem => mem.Address.AddressLine3, map => map.MapFrom(vm => vm.Address.AddressLine3))
                //.ForMember(mem => mem.Address.City, map => map.MapFrom(vm => vm.Address.City))
                //.ForMember(mem => mem.Address.State, map => map.MapFrom(vm => vm.Address.State))
                //.ForMember(mem => mem.Address.Country, map => map.MapFrom(vm => vm.Address.Country))
                //.ForMember(mem => mem.Address.PostalCode, map => map.MapFrom(vm => vm.Address.PostalCode))
                .ForMember(mem => mem.Parent, map => map.MapFrom(vm => vm.Parent))
                .ForMember(mem => mem.CompanyName, map => map.MapFrom(vm => vm.CompanyName))
                .ForMember(mem => mem.Email, map => map.MapFrom(vm => vm.Email))
                .ForMember(mem => mem.PhoneNo, map => map.MapFrom(vm => vm.PhoneNo))
                .ForMember(mem => mem.RowVersion, map => map.MapFrom(vm => vm.RowVersion));

            CreateMap<AddressViewModel, Address>()
                .ForMember(mem => mem.AddressLine1, map => map.MapFrom(vm => vm.AddressLine1))
                .ForMember(mem => mem.AddressLine2, map => map.MapFrom(vm => vm.AddressLine2))
                .ForMember(mem => mem.AddressLine3, map => map.MapFrom(vm => vm.AddressLine3))
                .ForMember(mem => mem.City, map => map.MapFrom(vm => vm.City))
                .ForMember(mem => mem.State, map => map.MapFrom(vm => vm.State))
                .ForMember(mem => mem.Country, map => map.MapFrom(vm => vm.Country))
                .ForMember(mem => mem.PostalCode, map => map.MapFrom(vm => vm.PostalCode));

        }
    }
}