using Application.AuthDTO;

namespace Application.Interfaces;

public interface IRabbitService
{
    public Task CreateUserProfileAsync(ProfileDataRabbit pofileData, string role,  CancellationToken cancellationToken);
}