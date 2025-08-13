using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using PurchaseOrder.Application.DTOs;
using PurchaseOrder.Application.Interfaces;
using PurchaseOrder.Application.Models;
using System.Collections.Generic;
using PurchaseOrder.Api.Controllers;

namespace PurchaseOrder.Api.Tests.Controllers
{
    public class PurchaseOrdersControllerTests
    {
        private readonly Mock<IPurchaseOrderService> _serviceMock;
        private readonly PurchaseOrdersController _controller;

        public PurchaseOrdersControllerTests()
        {
            _serviceMock = new Mock<IPurchaseOrderService>();
            _controller = new PurchaseOrdersController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetList_ReturnsOk_WithPagedResult()
        {
            // Arrange
            var query = new PoQueryParameters();
            var pagedResult = new PagedResult<PurchaseOrderDto>
            {
                Items = new List<PurchaseOrderDto> { new PurchaseOrderDto { PoNumber = "PO1" } },
                TotalCount = 1
            };
            _serviceMock.Setup(s => s.GetListAsync(query)).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Get(query) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(pagedResult, result.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var id = Guid.NewGuid();
            var po = new PurchaseOrderDto { Id = id, PoNumber = "PO123" };
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(po);

            var result = await _controller.Get(id) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(po, result.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PurchaseOrderDto)null);

            var result = await _controller.Get(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsCreated_WhenSuccessful()
        {
            var dto = new CreatePoDto { PoNumber = "PO1" };
            var created = new PurchaseOrderDto { Id = Guid.NewGuid(), PoNumber = "PO1" };
            _serviceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await _controller.Post(dto) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(created, result.Value);
            Assert.Equal(nameof(_controller.Get), result.ActionName);
        }

        [Fact]
        public async Task Put_ReturnsOk_WhenUpdated()
        {
            var id = Guid.NewGuid();
            var dto = new CreatePoDto { PoNumber = "PO1" };
            var updated = new PurchaseOrderDto { Id = id, PoNumber = "PO1" };

            _serviceMock.Setup(s => s.UpdateAsync(id, dto)).ReturnsAsync(updated);

            var result = await _controller.Put(id, dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(updated, result.Value);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<CreatePoDto>()))
                .ReturnsAsync((PurchaseOrderDto)null);

            var result = await _controller.Put(Guid.NewGuid(), new CreatePoDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await _controller.Delete(Guid.NewGuid());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

            var result = await _controller.Delete(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
