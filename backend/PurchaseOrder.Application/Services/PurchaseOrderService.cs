using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PurchaseOrder.Application.DTOs;
using PurchaseOrder.Application.Interfaces;
using PurchaseOrder.Application.Models;
using PurchaseOrder.Domain.Entities;
using PurchaseOrder.Domain.Interfaces;


namespace PurchaseOrder.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepository<PurchaseOrders> _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaseOrderService> _logger;

        public PurchaseOrderService(IRepository<PurchaseOrders> repo, IMapper mapper, ILogger<PurchaseOrderService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<PurchaseOrderDto>> GetListAsync(PoQueryParameters query)
        {
            try
            {
                var q = _repo.Query();

                if (!string.IsNullOrWhiteSpace(query.Supplier))
                    q = q.Where(x => x.SupplierName.Contains(query.Supplier));

                if (query.Status.HasValue)
                    q = q.Where(x => x.Status == query.Status.Value);

                if (query.From.HasValue)
                    q = q.Where(x => x.OrderDate >= query.From.Value);

                if (query.To.HasValue)
                    q = q.Where(x => x.OrderDate <= query.To.Value);

                // Sorting
                q = query.SortBy?.ToLower() switch
                {
                    "ponumber" => query.SortDesc ? q.OrderByDescending(x => x.PoNumber) : q.OrderBy(x => x.PoNumber),
                    "totalamount" => query.SortDesc ? q.OrderByDescending(x => x.TotalAmount) : q.OrderBy(x => x.TotalAmount),
                    _ => query.SortDesc ? q.OrderByDescending(x => x.OrderDate) : q.OrderBy(x => x.OrderDate)
                };

                var totalCount = await q.CountAsync();

                var items = await q
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ProjectTo<PurchaseOrderDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new PagedResult<PurchaseOrderDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving purchase orders list");
                throw new ApplicationException("Failed to retrieve purchase orders", ex);
            }
        }

        public async Task<PurchaseOrderDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var po = await _repo.Query()
                    .Where(x => x.Id == id)
                    .ProjectTo<PurchaseOrderDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return po;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving purchase order with ID {Id}", id);
                throw new ApplicationException("Failed to retrieve purchase order", ex);
            }
        }

        public async Task<PurchaseOrderDto> CreateAsync(CreatePoDto dto)
        {
            try
            {
                var exists = await _repo.Query().AnyAsync(x => x.PoNumber == dto.PoNumber);
                if (exists)
                    throw new InvalidOperationException("PO Number already exists");

                var po = _mapper.Map<PurchaseOrders>(dto);
                po.Status = PurchaseOrderStatus.Draft;
                po.CreatedAt = DateTime.Now;

                await _repo.AddAsync(po);
                await _repo.SaveChangesAsync();

                return _mapper.Map<PurchaseOrderDto>(po);
            }
            catch (InvalidOperationException ex) 
            {
                _logger.LogWarning(ex, "Business validation failed for CreateAsync");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating purchase order");
                throw new ApplicationException("Failed to create purchase order", ex);
            }
        }

        public async Task<PurchaseOrderDto?> UpdateAsync(Guid id, CreatePoDto dto)
        {
            try
            {
                var po = await _repo.GetByIdAsync(id);
                if (po == null) return null;

                if (po.Status == PurchaseOrderStatus.Completed)
                    throw new InvalidOperationException("Cannot update a completed purchase order");

                var updatedPo = _mapper.Map(dto, po);
                updatedPo.PoNumber = po.PoNumber;
                updatedPo.UpdatedAt = DateTime.Now;

                _repo.Update(updatedPo);
                await _repo.SaveChangesAsync();

                return _mapper.Map<PurchaseOrderDto>(updatedPo);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business validation failed for UpdateAsync");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating purchase order with ID {Id}", id);
                throw new ApplicationException("Failed to update purchase order", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var po = await _repo.GetByIdAsync(id);
                if (po == null) return false;

                _repo.Remove(po);
                await _repo.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting purchase order with ID {Id}", id);
                throw new ApplicationException("Failed to delete purchase order", ex);
            }
        }
    }
}
