using AutoMapper;
using Backend.Domain.Repositories.SuperTiendaDbContext;
using Backend.Models.Dtos;
using Backend.Models.Dtos.SuperTienda;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controller.SuperTienda;

[Authorize]
[Route("api/supertienda/orderDetails")]
[ApiController]
public class OrderDetailsController : ControllerBase
{
    private readonly SuperTiendaDbContext _context;
    private readonly IMapper _mapper;

    public OrderDetailsController(SuperTiendaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // TO-DO --> Abstract the logic to a service


    // GET: api/OrderDetails
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetails()
    {
      if (_context.OrderDetails == null)
      {
          return NotFound();
      }
      var orderDetails = await _context.OrderDetails.ToListAsync();
      var orderDetailsDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
      return Ok(orderDetailsDto);

    }

    // GET: api/OrderDetails/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDetailDto>> GetOrderDetail(double id)
    {
      if (_context.OrderDetails == null)
      {
          return NotFound();
      }
      var orderDetail = await _context.OrderDetails.FindAsync(id);

      if (orderDetail == null)
      {
          return NotFound();
      }
      var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);

      return orderDetailDto;
    }

    // PUT: api/OrderDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrderDetail(double id, OrderDetailDto orderDetailDto)
    {
        if (id != orderDetailDto.LineaDetalle)
        {
            return BadRequest();
        }
        var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
        _context.Entry(orderDetail).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderDetailExists(id))
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

    // POST: api/OrderDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetailDto orderDetailDto)
    {
      if (_context.OrderDetails == null)
      {
          return Problem("Entity set 'SuperTiendaContext.OrderDetails'  is null.");
      }
      var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
      _context.OrderDetails.Add(orderDetail);
      try
      {
          await _context.SaveChangesAsync();
      }
      catch (DbUpdateException)
      {
          if (OrderDetailExists(orderDetail.LineaDetalle))
          {
              return Conflict();
          }
          else
          {
              throw;
          }
      }

      return CreatedAtAction("GetOrderDetail", new { id = orderDetail.LineaDetalle }, orderDetail);
    }

    // DELETE: api/OrderDetails/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrderDetail(double id)
    {
        if (_context.OrderDetails == null)
        {
            return NotFound();
        }
        var orderDetail = await _context.OrderDetails.FindAsync(id);
        if (orderDetail == null)
        {
            return NotFound();
        }

        _context.OrderDetails.Remove(orderDetail);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrderDetailExists(double id)
    {
        return (_context.OrderDetails?.Any(e => e.LineaDetalle == id)).GetValueOrDefault();
    }
}
