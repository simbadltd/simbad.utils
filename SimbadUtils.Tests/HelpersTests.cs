using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simbad.Utils.Attributes;
using Simbad.Utils.Collections;
using Simbad.Utils.Helpers;
using Simbad.Utils.LookupCore;

namespace Simbad.Utils.Tests
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void StringListTest()
        {
            var list = new StringList();
            list.Add("AddedTestItem");
            Assert.AreEqual(list.Contains("AddedTestItem"), true);
            Assert.AreEqual(list.Contains("NotAddedTestItem"), false);

            list.Add(StringList.DEFAULT_ITEMS_SEPARATOR + "InvalidTestItem");
            Assert.AreEqual(list.Contains("InvalidTestItem"), true);
            Assert.AreEqual(list.Contains(StringList.DEFAULT_ITEMS_SEPARATOR + "InvalidTestItem"), false);

            list.Add("AddedTestItem2");
            var result = list.Serialize();
            var expectedResult = "AddedTestItem" +
                                 StringList.DEFAULT_ITEMS_SEPARATOR +
                                 "InvalidTestItem" +
                                 StringList.DEFAULT_ITEMS_SEPARATOR +
                                 "AddedTestItem2";
            Assert.AreEqual(result, expectedResult);

            list.Deserialize("1" +
                             StringList.DEFAULT_ITEMS_SEPARATOR +
                             "2" +
                             StringList.DEFAULT_ITEMS_SEPARATOR +
                             "3" +
                             StringList.DEFAULT_ITEMS_SEPARATOR +
                             "4");
            Assert.AreEqual(list.Count, 4);
        }

        [TestMethod]
        public void StringDictionaryTest()
        {
            var validKey = "validKey";
            var validValue = "validValue";

            var invalidKey = "invalidKey2" + StringDictionary.DEFAULT_VALUES_SEPARATOR +
                             StringDictionary.DEFAULT_ITEMS_SEPARATOR;

            var invalidValue = "invalidValue2" + StringDictionary.DEFAULT_VALUES_SEPARATOR +
                                         StringDictionary.DEFAULT_ITEMS_SEPARATOR;


            var dic = new StringDictionary();
            dic.Add(validKey, validValue);
            Assert.AreEqual(dic[validKey], validValue);

            dic[invalidKey] = invalidValue;
            Assert.AreEqual(dic["invalidKey2"], "invalidValue2");

            var result = dic.Serialize();
            var expectedResult = validKey +
                                 StringDictionary.DEFAULT_VALUES_SEPARATOR +
                                 validValue +
                                 StringDictionary.DEFAULT_ITEMS_SEPARATOR +
                                 "invalidKey2" +
                                 StringDictionary.DEFAULT_VALUES_SEPARATOR +
                                 "invalidValue2";

            Assert.AreEqual(result, expectedResult);

            dic.Deserialize(expectedResult);
            Assert.AreEqual(dic.Count, 2);
        }

        [TestMethod]
        public void TripleTupleListTest()
        {
            var validTag = "validTag";
            var validKey = "validKey";
            var validValue = "validValue";

            var invalidTag = "invalidTag2" + TripleTupleList.DEFAULT_VALUES_SEPARATOR +
                             TripleTupleList.DEFAULT_ITEMS_SEPARATOR;

            var invalidKey = "invalidKey2" + TripleTupleList.DEFAULT_VALUES_SEPARATOR +
                             TripleTupleList.DEFAULT_ITEMS_SEPARATOR;

            var invalidValue = "invalidValue2" + TripleTupleList.DEFAULT_VALUES_SEPARATOR +
                                         TripleTupleList.DEFAULT_ITEMS_SEPARATOR;


            var list = new TripleTupleList();
            list.Add(validTag, validKey, validValue);
            var item = list.First(_ => _.Item1 == validTag && _.Item2 == validKey);
            Assert.AreEqual(item.Item3, validValue);

            list.Add(invalidTag, invalidKey, invalidValue);
            item = list.First(_ => _.Item1 == "invalidTag2" && _.Item2 == "invalidKey2");
            Assert.AreEqual(item.Item3, "invalidValue2");

            var result = list.Serialize();
            var expectedResult = validTag +
                                 TripleTupleList.DEFAULT_VALUES_SEPARATOR +
                                 validKey +
                                 TripleTupleList.DEFAULT_VALUES_SEPARATOR +
                                 validValue +
                                 TripleTupleList.DEFAULT_ITEMS_SEPARATOR +
                                 "invalidTag2" +
                                 TripleTupleList.DEFAULT_VALUES_SEPARATOR +
                                 "invalidKey2" +
                                 TripleTupleList.DEFAULT_VALUES_SEPARATOR +
                                 "invalidValue2";

            Assert.AreEqual(result, expectedResult);

            list.Deserialize(expectedResult);
            Assert.AreEqual(list.Count, 2);
        }

        [TestMethod]
        public void ConstantClassHelperTest()
        {
            var dic = typeof(TestConstantClass).ToLookupTable();
            Assert.AreEqual(dic.Count, 3);
            Assert.AreEqual(dic.Any(_ => _.Name == "CONST1_NAME"), true);
            Assert.AreEqual(dic.Any(_ => _.Name == "CONST2_NAME"), true);
            Assert.AreEqual(dic.Any(_ => _.Name == "CONST3_NAME"), true);
            Assert.AreEqual(dic.First(_ => _.Name == "CONST1_NAME").Value, "CONST1");
            Assert.AreEqual(dic.First(_ => _.Name == "CONST2_NAME").Value, "CONST2");
            Assert.AreEqual(dic.First(_ => _.Name == "CONST3_NAME").Value, "CONST3");
        }

        #region

        internal static class TestConstantClass 
        {
            [StringValue("CONST1_NAME")]
            public const string TEST_CONST1 = "CONST1";

            [StringValue("CONST2_NAME")]
            public const string TEST_CONST2 = "CONST2";

            [StringValue("CONST3_NAME")]
            public const string TEST_CONST3 = "CONST3";
        }

        #endregion
    }
}