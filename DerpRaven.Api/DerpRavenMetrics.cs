using System.Diagnostics;
using System.Diagnostics.Metrics;
namespace DerpRaven.Api;

public class DerpRavenMetrics : IDerpRavenMetrics
{
    public static ActivitySource ActivitySource = new("derp-raven-custom-trace-source");
    private readonly Counter<int> _imageEndpointCalls;
    private readonly Counter<int> _customRequestEndpointCalls;
    private readonly Counter<int> _portfolioEndpointCalls;

    public DerpRavenMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("DerpRaven");
        _imageEndpointCalls = meter.CreateCounter<int>("DerpRaven.ImageEndpointCalls");
        _customRequestEndpointCalls = meter.CreateCounter<int>("DerpRaven.CustomRequestEndpointCalls");
        _portfolioEndpointCalls = meter.CreateCounter<int>("DerpRaven.PortfolioEndpointCalls");
    }

    public void AddImageEndpointCall()
    {
        _imageEndpointCalls.Add(1);
    }

    public void AddCustomRequestEndpointCall()
    {
        _customRequestEndpointCalls.Add(1);
    }

    public void AddPortfolioEndpointCall()
    {
        _portfolioEndpointCalls.Add(1);
    }
}