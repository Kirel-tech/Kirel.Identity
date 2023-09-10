namespace Kirel.Identity.Core.Models;
/// <summary>
/// Dictionary for storing user tokens and their expiration dates.
/// </summary>
/// <typeparam name="TKey"> The type of the user ID key. </typeparam>
public class KirelUserToken<TKey> : Dictionary<TKey, Dictionary<string, List<(string Token, DateTime ExpirationDate)>>>
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// Generate token
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="provider">token provider</param>
    /// <returns></returns>
    public string GenerateToken(TKey userId, string provider)
    {

        if (!TryGetValue(userId, out var storage))
        {
            storage = new Dictionary<string, List<(string Token, DateTime ExpirationDate)>>();
            this[userId] = storage;
        }

        if (!storage.TryGetValue(provider, out var tokens))
        {
            tokens = new List<(string Token, DateTime ExpirationDate)>();
            storage[provider] = tokens;
        }
        var random = new Random();
        var randomNumber = random.Next(100000, 1000000).ToString();

        tokens.Add((randomNumber, DateTime.Now.ToUniversalTime().AddMinutes(30)));
        return randomNumber;
    }

    /// <summary>
    /// Validate token 
    /// </summary>
    /// <param name="userId">user id </param>
    /// <param name="provider">token provider</param>
    /// <param name="token">our token for cinfirmation</param>
    /// <returns></returns>
    public bool ValidateToken(TKey userId, string provider,string token)
    {
        
        if (!TryGetValue(userId, out var storage))
        {
            return false;
        }

        if (!storage.TryGetValue(provider, out var tokens))
        {
            return false;
        }

        var tokenPairs =
            tokens.FirstOrDefault(t => t.Token == token && t.ExpirationDate > DateTime.Now.ToUniversalTime());
        if (tokenPairs != default)
        {
            tokens.Remove(tokenPairs);
            return true;
        }
        return false;
    }
}