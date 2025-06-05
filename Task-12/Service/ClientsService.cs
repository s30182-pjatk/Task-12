using Microsoft.EntityFrameworkCore;
using Task_12.Context;
using Task_12.Exceptions;

namespace Task_12.Service;

public class ClientsService : IClientsService
{
    
    private readonly MyDbContext _context;

    public ClientsService(MyDbContext context)
    {
        _context = context;
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
}