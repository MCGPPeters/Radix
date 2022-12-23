using Radix.Data;
using Xunit.Abstractions;

namespace Radix.Infrastructure.Azure.Tests
{
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
            var serviceBusNamespace = Data.Names.ServiceBus.Namespace.Create("s&");

            Assert.IsType<Invalid<Data.Names.ServiceBus.Namespace>>(serviceBusNamespace);

            if (serviceBusNamespace is Invalid<Data.Names.ServiceBus.Namespace> invalidServiceBusNamespace)
            {
                output.WriteLine(invalidServiceBusNamespace.Reasons.Select(x => x.Descriptions.Aggregate((current, next) => $"{current}{Environment.NewLine}{next}")).Aggregate((current, next) => $"{current}{Environment.NewLine}{next}"));
                Assert.Equal(2, invalidServiceBusNamespace.Reasons.Length);
            }

        }

        [Fact(DisplayName = "An valid name for a namspace should be accepted")]
        public void Test2()
        {
            var serviceBusNamespace = Data.Names.ServiceBus.Namespace.Create("rg-radix");

            Assert.IsType<Valid<Data.Names.ServiceBus.Namespace>>(serviceBusNamespace);

            if (serviceBusNamespace is Valid<Data.Names.ServiceBus.Namespace> (var validServiceBusNamespace))
            {
                string ns = validServiceBusNamespace;
                output.WriteLine($"{validServiceBusNamespace} is a valid name for a servicebus namespace" );
                Assert.Equal(ns, validServiceBusNamespace);
            }
        }
    }
}
