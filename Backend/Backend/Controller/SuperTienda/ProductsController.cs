using AutoMapper;
using Backend.Domain.Repositories.SuperTiendaDbContext;
using Backend.Models.Dtos.SuperTienda;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller.SuperTienda;

[Route("api/supertienda/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly SuperTiendaContext _context;
    private readonly IMapper _mapper;

    public ProductsController(SuperTiendaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // TO-DO --> Abstract the logic to a service


    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        if (_context.Products == null)
        {
            return NotFound();
        }

        var products = await _context.Products.ToListAsync();
        var productsDto = _mapper.Map<List<ProductDto>>(products);
        return Ok(productsDto);
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string id)
    {
        if (_context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);

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
    public async Task<IActionResult> PutProduct(string id, Product productDto)
    {
        if (id != productDto.IdArticulo)
        {
            return BadRequest();
        }

        var product = _mapper.Map<Product>(productDto);
        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
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
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'SuperTiendaContext.Products'  is null.");
        }

        _context.Products.Add(product);
        try
        {
            await _context.SaveChangesAsync();
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
        if (_context.Products == null)
        {
            return NotFound();
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(string id)
    {
        return (_context.Products?.Any(e => e.IdArticulo == id)).GetValueOrDefault();
    }
}