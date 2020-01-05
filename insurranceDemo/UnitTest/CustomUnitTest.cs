using insurranceDemo.Controllers.DataHelper;
using InsurranceDemo.Controllers;
using InsurranceDemo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CustomUnitTest
    {
        [TestMethod]
        public void Test_AddNewCustom_WithWrongID()
        {

            /// Arrange
            string identity = "123456";
            string name = "test1";
            bool sex = true;

            /// Act
            Custom custom = CustomDataHelper.getValidCustom(name, sex, identity);


            /// Assert
            Assert.AreEqual(custom, null);
        }
    }
}
