using PurchaseOrder.Domain.Entities;

namespace PurchaseOrder.Application.Models
{
    public class PoQueryParameters
    {
        public string? Supplier { get; set; }
        public PurchaseOrderStatus? Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? SortBy { get; set; } = "OrderDate";
        public bool SortDesc { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
