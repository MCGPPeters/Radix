using Radix.Data;

namespace Radix.Infrastructure.Azure.Tests
{
    public class UnitTest1
    {
        [Fact(DisplayName = "An invalid name for a namspace should not be accepted")]
        public void Test1()
        {
            var serviceBusNamespace = Data.Names.ServiceBus.Namespace.Create("s&");

            Assert.IsType<Invalid<Data.Names.ServiceBus.Namespace>>(serviceBusNamespace);

            if (serviceBusNamespace is (Invalid<Data.Names.ServiceBus.Namespace> invalidServiceBusNamespace))
            {
                Assert.Equal(3, invalidServiceBusNamespace.Reasons.Length);
            }
        }
    }
}
