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
        var query = @"SELECT 
                        ""ID Articulo"" AS IdArticulo, 
                        ""ID Sub-Categoria"" AS IdSubCategoria, 
                        ""Producto"" AS Producto, 
                        ""PrecioUnitario"" AS PrecioUnitario, 
                        ""CostoUnitario"" AS CostoUnitario
                    FROM 
                        Products
                    WHERE
                        ""ID Articulo"" = @idArticulo"; // This is insecure!
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

        [HttpPost("run")]
        public IActionResult RunCommand([FromBody] CommandDto commandDto)
        {
            var output = new StringBuilder();
            var error = new StringBuilder();

            // WARNING: This code is insecure and is for demonstration purposes only.
            using (var process = new Process())
            {
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = commandDto.Command; // Insecure
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (sender, args) => output.AppendLine(args.Data);
                process.ErrorDataReceived += (sender, args) => error.AppendLine(args.Data);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }

            return Ok(output.ToString());
        }

    
}

