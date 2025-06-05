using Task_12.DTOs;

namespace Task_12.Service;

public interface ITripService
{
    Task<GetAllDTO> GetAll(int page, int pageSize);
    Task ClientExists(string pesel);
    Task ClientAssignedToTrip(int clientId, int tripId);
    Task CheckTrip(int tripId, DateTime? date);
    Task AssignClientToTrip(AssignClientToTipDTO dto, int IdClient);
}