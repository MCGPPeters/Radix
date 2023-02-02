using Radix.Data;
using Radix.Infrastructure.Azure.Data.Names;
using Xunit.Abstractions;

namespace Radix.Infrastructure.Azure.Tests;

public class ServiceBusNamespaceNameTests
{
    private readonly ITestOutputHelper output;

    public ServiceBusNamespaceNameTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact(DisplayName = "An invalid name for a namespace should not be accepted")]
    public void Test1()
    {
        var serviceBusNamespace = Namespace.Create("s&");

        Assert.IsType<Invalid<Namespace>>(serviceBusNamespace);

        if (serviceBusNamespace is Invalid<Namespace> invalidServiceBusNamespace)
        {
            output.WriteLine(invalidServiceBusNamespace.ToString());
            Assert.Equal(2, invalidServiceBusNamespace.Reasons.Length);
        }

    }

    [Fact(DisplayName = "An valid name for a namespace should be accepted")]
    public void Test2()
    {
        var serviceBusNamespace = Namespace.Create("rg-radix");

        Assert.IsType<Valid<Namespace>>(serviceBusNamespace);

        if (serviceBusNamespace is Valid<Namespace> (var validServiceBusNamespace))
        {
            string ns = validServiceBusNamespace;
            output.WriteLine($"{validServiceBusNamespace} is a valid name for a servicebus namespace" );
            Assert.Equal(ns, validServiceBusNamespace);
        }
    }
}
