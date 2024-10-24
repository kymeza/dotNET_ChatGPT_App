using System.Data;
using AutoMapper;
using Backend.Models.Dtos;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/vulnerable")]
public class VulnerableController : ControllerBase
{
    private readonly ILogger<VulnerableController> _logger;
    private readonly IDbConnection _dbConnection;
    private readonly IMapper _mapper;

    public VulnerableController(ILogger<VulnerableController> logger, IDbConnection dbConnection, IMapper mapper)
    {
        _logger = logger;
        _dbConnection = dbConnection;
        _mapper = mapper;
    }
    
    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
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
    public async Task<ActionResult<ProductDto>> GetProduct(string id)
    {
        var query = $@"SELECT 
                        ""ID Articulo"" AS IdArticulo, 
                        ""ID Sub-Categoria"" AS IdSubCategoria, 
                        ""Producto"" AS Producto, 
                        ""PrecioUnitario"" AS PrecioUnitario, 
                        ""CostoUnitario"" AS CostoUnitario
                    FROM 
                        Products
                    WHERE ""ID Articulo"" = '{id}'
                        ";
        var product = await _dbConnection.QueryAsync<ProductDto>(query);
        return Ok(product);
    }
    
}