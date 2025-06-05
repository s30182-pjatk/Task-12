using Microsoft.AspNetCore.Mvc;
using Task_12.Exceptions;
using Task_12.Service;

namespace Task_12.Controller;


[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    
    private readonly IClientsService _service;

    public ClientsController(IClientsService service)
    {
        _service = service;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await _service.ClientHasTrip(idClient);
            await _service.DeleteClient(idClient);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
}