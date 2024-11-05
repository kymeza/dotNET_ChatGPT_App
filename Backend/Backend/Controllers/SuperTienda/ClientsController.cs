using AutoMapper;
using Backend.Domain.Repositories.SuperTiendaDbContext;
using Backend.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers.SuperTienda;

[Authorize]
[Route("api/supertienda/clients")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly SuperTiendaDbContext _context;
    private readonly IMapper _mapper;


    public ClientsController(SuperTiendaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // TO-DO --> Abstract the logic to a service

    // GET: api/Clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
    {
        if (_context.Clients == null)
        {
            return NotFound();
        }

        var clients = await _context.Clients.ToListAsync();
        var cllentsDto = _mapper.Map<List<ClientDto>>(clients);

        return Ok(cllentsDto);
    }

    // GET: api/Clients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetClient(string id)
    {
        if (_context.Clients == null)
        {
            return NotFound();
        }

        var client = await _context.Clients.FindAsync(id);

        if (client == null)
        {
            return NotFound();
        }

        var clientDto = _mapper.Map<ClientDto>(client);

        return clientDto;
    }

    // PUT: api/Clients/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClient(string id, ClientDto clientDto)
    {
        if (id != clientDto.IdCliente)
        {
            return BadRequest();
        }

        var client = _mapper.Map<Client>(clientDto);

        _context.Entry(client).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id))
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

    // POST: api/Clients
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Client>> PostClient(ClientDto clientDto)
    {
        if (_context.Clients == null)
        {
            return Problem("Entity set 'SuperTiendaContext.Clients'  is null.");
        }

        var client = _mapper.Map<Client>(clientDto);
        _context.Clients.Add(client);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (ClientExists(client.IdCliente))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetClient", new { id = client.IdCliente }, client);
    }

    // DELETE: api/Clients/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(string id)
    {
        if (_context.Clients == null)
        {
            return NotFound();
        }

        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClientExists(string id)
    {
        return (_context.Clients?.Any(e => e.IdCliente == id)).GetValueOrDefault();
    }
}