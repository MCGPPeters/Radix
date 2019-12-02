using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Radix.Tests.Result;
using Radix.Tests.Validated;
using Xunit;
using static Radix.Tests.Validated.Extensions;

namespace Radix.Tests
{
    public class ValidationProperties
    {
        [Fact(
            DisplayName =
                "Check will aggregate all failed validations")]
        public void Test4()
        {
            var validationResult =
                Valid(Person.Create)
                    .Apply(
                        11 >= 18
                            ? Valid(18)
                            : Invalid<int>(new List<string> {"Must have a valid age"}))
                    .Apply(
                        !string.IsNullOrWhiteSpace("")
                            ? Valid("")
                            : Invalid<string>(new List<string> {"Must have a valid first name"}))
                    .Apply(
                        !string.IsNullOrWhiteSpace("Doe")
                            ? Valid("")
                            : Invalid<string>(new List<string> {"Must have a valid last name"}));


            //Func<Person, Person> create2 = _ =>  person;

            //var validationResult2 =
            //    person
            //        .Check(p => p.Age >= 18, _ =>  "Must have a valid age")
            //        .Apply(p => !string.IsNullOrWhiteSpace(p.FirstName), "Must have a first name" )
            //        .Check()

            switch (validationResult)
            {
                case Valid<Person> _:
                    Assert.Fail();
                    break;
                case Invalid<Person> error:
                    error.Reasons.Should().Contain("Must have a valid age", "Must have a valid first name");
                    break;
            }
        }

        private static Validated<Person> XValidated(Person person)
        {

            return person.Age >= 18 
                ? Valid(person) 
                : Invalid<Person>(new List<string>{ "Must have a valid age" });
        }

    }
    
    public class Person
    {
        public static Func<int, string, string, Person> Create = (age, firstName, lastName)
        => new Person
            {
                Age = age,
                FirstName = firstName,
                LastName = lastName
            };
        

        public int Age { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
