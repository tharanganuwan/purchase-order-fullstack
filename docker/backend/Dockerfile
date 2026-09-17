# ==========================================
# Build stage
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project files first
# This improves Docker layer caching.
COPY backend/PurchaseOrder.Api/PurchaseOrder.Api.csproj PurchaseOrder.Api/
COPY backend/PurchaseOrder.Application/PurchaseOrder.Application.csproj PurchaseOrder.Application/
COPY backend/PurchaseOrder.Domain/PurchaseOrder.Domain.csproj PurchaseOrder.Domain/
COPY backend/PurchaseOrder.Infrastructure/PurchaseOrder.Infrastructure.csproj PurchaseOrder.Infrastructure/

# Restore dependencies
RUN dotnet restore PurchaseOrder.Api/PurchaseOrder.Api.csproj

# Copy source code
COPY backend/ .

# Build
WORKDIR /src/PurchaseOrder.Api

RUN dotnet build PurchaseOrder.Api.csproj \
    -c Release \
    -o /app/build \
    --no-restore


# ==========================================
# Publish stage
# ==========================================
FROM build AS publish

RUN dotnet publish PurchaseOrder.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false


# ==========================================
# Runtime stage
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

# ASP.NET Core will listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "PurchaseOrder.Api.dll"]