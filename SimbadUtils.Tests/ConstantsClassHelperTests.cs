using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simbad.Utils.Attributes;
using Simbad.Utils.Helpers;

namespace Simbad.Utils.Tests
{
    [TestClass]
    public class ConstantsClassHelperTests
    {
        internal class TestConstantClass
        {
            [StringValue("TestConstantName1")]
            public const string TestContstant1 = "TestConstantValue1";

            [StringValue("TestConstantName2")]
            public const string TestContstant2 = "TestConstantValue2";

            [StringValue("TestConstantName3")]
            private const string TestContstant3 = "TestConstantValue3";

            [StringValue("TestConstantName4")]
            private static string TestContstant4 = "TestConstantValue4";

            [StringValue("TestConstantName5")]
            public static string TestContstant5 = "TestConstantValue5";
        }

        [TestMethod]
        public void ShouldCreateLookupTable()
        {
            //Given
            var targetType = typeof (TestConstantClass);

            //When
            var lookupTable = targetType.ToLookupTable();

            //Then
            Assert.IsTrue(lookupTable.Count == 2);
            Assert.IsNotNull(lookupTable.GetValueByName("TestConstantName1"));
            Assert.IsNotNull(lookupTable.GetValueByName("TestConstantName2"));
            Assert.IsNull(lookupTable.GetValueByName("TestConstantName3"));
            Assert.IsNull(lookupTable.GetValueByName("TestConstantName4"));
            Assert.IsNull(lookupTable.GetValueByName("TestConstantName5"));
        }
    }
}
