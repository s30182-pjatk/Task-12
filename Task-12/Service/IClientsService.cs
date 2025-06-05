namespace Task_12.Service;

public interface IClientsService
{
    
    Task ClientHasTrip(int clientId);
    Task DeleteClient(int clientId);
}