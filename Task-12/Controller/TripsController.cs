using Microsoft.AspNetCore.Mvc;
using Task_12.DTOs;
using Task_12.Exceptions;
using Task_12.Service;

namespace Task_12.Controller;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
    {
        _tripService = tripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;
            
            var trips = await _tripService.GetAll(page, pageSize);
            return Ok(trips);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("{idClient}/clients")]
    public async Task<IActionResult> AssignClientToTrip([FromBody] AssignClientToTipDTO dto, int idClient)
    {
        try
        {
            await _tripService.ClientExists(dto.Pesel);
            await _tripService.ClientAssignedToTrip(idClient, dto.IdTrip);
            await _tripService.CheckTrip(dto.IdTrip, dto.PayementDate);

            await _tripService.AssignClientToTrip(dto, idClient);
            
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