using CppRefactorMaster.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCppRefactorMaster {
    [TestClass]
    public class RefactorUtilsTest1 {
        [TestMethod]
        public void TestDeleteParam1() {
            Assert.AreEqual(
                "someMethod();\r\nint someMethod(){\r\n}",
                RefactorUtils.deleteParams(
                    "someMethod(5);\r\nint someMethod(int argument1){\r\n}",
                    "arhument1")
                );
        }

        [TestMethod]
        public void TestDeleteParam2() {
            Assert.AreEqual(
                "someMethod(5);\r\nint someMethod(int argument1){\r\n}",
                RefactorUtils.deleteParams(
                    "someMethod(5, \"hello\");\r\nint someMethod(int argument1, string message){\r\n}", "message")
                );
        }

        [TestMethod]
        public void TestDeleteParam3() {
            Assert.AreEqual(
                "someMethod(5, 5.5);\r\nint someMethod(int age, double price){\r\n}\r\nint x=0;\r\nsomeMethod(5, 5.5);",
                RefactorUtils.deleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}\r\nint x=0;\r\nsomeMethod(5, 10, \"hello\", 5.5);",
                    "message", "argument1")
                );
        }


        [TestMethod]
        public void TestDeleteParam4() {
            Assert.AreEqual(
                "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                RefactorUtils.deleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "int")
                );
        }

        [TestMethod]
        public void TestDeleteParam5() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.deleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "int", "params", "argument1", "out")
                );
        }

        [TestMethod]
        public void TestRenameMethod1() {
            Assert.AreEqual(
                "reamedMethod(5, \"hello\", 5.5);\r\nint reamedMethod(int age, string message, double price){\r\n}",
                RefactorUtils.renameMethod(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "someMethod", "reamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod2() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.renameMethod(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "methodName", "reamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod3() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.renameMethod(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "methodName", "reamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod4() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.renameMethod(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "methodName", "")
                );
        }

        [TestMethod]
        public void TestRenameMethod5() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.renameMethod(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "", "reamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod6() {
            Assert.AreEqual(
                "reamedMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.renameMethod(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                    "someMethod", "reamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod7() {
            Assert.AreEqual(
                "reamedMethod(5, 10, \"hello\");\r\nint reamedMethod(int age, int argument1, string message = \"someMethod\"){\r\n}",
                RefactorUtils.renameMethod(
                    "someMethod(5, 10, \"hello\");\r\nint someMethod(int age, int argument1, string message = \"someMethod\"){\r\n}",
                    "someMethod", "reamedMethod")
                );
        }
    }
}