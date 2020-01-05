using insurranceDemo.Controllers.DataHelper;
using InsurranceDemo.Controllers;
using InsurranceDemo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CustomUnitTest
    {
        /// <summary>
        /// ���ժ��פ����T�������Ҧr��
        /// </summary>
        [TestMethod]
        public void Test_GetValidCustom_With_ShortID()
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

        /// <summary>
        /// �ˬd�����Ҧr�����r���S���j�p�g
        /// </summary>
        [TestMethod]
        public void Test_GetValidCustom_Without_LetterBegin()
        {
            /// Arrange
            string identity = "0123456789";
            string name = "test1";
            bool sex = true;

            /// Act
            Custom custom = CustomDataHelper.getValidCustom(name, sex, identity);


            /// Assert
            Assert.AreEqual(custom, null);
        }

        /// <summary>
        /// �ˬd�����Ҧr�����r���S���j�p�g
        /// </summary>
        [TestMethod]
        public void Test_GetValidCustom_With_WrongmSex_Male()
        {
            // ���ըk�ͪ������Ҧr���}�Y����1

            /// Arrange
            string identity = "A223456789";
            string name = "test1";
            bool sex = true;

            /// Act
            Custom custom = CustomDataHelper.getValidCustom(name, sex, identity);


            /// Assert
            Assert.AreEqual(custom, null);
        }

        [TestMethod]

        public void Test_GetValidCustom_With_ValidIdentity()
        {
            /// Arrange
            string identity = "A123456789";
            string name = "test1";
            bool sex = true;

            /// Act
            Custom custom = CustomDataHelper.getValidCustom(name, sex, identity);

            /// Assert
            Assert.IsTrue(custom != null);
        }
    }
}
