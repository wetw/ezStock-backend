using System.Collections.Generic;
using WE.Application.Extensions;
using Xunit;
using System.Linq;

namespace WE.Application.Tests.Extensions
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void IsMultiple_StateUnderTest_ExpectedBehavior()
        {
            var list = new[] { 1, 2, 3, 1, 4, 2 };
            var duplicateItems = list.Duplicates();
            Assert.Equal(4, duplicateItems.Count());
        }

        private IEnumerable<string> GetStrings(int size = 10)
        {
            for (var i = 0; i < size; i++)
                yield return $"{i}";
            yield return null;
        }
    }
}
