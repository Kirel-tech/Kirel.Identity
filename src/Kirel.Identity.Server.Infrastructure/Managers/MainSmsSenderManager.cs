using System.Net.Http.Headers;
using System.Net.Http.Json;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Server.Infrastructure.Models;

namespace Kirel.Identity.Server.Infrastructure.Managers;

/// <summary>
/// Manager for sms sending
/// </summary>
public class MainSmsSenderManager : IKirelSmsSender
{
    private readonly MainSmsSenderConfig _config;
    
    /// <summary>
    /// Constructor for SmsSenderManager
    /// </summary>
    /// <param name="config">Sms sender configuration</param>
    public MainSmsSenderManager(MainSmsSenderConfig config)
    {
        _config = config;
    }
    
    /// <summary>
    /// Sends sms message with given text to the given phone number
    /// </summary>
    /// <param name="text">Sms message text</param>
    /// <param name="phoneNumber">Phone number</param>
    public async Task SendSms(string text, string phoneNumber)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync($"https://mainsms.ru/api/mainsms/message/send?apikey={_config.ApiKey}&project={_config.Project}&recipients={phoneNumber}&message={text}");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to send request to the sms service");
        }
    }
}
