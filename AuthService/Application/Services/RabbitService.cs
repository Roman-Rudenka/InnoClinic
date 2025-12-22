using System.Text;
using System.Text.Json;
using Application.AuthDTO;
using Application.Interfaces;
using Application.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Application.Services;

public class RabbitService(IOptions<RabbitOptions> rabbitOptions) : IRabbitService
{
    public async Task CreateUserProfileAsync(ProfileDataRabbit profileData, string role, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory() 
        { 
            Uri = new Uri(rabbitOptions.Value.ConnectionUri) 
        };
        
        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        
        string exchangeName = rabbitOptions.Value.QueueName;
        
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName, 
            type: ExchangeType.Direct, 
            durable: true, 
            autoDelete: false, 
            cancellationToken: cancellationToken);
        
        var routingKey = role.ToLower() switch
        {
            "reception" => "user.created.reception",
            "doctor" => "user.created.doctor",
            _ => "user.created.patient"
        };
        
        var profileDataToJson = JsonSerializer.Serialize(profileData);
        var body = Encoding.UTF8.GetBytes(profileDataToJson);
        
        await channel.BasicPublishAsync(
            exchange: exchangeName, 
            routingKey: routingKey, 
            body: body, 
            cancellationToken: cancellationToken);
    }
}