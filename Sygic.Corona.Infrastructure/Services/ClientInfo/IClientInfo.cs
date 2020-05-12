namespace Sygic.Corona.Infrastructure.Services.ClientInfo
{
    public interface IClientInfo
    {
        AppClientInfo Parse(string input);
    }
}