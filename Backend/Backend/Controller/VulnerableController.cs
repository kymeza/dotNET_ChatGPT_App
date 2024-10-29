using System.Data;
using System.Diagnostics;
using System.Text;
using AutoMapper;
using Backend.Models.Dtos;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/vulnerable")]
//[Authorize]
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
        // Recomendaciones
        // 1. No permitir que el usuario maneje el nombre del archivo
        // 2. Ignorar el nombre de archivo original y generar un GUID
        // 3. No permitir caracteres especiales usando un RegEx
        // 4. VALIDAR EL HEADER DEL CONTENT TYPE DEL ARCHIVO


        if (file == null || file.Length == 0)
        {
            return BadRequest("No file provided.");
        }

        // Generar un GUID para el archivo
        var fileId = Guid.NewGuid().ToString();

        // Obtener y validar la extensión del archivo
        var extension = Path.GetExtension(file.FileName).ToLower();
        var allowedExtensions = new[] { ".jpg", ".png", ".pdf", ".txt" };
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("Invalid file type.");
        }

        // Generar el nombre final del archivo
        fileName = fileId + extension;

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var fullPath = Path.Combine(uploadPath, fileName);
        if (!fullPath.StartsWith(uploadPath))
        {
            return BadRequest("Invalid file path.");
        }

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
    
    // ESTE ENDPOINT ES PARA SUPONER QUE UN ATACANTE LOGRA INVOCAR UNA TERMINAL DESDE LA APP WEB
    [HttpPost("run")]
    public IActionResult RunCommand([FromBody] CommandRequest commandRequest)
    {
        var output = new StringBuilder();
        var error = new StringBuilder();

        // WARNING: This code is insecure and is for demonstration purposes only.
        using (var process = new Process())
        {
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.Arguments = commandRequest.Command; // Insecure
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
    
    public class CommandRequest
    {
        public string Command { get; set; }
    }
    
}