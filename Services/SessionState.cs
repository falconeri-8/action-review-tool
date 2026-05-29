using Audit.Models;

namespace Audit.Services;

public class SessionState
{
    public User? Current { get; private set; }
    public event Action? Changed;

    public bool IsAuthenticated => Current is not null;

    public void SignIn(User user)
    {
        Current = user;
        Changed?.Invoke();
    }

    public void SignOut()
    {
        Current = null;
        Changed?.Invoke();
    }
}
