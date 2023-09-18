namespace Kirel.Identity.Core.Interfaces;

/// <summary>
/// Interface for sms sender
/// </summary>
public interface ISmsSender
{
    /// <summary>
    /// Sends sms message to the given phone number
    /// </summary>
    /// <param name="text"></param>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    Task SendSms(string text, string phoneNumber);
}