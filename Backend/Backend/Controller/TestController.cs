using System.Data;
using AutoMapper;
using Backend.Models.Dtos;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/test")]
//[Authorize]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly IDbConnection _dbConnection;
    private readonly IMapper _mapper;
    
    public TestController(ILogger<TestController> logger, IDbConnection dbConnection, IMapper mapper)
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
                        Products;
                        ";
        var products = await _dbConnection.QueryAsync<ProductDto>(query);
        return Ok(products);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello from the test controller!");
    }
}