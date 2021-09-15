using FluentAssertions;
using Radix.Validated;
using Xunit;

namespace Radix.Tests
{
    public class ValidationProperties
    {
        [Fact(
            DisplayName =
                "Check will aggregate all failed validations")]
        public void Test4()
        {
            Validated<Person> validationResult =
                Valid(Person.Create)
                    .Apply(
                        Invalid<int>("Must have a valid age"))
                    .Apply(
                        !string.IsNullOrWhiteSpace("")
                            ? Valid("")
                            : Invalid<string>("Must have a valid first name"))
                    .Apply(
                        !string.IsNullOrWhiteSpace("Doe")
                            ? Valid("")
                            : Invalid<string>("Must have a valid last name"));


            switch (validationResult)
            {
                case Valid<Person> _:
                    Fail();
                    break;
                case Invalid<Person> error:
                    error.Reasons.Should().Contain("Must have a valid age", "Must have a valid first name");
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
