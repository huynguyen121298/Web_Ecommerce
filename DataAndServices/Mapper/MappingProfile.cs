using AutoMapper;
using DataAndServices.DataModel;
using Model.DTO.DTO_Ad;

namespace DataAndServices.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CheckoutCustomerOrder, DTO_Checkout_Customer>();
        }
    }
}
