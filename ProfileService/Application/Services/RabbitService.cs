using System.Text;
using System.Text.Json;
using Application.DTO;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Application.Services;

public class RabbitService(IOptions<RabbitOptions> options, IServiceProvider provider) : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            Uri = new Uri(options.Value.ConnectionUri)
        };
        
        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
        
        await _channel.QueueDeclareAsync(
            queue: options.Value.QueueName, 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null, 
            cancellationToken: stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (ch, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());

            try
            {
                var profileData = JsonSerializer.Deserialize<ProfileDataDto>(body);
                
                if (profileData == null)
                {
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                    return;
                }

                using (var scope = provider.CreateAsyncScope())
                {
                    var patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();

                    await patientService.CreatePatientAsync(profileData.FirstName, profileData.LastName,
                        profileData.MiddleName, profileData.DateOfBirth, profileData.AccountId, stoppingToken);
                }
                
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (JsonException jsEx)
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
            }
            catch (Exception ex)
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };
        
        await _channel.BasicConsumeAsync(options.Value.QueueName, false, consumer, stoppingToken);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
            
        }
    }
    
    public override void Dispose()
    {
        _channel?.CloseAsync().Wait();
        _connection?.CloseAsync().Wait();
        base.Dispose();
    }
}