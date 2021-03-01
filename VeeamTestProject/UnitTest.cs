using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VeeamTestTask;

namespace VeeamTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void FileNotExist()
        {
            FileModel file = new FileModel("some path", "md5", "12345");
            Assert.AreEqual(Result.NOT_FOUND, Program.CheckFile(file));
        }

        [TestMethod]
        public void WrongHashSum()
        {
            FileModel file = new FileModel("файл_04.txt", "sha1", "12345");
            Assert.AreEqual(Result.FAIL, Program.CheckFile(file));
        }

        [TestMethod]
        public void CorrectHashSum()
        {
            FileModel file = new FileModel("файл_04.txt", "sha1", "M48mP1ZCGv1NHvrdeU4h7a9XwY8");
            Assert.AreEqual(Result.OK, Program.CheckFile(file));
        }
    }
}
