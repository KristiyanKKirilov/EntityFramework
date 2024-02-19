using AutoMapper;
using CarDealer.DTOs;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierDto, Supplier>();
            CreateMap<PartDto, Part>();
            CreateMap<CarDto, Car>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<SaleDto, Sale>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Car, CarDto>().ForMember(cdt => cdt.PartsId, c => c.MapFrom(x => x.PartsCars.Select(pc => pc.Part.Id)));
            //CreateMap<Car, CarPartsDto>();//.ForMember(cdt => cdt.Parts, c => c.MapFrom(x => x.PartsCars));
        }
    }
}
