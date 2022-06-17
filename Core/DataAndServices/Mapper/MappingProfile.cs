using AutoMapper;
using DataAndServices.DataModel;
using Model.DTO.DTO_Ad;
using Model.DTO_Model;

namespace DataAndServices.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CheckoutCustomerOrder, DTO_Checkout_Customer>();
            CreateMap<CheckoutCustomerOrder, DTOCheckoutCustomerOrder>();
            CreateMap<Checkout_Oder, DTO_Checkout_Order>();
        }
    }
}