using AutoMapper;
using PurchaseOrder.Application.DTOs;
using PurchaseOrder.Domain.Entities;

namespace PurchaseOrder.Application.Mapping
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
