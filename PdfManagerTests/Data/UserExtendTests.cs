using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data.Tests
{
    [TestClass()]
    public class UserExtendTests
    {
        PdfManageModelContainer container;
        [TestInitialize]
        public void Init()
        {
            container = new PdfManageModelContainer();
        }
        [TestCleanup]
        public void Close()
        {
            container.UserSet.RemoveRange(container.UserSet);
            container.SaveChanges();
            container.Dispose();
        }

        [TestMethod()]
        public void ExistUserTest()
        {
            Assert.IsFalse(container.ExistUser("test"));
            var user = container.UserSet.Create();
            user.LastLoginTime = DateTime.Now;
            user.Username = "test";
            user.Password = "test";
            container.UserSet.Add(user);
            container.SaveChanges();
            Assert.IsTrue(container.ExistUser("test"));
        }
        [TestMethod()]
        public void RegisterTest()
        {
            Assert.IsTrue(container.Register("test", "test"));
            Assert.IsFalse(container.Register("test", "test"));
        }

        [TestMethod()]
        public void LoginTest()
        {
            Assert.IsFalse(container.Login("test", "test"));
            Assert.IsTrue(container.Login("admin", "admin123"));
        }
    }
}