using PurchaseOrder.Api.DTOs;
using PurchaseOrder.Api.Models;

namespace PurchaseOrder.Api.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<PagedResult<PurchaseOrderDto>> GetListAsync(PoQueryParameters query);
        Task<PurchaseOrderDto?> GetByIdAsync(Guid id);
        Task<PurchaseOrderDto> CreateAsync(CreatePoDto dto);
        Task<PurchaseOrderDto?> UpdateAsync(Guid id, CreatePoDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
