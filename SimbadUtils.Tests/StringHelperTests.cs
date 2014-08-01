using Microsoft.VisualStudio.TestTools.UnitTesting;

using Simbad.Utils.Utils;

namespace Simbad.Utils.Tests
{
    [TestClass]
    public class StringHelperTests
    {
        [TestMethod]
        public void MatchWildcardTest()
        {
            // Positive Tests
            Assert.IsTrue(StringUtils.MatchWildcard("*", ""));
            Assert.IsTrue(StringUtils.MatchWildcard("?", " "));
            Assert.IsTrue(StringUtils.MatchWildcard("*", "a"));
            Assert.IsTrue(StringUtils.MatchWildcard("*", "ab"));
            Assert.IsTrue(StringUtils.MatchWildcard("?", "a"));
            Assert.IsTrue(StringUtils.MatchWildcard("*?", "abc"));
            Assert.IsTrue(StringUtils.MatchWildcard("?*", "abc"));
            Assert.IsTrue(StringUtils.MatchWildcard("*abc", "abc"));
            Assert.IsTrue(StringUtils.MatchWildcard("*abc*", "abc"));
            Assert.IsTrue(StringUtils.MatchWildcard("*a*bc*", "aXXXbc"));

            // Negative Tests
            Assert.IsFalse(StringUtils.MatchWildcard("*a", ""));
            Assert.IsFalse(StringUtils.MatchWildcard("a*", ""));
            Assert.IsFalse(StringUtils.MatchWildcard("?", ""));
            Assert.IsFalse(StringUtils.MatchWildcard("*b*", "a"));
            Assert.IsFalse(StringUtils.MatchWildcard("b*a", "ab"));
            Assert.IsFalse(StringUtils.MatchWildcard("??", "a"));
            Assert.IsFalse(StringUtils.MatchWildcard("*?", ""));
            Assert.IsFalse(StringUtils.MatchWildcard("??*", "a"));
            Assert.IsFalse(StringUtils.MatchWildcard("*abc", "abX"));
            Assert.IsFalse(StringUtils.MatchWildcard("*abc*", "Xbc"));
            Assert.IsFalse(StringUtils.MatchWildcard("*a*bc*", "ac"));
        }
    }
}
