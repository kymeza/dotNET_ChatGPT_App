using System.Security.Claims;
using AutoMapper;
using Backend.Domain.Repositories.SuperTiendaDbContext;
using Backend.Models.Dtos.SuperTienda;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller.SuperTienda;

[Authorize]
[Route("api/supertienda/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly SuperTiendaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProductsController(SuperTiendaDbContext dbContext, IMapper mapper, ILogger<ProductsController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    // TO-DO --> Abstract the logic to a service


    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(int pageNumber = 1, int pageSize = 20)
    {
        var userLoggedIn = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Value == ClaimTypes.Name);
        _logger.LogInformation("User {user} has requested api/Products endpoint", userLoggedIn);
        if (_dbContext.Products == null)
        {
            return NotFound();
        }

        var products = await _dbContext.Products
            .OrderBy(x=> x.IdArticulo)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var productsDto = _mapper.Map<List<ProductDto>>(products);
        return Ok(productsDto);
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string id)
    {
        if (_dbContext.Products == null)
        {
            return NotFound();
        }

        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var productDto = _mapper.Map<ProductDto>(product);

        return productDto;
    }

    // PUT: api/Products/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(string id, ProductDto productDto)
    {
        if (id != productDto.IdArticulo)
        {
            return BadRequest();
        }

        var product = _mapper.Map<Product>(productDto);
        _dbContext.Entry(product).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Products
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(ProductDto productDto)
    {
        if (_dbContext.Products == null)
        {
            return Problem("Entity set 'SuperTiendaContext.Products'  is null.");
        }
        
        //TODO: Validar los campos del product para no permitir caracteres especiales ni descripciones o valores
        // no alfanumericos
        
        var product = _mapper.Map<Product>(productDto);
        _dbContext.Products.Add(product);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (ProductExists(product.IdArticulo))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetProduct", new { id = product.IdArticulo }, product);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        if (_dbContext.Products == null)
        {
            return NotFound();
        }

        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(string id)
    {
        return (_dbContext.Products?.Any(e => e.IdArticulo == id)).GetValueOrDefault();
    }
}