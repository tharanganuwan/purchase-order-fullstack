using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PurchaseOrder.Api.DTOs;
using PurchaseOrder.Api.Entities;
using PurchaseOrder.Api.Interfaces;
using PurchaseOrder.Api.Models;
using static PurchaseOrder.Api.Interfaces.IRepository;

namespace PurchaseOrder.Api.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IRepository<PurchaseOrders> _repo;
        private readonly IMapper _mapper;

        public PurchaseOrderService(IRepository<PurchaseOrders> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<PurchaseOrderDto>> GetListAsync(PoQueryParameters query)
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

        public async Task<PurchaseOrderDto?> GetByIdAsync(Guid id)
        {
            var po = await _repo.Query()
                .Where(x => x.Id == id)
                .ProjectTo<PurchaseOrderDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return po;
        }

        public async Task<PurchaseOrderDto> CreateAsync(CreatePoDto dto)
        {
            // Check for unique PoNumber
            var exists = await _repo.Query().AnyAsync(x => x.PoNumber == dto.PoNumber);
            if (exists)
                throw new Exception("PO Number already exists");

            var po = _mapper.Map<PurchaseOrders>(dto);
            po.Status = PurchaseOrderStatus.Draft;
            po.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(po);
            await _repo.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderDto>(po);
        }

        public async Task<PurchaseOrderDto?> UpdateAsync(Guid id, CreatePoDto dto)
        {
            var po = await _repo.GetByIdAsync(id);
            if (po == null) return null;

            if (po.Status == PurchaseOrderStatus.Completed)
                throw new Exception("Cannot update a completed purchase order");

            // Keep PoNumber immutable
            var updatedPo = _mapper.Map(dto, po);
            updatedPo.PoNumber = po.PoNumber;
            updatedPo.UpdatedAt = DateTime.UtcNow;

            _repo.Update(updatedPo);
            await _repo.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderDto>(updatedPo);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var po = await _repo.GetByIdAsync(id);
            if (po == null) return false;

            _repo.Remove(po);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
