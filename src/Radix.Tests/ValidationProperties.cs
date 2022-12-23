using FluentAssertions;
using Radix.Data;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Probability;
using Xunit;
using Xunit.Abstractions;

namespace Radix.Tests
{
    [Validated<Distribution<int>, DV>]
    public partial class Foo { }

    internal class DV : Validity<Distribution<int>>
    {
        public static Func<string, Func<Distribution<int>, Validated<Distribution<int>>>> Validate => throw new NotImplementedException();
    }

    public class ValidationProperties
    {
        private ITestOutputHelper _output;

        public ValidationProperties(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "Can generate validated types for generics")]
        public void Test5()
        {

        }

        [Fact(
            DisplayName =
                "Check will aggregate all failed validations")]
        public void Test4()
        {
            Validated<Person> validationResult =
                Valid(Person.Create)
                    .Apply(
                        Invalid<int>("Age", "Must have a valid age"))
                    .Apply(
                        !string.IsNullOrWhiteSpace("")
                            ? Valid("")
                            : Invalid<string>("First name", "Must have a valid first name"))
                    .Apply(
                        !string.IsNullOrWhiteSpace("Doe")
                            ? Valid("")
                            : Invalid<string>("Last name", "Must have a valid last name"));



            switch (validationResult)
            {
                case Valid<Person> _:
                    Fail();
                    break;
                case Invalid<Person> error:
                    error.Reasons.Select(x => x.Descriptions).Aggregate((current, next) => current.Concat(next).ToArray()).Should().Contain(new[] { "Must have a valid age" });
                    error.Reasons.Select(x => x.Descriptions).Aggregate((current, next) => current.Concat(next).ToArray()).Should().Contain("Must have a valid first name");
                    _output.WriteLine(error.ToString());
                    break;
            }
        }
    }

    public class Person
    {
        public static readonly Func<int, string, string, Person> Create = (age, firstName, lastName)
            => new Person {Age = age, FirstName = firstName, LastName = lastName};


        public int Age { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
