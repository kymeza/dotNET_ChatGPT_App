using AutoMapper;
using Backend.Domain.Repositories.SuperTienda;
using Backend.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.SuperTienda;

[Authorize]
[ApiController]
[Route("api/supertienda/products")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly SuperTiendaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public ProductsController(ILogger<ProductsController> logger, SuperTiendaDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(int pageNumber = 1, int pageSize = 20)
    {
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
        _logger.LogInformation("User: {user} has sent a Product Create request",_httpContextAccessor.HttpContext.User.Identity.Name);
        
        if (_dbContext.Products == null)
        {
            return Problem("Entity set 'SuperTiendaContext.Products'  is null.");
        }
        
        //TODO: VALIDAR LOS CAMPOS QUE EL USUARIO PUEDE CONTROLAR

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