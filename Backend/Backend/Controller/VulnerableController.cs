using System.Data;
using Backend.Models.Dtos.SuperTienda;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/vulnerable")]
public class VulnerableController : ControllerBase
{
    private readonly ILogger<VulnerableController> _logger;
    private readonly IDbConnection _dbConnection;

    public VulnerableController(ILogger<VulnerableController> logger, IDbConnection dbConnection)
    {
        _logger = logger;
        _dbConnection = dbConnection;
    }
    
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var query = @"SELECT 
                        ""ID Articulo"" AS IdArticulo, 
                        ""ID Sub-Categoria"" AS IdSubCategoria, 
                        ""Producto"" AS Producto, 
                        ""PrecioUnitario"" AS PrecioUnitario, 
                        ""CostoUnitario"" AS CostoUnitario
                    FROM 
                        Products
                    LIMIT 10;
                        ";
        var products = await _dbConnection.QueryAsync<ProductDto>(query);
        return Ok(products);
    }
    
    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProducts(string id)
    {
        var query = $@"SELECT 
                        ""ID Articulo"" AS IdArticulo, 
                        ""ID Sub-Categoria"" AS IdSubCategoria, 
                        ""Producto"" AS Producto, 
                        ""PrecioUnitario"" AS PrecioUnitario, 
                        ""CostoUnitario"" AS CostoUnitario
                    FROM 
                        Products
                    WHERE
                        IdArticulo = '{id}'"; // This is insecure!
        var product = await _dbConnection.QueryAsync<ProductDto>(query);
        return Ok(product);

    }
}