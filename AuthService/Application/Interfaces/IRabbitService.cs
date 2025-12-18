using Application.AuthDTO;

namespace Application.Interfaces;

public interface IRabbitService
{
    public Task CreateUserProfileAsync(ProfileDataDto pofileData, string role,  CancellationToken cancellationToken);
}