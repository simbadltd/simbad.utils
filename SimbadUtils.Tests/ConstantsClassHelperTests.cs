using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simbad.Utils.Attributes;
using Simbad.Utils.LookupCore;

namespace Simbad.Utils.Tests
{
    [TestClass]
    public class LookupTableHelperTests
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

        internal enum TestEnum
        {
            [System.ComponentModel.Description("TestEnumValue1Description")]
            TestEnumValue1 = 1,

            TestEnumValue2 = 2,

            [System.ComponentModel.Description("TestEnumValue3Description")]
            TestEnumValue3 = 3,
        }

        [TestMethod]
        public void ShouldCreateLookupTableFromEnum()
        {
            // Given
            var targetType = typeof(TestEnum);

            // When
            var lookupTable = targetType.EnumToLookupTable();

            // Then
            Assert.AreEqual("1", lookupTable.GetValueByName("TestEnumValue1Description"));
            Assert.AreEqual("2", lookupTable.GetValueByName("TestEnumValue2"));
            Assert.AreEqual("3", lookupTable.GetValueByName("TestEnumValue3Description"));
        }

        [TestMethod]
        public void ShouldCreateLookupTable()
        {
            // Given
            var targetType = typeof(TestConstantClass);

            // When
            var lookupTable = targetType.ToLookupTable();

            // Then
            Assert.IsTrue(lookupTable.Count == 2);
            Assert.IsNotNull(lookupTable.GetValueByName("TestConstantName1"));
            Assert.IsNotNull(lookupTable.GetValueByName("TestConstantName2"));
            Assert.IsNull(lookupTable.GetValueByName("TestConstantName3"));
            Assert.IsNull(lookupTable.GetValueByName("TestConstantName4"));
            Assert.IsNull(lookupTable.GetValueByName("TestConstantName5"));
        }
    }
}
