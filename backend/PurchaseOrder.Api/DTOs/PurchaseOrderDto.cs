using PurchaseOrder.Api.Entities;

namespace PurchaseOrder.Api.DTOs
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public string PoNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PurchaseOrderStatus Status { get; set; }
    }
}
