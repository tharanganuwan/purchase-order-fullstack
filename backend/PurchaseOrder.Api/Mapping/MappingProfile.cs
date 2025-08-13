using AutoMapper;
using PurchaseOrder.Api.DTOs;
using PurchaseOrder.Api.Entities;

namespace PurchaseOrder.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PurchaseOrders, PurchaseOrderDto>().ReverseMap();
            CreateMap<CreatePoDto, PurchaseOrders>();
        }
    }
}
