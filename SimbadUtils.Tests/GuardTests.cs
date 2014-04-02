using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simbad.Utils.Tests
{
    [TestClass]
    public class GuardTests
    {
        private class GuardTestClass
        {
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenParameterIsNull()
        {
            //Given
            GuardTestClass tstClass = null;

            //When/Then
            Guard.NotNull(() => tstClass);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowArgumentNullExceptionWhenParameterIsNull2()
        {
            //Given
            GuardTestClass tstClass = null;

            //When/Then
            Guard.NotNull(tstClass, "tstClass");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowArgumentExceptionWhenStringIsNull()
        {
            //Given
            string s = null;

            //When/Then
            Guard.NotNullOrEmpty(() => s);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowArgumentExceptionWhenStringIsEmpty()
        {
            //Given
            string s = string.Empty;

            //When/Then
            Guard.NotNullOrEmpty(() => s);
        }

        [TestMethod]
        public void ShouldNotThrowArgumentExceptionWhenStringIsNotNullAndEmpty()
        {
            //Given
            string s = "123";

            //When/Then
            Guard.NotNullOrEmpty(() => s);
        }

        [TestMethod]
        public void ShouldNotThrowArgumentNullExceptionWhenParameterNotNull()
        {
            //Given
            GuardTestClass tstClass = new GuardTestClass();

            //When/Then
            Guard.NotNull(() => tstClass);
        }

        [TestMethod]
        public void ShouldNotThrowArgumentNullExceptionWhenParameterNotNull2()
        {
            //Given
            GuardTestClass tstClass = new GuardTestClass();

            //When/Then
            Guard.NotNull(tstClass, "tstClass");
        }
    }
}