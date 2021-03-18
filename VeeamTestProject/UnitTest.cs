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
            FileModel file = new FileModel("файл_04.txt", "sha1", "338f263f56421afd4d1efadd794e21edaf57c18f");
            Assert.AreEqual(Result.OK, Program.CheckFile(file));
        }
    }
}
