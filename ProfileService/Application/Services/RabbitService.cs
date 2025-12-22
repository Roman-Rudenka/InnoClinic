using System.Text;
using System.Text.Json;
using Application.DTO;
using Application.Interfaces;
using Application.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Application.Services;

public class RabbitService(
    IOptions<RabbitOptions> options,
    IServiceProvider provider) : BackgroundService
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
        
        var exchangeName = options.Value.QueueName; 
        
        await _channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);
        
        var queueName = "profile_service_queue";

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);
        
        await _channel.QueueBindAsync(queueName, exchangeName, "user.created.patient", cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queueName, exchangeName, "user.created.doctor", cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queueName, exchangeName, "user.created.reception", cancellationToken: stoppingToken);

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
                    var resolver = scope.ServiceProvider.GetRequiredService<IIdResolverService>();

                    switch (ea.RoutingKey)
                    {
                        case "user.created.patient":
                            var patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
                            await patientService.CreatePatientAsync(
                                profileData.FirstName, profileData.LastName, profileData.MiddleName,
                                profileData.DateOfBirth, profileData.AccountId, stoppingToken);
                            break;

                        case "user.created.doctor":
                            var doctorService = scope.ServiceProvider.GetRequiredService<IDoctorService>();

                            if (string.IsNullOrEmpty(profileData.SpecializationName) || string.IsNullOrEmpty(profileData.OfficeAddress))
                                throw new InvalidDataException("Doctor data missing Spec or Address");
                            
                            var specId = await resolver.ResolveSpecializationIdAsync(profileData.SpecializationName, stoppingToken);
                            var docOfficeId = await resolver.ResolveOfficeIdAsync(profileData.OfficeAddress, stoppingToken);
                            
                            await doctorService.CreateDoctorAsync(
                                profileData.FirstName, profileData.LastName, profileData.MiddleName,
                                profileData.DateOfBirth, profileData.AccountId,
                                specId,
                                docOfficeId,
                                profileData.StartWorkDate ?? DateOnly.FromDateTime(DateTime.Now),
                                stoppingToken);
                            break;

                        case "user.created.reception":
                            var receptionService = scope.ServiceProvider.GetRequiredService<IReceptionService>();

                            if (string.IsNullOrEmpty(profileData.OfficeAddress))
                                throw new InvalidDataException("Reception data missing Office Address");
                            
                            var recOfficeId = await resolver.ResolveOfficeIdAsync(profileData.OfficeAddress, stoppingToken);
                            
                            await receptionService.CreateReceptionAsync(
                                profileData.FirstName, profileData.LastName, profileData.MiddleName,
                                profileData.DateOfBirth, profileData.AccountId,
                                recOfficeId,
                                stoppingToken);
                            break;
                    }
                }
                
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (JsonException)
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
            }
            catch (Exception ex)
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
            }
        };

        await _channel.BasicConsumeAsync(queueName, false, consumer, stoppingToken);
        
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
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
}