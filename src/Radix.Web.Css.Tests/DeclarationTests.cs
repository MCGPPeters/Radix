using Radix.Web.Css.Data;
using Radix.Web.Css.Data.Dimensions;
using Radix.Web.Css.Data.Units.Length.Absolute;

namespace Radix.Web.Css.Tests
{
    public class DeclarationTests
    {
        [Fact(DisplayName = "Declarations should be properly serialized")]
        public void Test1()
        {
            var paddingLeft = new Data.Declarations.PaddingLeft.Length
            {
                Value = new AbsoluteLength<px>((Number)22)
            };

            Assert.Equal("padding-left: 22px", paddingLeft.ToString());
        }
    }
}
