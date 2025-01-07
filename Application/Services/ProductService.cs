namespace Application.Services
{
  using Application.Interfaces;
  using Domain.Dtos;
  using Domain.Entities;
  using Microsoft.EntityFrameworkCore;
  using Persistence;

  public class ProductService : IProductService
  {
    private readonly DataContext dataContext;

    public ProductService(DataContext dataContext)
    {
      this.dataContext = dataContext;
    }

    public async Task<List<ProductWareHouseDto>> GetProductsByProductCodeOrWarehouseCodeAsync(string? productCode, string? warehouseCode)
    {
      var query = this.dataContext.ProductWareHouses.AsQueryable();

      if (!string.IsNullOrEmpty(productCode))
      {
        query = query.Where(pw => pw.Product.ProductCode == productCode);
      }

      if (!string.IsNullOrEmpty(warehouseCode))
      {
        query = query.Where(pw => pw.WareHouse.WareHouseCode == warehouseCode);
      }

      return await query.Select(pw => new ProductWareHouseDto
      {
        ProductCode = pw.Product.ProductCode,
        WareHouseCode = pw.WareHouse.WareHouseCode,
        WareHouseName = pw.WareHouse.WareHouseName,
        Quantity = pw.Quantity
      }).ToListAsync();
    }

    public async Task<GeneralServiceResponseDto> CreateProductAsync(CreateProductDto product)
    {
      if (product == null)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product cannot be null.");
      }

      var validationResponse = this.ValidateProduct(product);
      if (!validationResponse.IsSucceed)
      {
        return validationResponse;
      }

      var existingProduct = await this.dataContext.Products
          .Where(p => p.ProductCode == product.ProductCode)
          .FirstOrDefaultAsync();

      if (existingProduct != null)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product code must be unique.");
      }

      var productNew = new Product
      {
        ProductCode = product.ProductCode,
        ProductQuantity = product.ProductQuantity,
        ProductDescription = product.ProductDescription
      };

      var warehouse = await GetWarehouseAsync(product.WareHouseCode);
      if (warehouse == null)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 404, "Warehouse not found.");
      }
      await this.dataContext.Products.AddAsync(productNew);
      await this.dataContext.SaveChangesAsync();

      await CreateProductWarehouseAssociation(productNew.Id, warehouse.Value, product.ProductQuantity);

      try
      {
        await this.dataContext.SaveChangesAsync();
        return GeneralServiceResponseDto.CreateResponse(true, 200, "Product created and stored in Warehouse successfully.");
      }
      catch (Exception ex)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 500, $"An error occurred: {ex.Message}");
      }
    }


    private GeneralServiceResponseDto ValidateProduct(CreateProductDto product)
    {
      if (string.IsNullOrEmpty(product.ProductCode))
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product code is required.");
      }

      if (product.ProductQuantity <= 0)
      {
        return GeneralServiceResponseDto.CreateResponse(false, 400, "Product quantity must be greater than 0.");
      }

      return GeneralServiceResponseDto.CreateResponse(true, 200, "Product is valid.");
    }

    private async Task<int?> GetWarehouseAsync(string warehouseCode)
    {
      return await this.dataContext.WareHouses
                                   .Where(w => w.WareHouseCode == warehouseCode)
                                   .Select(w => (int?)w.Id)
                                   .FirstOrDefaultAsync();
    }

    private async Task CreateProductWarehouseAssociation(int productId, int warehouseId, int quaantity)
    {
      var productWarehouse = new ProductWareHouse
      {
        ProductId = productId,
        WareHouseId = warehouseId,
        Quantity = quaantity
      };

      await this.dataContext.ProductWareHouses.AddAsync(productWarehouse);
      this.dataContext.SaveChanges();
    }
  }
}