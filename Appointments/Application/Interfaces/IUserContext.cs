namespace Application.Interfaces;

public interface IUserContext
{
    Guid GetUserId();
    string GetUserRole();
}