
namespace ERP.Contracts.Domain.Core.Enums
{
    public enum AuthorisationType : int
    {
        Authorized,
        AuthorizeFailed,
        AuthorizeWaitingForRelease,
        AuthorizeBlocked
    }
}
