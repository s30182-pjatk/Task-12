using Microsoft.EntityFrameworkCore;
using Task_12.Context;
using Task_12.DTOs;
using Task_12.Exceptions;
using Task_12.Migrations;

namespace Task_12.Service;

public class TripService : ITripService
{
    
    private readonly MyDbContext _context;

    public TripService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<GetAllDTO> GetAll(int page, int pageSize)
    {


        var queryTrips =  _context.Trips
            .OrderByDescending(trip => trip.DateFrom)    
            .Select(trip =>
        
            new GetTripDTO()
            {
                Name = trip.Name,
                Description = trip.Description,
                DateFrom = trip.DateFrom,
                DateTo = trip.DateTo,
                MaxPeople = trip.MaxPeople,
                Countries = trip.IdCountries.Select(country => new GetCountryDTO()
                {
                    Name = country.Name,
                }).ToList(),
                Clients = trip.ClientTrips.Select(client => new GetClientDTO()
                {
                    FirstName = client.IdClientNavigation.FirstName,
                    LastName = client.IdClientNavigation.LastName,
                }).ToList()
            }
        );
        
        var totalCount = await _context.Trips.CountAsync();
        var tripsOnPage = await queryTrips
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (tripsOnPage == null)
        {
            throw new NotFoundException("Trips not found");
        }
        
        var result = new GetAllDTO();
        result.PageNum = page;
        result.PageSize = pageSize;
        result.Trips = tripsOnPage;
        result.AllPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        return result;

    }

    public async Task ClientHasTrip(int clientId)
    {
        var res = await _context.ClientTrips.FirstOrDefaultAsync(ct => ct.IdClient == clientId);
        if (res != null)
        {
            throw new ConflictException("Client has trip");
        }
    }

    public async Task DeleteClient(int clientId)
    {
        var client = await _context.Clients.FindAsync(clientId);

        if (client == null)
        {
            throw new NotFoundException("Client not found");
        }

        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw e;
        }
    }

    public async Task ClientExists(string pesel)
    {
        var res = await _context.Clients.FirstOrDefaultAsync(client => client.Pesel == pesel);
        if (res == null)
        {
            throw new NotFoundException("Client not found");
        }
    }

    public async Task ClientAssignedToTrip(int clientId, int tripId)
    {
        var res = await _context.ClientTrips.FirstOrDefaultAsync(ct => ct.IdTrip == tripId && ct.IdClient == clientId);
        if (res != null)
        {
            throw new ConflictException("Client already assigned to trip");
        }
    }

    public async Task CheckTrip(int tripId, DateTime? date)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        if (trip == null)
        {
            throw new NotFoundException("Trip not found");
        }

        if (date == null)
        {
            return;
        }
        
        if (date < DateTime.Now)
        {
            throw new ConflictException("Trip has to be in the future");
        }
        
    }

    public async Task AssignClientToTrip(AssignClientToTipDTO dto, int IdClient)
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.ClientTrips.AddAsync(new ClientTrip()
            {
                IdClient = IdClient,
                IdTrip = dto.IdTrip,
                PaymentDate = dto.PayementDate,
                RegisteredAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw e;
        }
    }
}