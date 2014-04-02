using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simbad.Utils.Helpers;

namespace Simbad.Utils.Tests
{
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void MatchWildcardTest()
        {
            // Positive Tests
            Assert.IsTrue(StringHelper.MatchWildcard("*", ""));
            Assert.IsTrue(StringHelper.MatchWildcard("?", " "));
            Assert.IsTrue(StringHelper.MatchWildcard("*", "a"));
            Assert.IsTrue(StringHelper.MatchWildcard("*", "ab"));
            Assert.IsTrue(StringHelper.MatchWildcard("?", "a"));
            Assert.IsTrue(StringHelper.MatchWildcard("*?", "abc"));
            Assert.IsTrue(StringHelper.MatchWildcard("?*", "abc"));
            Assert.IsTrue(StringHelper.MatchWildcard("*abc", "abc"));
            Assert.IsTrue(StringHelper.MatchWildcard("*abc*", "abc"));
            Assert.IsTrue(StringHelper.MatchWildcard("*a*bc*", "aXXXbc"));

            // Negative Tests
            Assert.IsFalse(StringHelper.MatchWildcard("*a", ""));
            Assert.IsFalse(StringHelper.MatchWildcard("a*", ""));
            Assert.IsFalse(StringHelper.MatchWildcard("?", ""));
            Assert.IsFalse(StringHelper.MatchWildcard("*b*", "a"));
            Assert.IsFalse(StringHelper.MatchWildcard("b*a", "ab"));
            Assert.IsFalse(StringHelper.MatchWildcard("??", "a"));
            Assert.IsFalse(StringHelper.MatchWildcard("*?", ""));
            Assert.IsFalse(StringHelper.MatchWildcard("??*", "a"));
            Assert.IsFalse(StringHelper.MatchWildcard("*abc", "abX"));
            Assert.IsFalse(StringHelper.MatchWildcard("*abc*", "Xbc"));
            Assert.IsFalse(StringHelper.MatchWildcard("*a*bc*", "ac"));
        }
    }
}
