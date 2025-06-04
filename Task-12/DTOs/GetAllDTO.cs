namespace Task_12.DTOs;

public class GetAllDTO
{
    public int PageNum {get;set;}
    public int PageSize {get;set;}
    public int AllPages {get;set;}
    public List<GetTripDTO> Trips {get;set;}
}

public class GetTripDTO
{
    public string Name {get;set;}
    public string Description {get;set;}
    public DateTime? DateFrom {get;set;}
    public DateTime? DateTo {get;set;}
    public int MaxPeople {get;set;}
    public List<GetCountryDTO> Countries {get;set;}
    public List<GetClientDTO> Clients {get;set;}
}

public class GetCountryDTO
{
    public string Name {get;set;}
}

public class GetClientDTO
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
}