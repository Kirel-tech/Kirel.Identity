using Kirel.Identity.Exceptions;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.Infrastructure.Models;

namespace Kirel.Identity.Server.Infrastructure.Providers;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Provider for 4-digit code tokens
/// </summary>
public class DisposableCodeTokenProvider : Dictionary<Guid, Dictionary<string, List<CodeToken>>>, 
    IUserTwoFactorTokenProvider<User>
{
    private readonly DisposableCodesConfig _codeTokenCfg;
    private readonly Dictionary<Guid, DateTime> _userCooldown;
    private readonly Dictionary<Guid, Dictionary<string, List<(string Code, Timer DisposeTimer)>>> _disposeTimers;

    /// <summary>
    /// Constructor for DisposableCodeTokenProvider
    /// </summary>
    /// <param name="cfg">Disposable codes config</param>
    public DisposableCodeTokenProvider(DisposableCodesConfig cfg)
    {
        _userCooldown = new Dictionary<Guid, DateTime>();
        _disposeTimers = new Dictionary<Guid, Dictionary<string, List<(string Code, Timer DisposeTimer)>>>();
        _codeTokenCfg = cfg;
    }
    
    /// <summary>
    /// Generates 4-digit codes
    /// </summary>
    /// <param name="purpose">Generation purpose</param>
    /// <param name="manager">Identity user manager</param>
    /// <param name="user">User manager</param>
    /// <exception cref="KirelUnauthorizedException">If code generation on cooldown</exception>
    public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
    {
        if (_userCooldown.TryGetValue(user.Id, out var expirationTime))
        {
            if (expirationTime > DateTime.Now)
                throw new KirelUnauthorizedException("Code generation on cooldown");
        }

        var rng = new Random();
        var code = rng.Next(1000, 9999).ToString();
        while (code.Distinct().Count() != 3)
        {
            code = rng.Next(1000, 9999).ToString();
        }

        StoreCodeToken(user.Id, purpose, code);
        _userCooldown[user.Id] = DateTime.Now.Add(TimeSpan.FromMinutes(1));
        SetCodeDisposeTimer(user.Id, purpose, code);
        return Task.FromResult(code);
    }

    private void SetCodeDisposeTimer(Guid userId, string purpose, string code)
    {
        var userCodeData = new Dictionary<Guid, (string, string)>();
        userCodeData.Add(userId, (purpose, code));
        var timer = new Timer(DisposeCodeToken, userCodeData, TimeSpan.FromMinutes(_codeTokenCfg.ExpirationTime),
            TimeSpan.Zero);
        if (!_disposeTimers.TryGetValue(userId, out var userStorage))
        {
            userStorage = new Dictionary<string, List<(string Code, Timer DisposeTimer)>>();
            _disposeTimers[userId] = userStorage;
        }
        if (!userStorage.TryGetValue(purpose, out var userTokens))
        {
            userTokens = new List<(string Code, Timer DisposeTimer)>();
            userStorage[purpose] = userTokens;
        }
        userTokens.Add((code,timer));
    }

    private void DisposeCodeToken(object? obj)
    {
        var userCodeData = (Dictionary<Guid, (string, string)>)obj!;
        var userData = userCodeData.First();

        var userId = userData.Key;
        var (purpose, code) = userData.Value;
        if (!_disposeTimers.TryGetValue(userId, out var storage))
        {
            return;
        }
        if (!storage.TryGetValue(purpose, out var timersStorage))
        {
            return;
        }

        var (codeToken, timer) = timersStorage.FirstOrDefault(t => t.Code == code);
        if(codeToken == null) return;
        
        if (!TryGetValue(userId, out var codeStorage))
        {
            return;
        }

        if (!codeStorage.TryGetValue(purpose, out var userTokens))
        {
            return;
        }

        var tokenPairs =
            userTokens.FirstOrDefault(t => t.Code == code);

        if (tokenPairs == default) return;

        userTokens.Remove(tokenPairs);

        timer.Dispose();
    }

    private void StoreCodeToken(Guid userId, string purpose, string code)
    {
        var codeToken = new CodeToken()
        {
            Code = code,
        };
        if (!TryGetValue(userId, out var userStorage))
        {
            userStorage = new Dictionary<string, List<CodeToken>>();
            this[userId] = userStorage;
        }

        if (!userStorage.TryGetValue(purpose, out var userTokens))
        {
            userTokens = new List<CodeToken>();
            userStorage[purpose] = userTokens;
        }
        userTokens.Add(codeToken);
    }

    /// <summary>
    /// Validates given 4-digit code
    /// </summary>
    /// <param name="purpose">Generation purpose</param>
    /// <param name="code">Given 4-digit code</param>
    /// <param name="manager">Identity user manager</param>
    /// <param name="user">Identity user</param>
    public Task<bool> ValidateAsync(string purpose, string code, UserManager<User> manager, User user)
    {
        var userId = user.Id;
        
        if (!TryGetValue(userId, out var storage))
        {
            return Task.FromResult(false);
        }

        if (!storage.TryGetValue(purpose, out var userTokens))
        {
            return Task.FromResult(false);
        }

        var tokenPairs =
            userTokens.FirstOrDefault(t => t.Code == code);

        if (tokenPairs == default) return Task.FromResult(false);
        
        userTokens.Remove(tokenPairs);
        return Task.FromResult(true);
    }

    /// <summary>
    /// Checks if code can be generated for given user
    /// </summary>
    /// <param name="manager">Identity user manager</param>
    /// <param name="user">Identity user</param>
    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
    {
        return Task.FromResult(user.PhoneNumberConfirmed);
    }
}