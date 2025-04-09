namespace DerpRaven.Api
{
    public interface IDerpRavenMetrics
    {
        void AddCustomRequestEndpointCall();
        void AddImageEndpointCall();
        void AddPortfolioEndpointCall();
    }
}