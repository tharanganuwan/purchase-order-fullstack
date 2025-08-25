using System.ComponentModel.DataAnnotations;
using PurchaseOrder.Domain.Entities;

namespace PurchaseOrder.Application.DTOs
{
    public class CreatePoDto
    {
        [Required, MaxLength(50)]
        public string PoNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public string SupplierName { get; set; } = string.Empty;
        [Required]
        public DateTime OrderDate { get; set; }
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        public PurchaseOrderStatus? Status { get; set; }
    }
}
