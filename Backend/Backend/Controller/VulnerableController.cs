using System.Data;
using AutoMapper;
using Backend.Models.Dtos;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/vulnerable")]
[Authorize]
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
        var query = @"SELECT 
                        ""ID Articulo"" AS IdArticulo, 
                        ""ID Sub-Categoria"" AS IdSubCategoria, 
                        ""Producto"" AS Producto, 
                        ""PrecioUnitario"" AS PrecioUnitario, 
                        ""CostoUnitario"" AS CostoUnitario
                    FROM 
                        Products
                    WHERE ""ID Articulo"" = @idArticulo
                        ";
        
        var product = await _dbConnection.QueryAsync<ProductDto>(query, new { idArticulo = id });
        return Ok(product);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string fileName)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file provided.");
        }
        
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
        
        var extension = Path.GetExtension(file.FileName);
        
        fileName = fileName + extension;
        
        var fullPath = Path.Combine(uploadPath, fileName);
        
        try
        {
            // Save file to disk
            using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, such as access denied, path not found, etc.
            return StatusCode(500, "Internal Server Error: " + ex.Message);
        }

        return Ok("File uploaded successfully.");
        
    }
    
    
}