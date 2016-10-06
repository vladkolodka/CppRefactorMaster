using CppRefactorMaster.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCppRefactorMaster {
    [TestClass]
    public class RefactorUtilsTest1 {
        [TestMethod]
        public void TestDeleteParam1() {
            Assert.AreEqual(
                "someMethod();\r\nint someMethod(){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5);\r\nint someMethod(int argument1){\r\n}", "someMethod",
                    "argument1")
                );
        }

        [TestMethod]
        public void TestDeleteParam2() {
            Assert.AreEqual(
                "someMethod(5);\r\nint someMethod(int argument1){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5, \"hello\");\r\nint someMethod(int argument1, string message){\r\n}", "someMethod", "message")
                );
        }

        [TestMethod]
        public void TestDeleteParam3() {
            Assert.AreEqual(
                "someMethod(5, 5.5);\r\nint someMethod(int age, double price){\r\n}\r\nint x=0;\r\nsomeMethod(5, 5.5);",
                RefactorUtils.DeleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}\r\nint x=0;\r\nsomeMethod(5, 10, \"hello\", 5.5);", "someMethod",
                    "message", "argument1")
                );
        }


        [TestMethod]
        public void TestDeleteParam4() {
            Assert.AreEqual(
                "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}", "someMethod",
                    "int")
                );
        }

        [TestMethod]
        public void TestDeleteParam5() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}", "someMethod",
                    "int", "params", "argument1", "out")
                );
        }

        [TestMethod]
        public void TestRenameMethod1() {
            Assert.AreEqual(
                "renamedMethod(5, \"hello\", 5.5);\r\nint renamedMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "someMethod", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod2() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "methodName", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod3() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "methodName", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod4() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "methodName", "")
                );
        }

        [TestMethod]
        public void TestRenameMethod5() {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod6() {
            Assert.AreEqual(
                "renamedMethod(5, \"hello\", 5.5);\r\nint renamedMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "someMethod", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod7() {
            Assert.AreEqual(
                "reamedMethod(5, 10, \"hello\");\r\nint reamedMethod(int age, int argument1, string message = \"someMethod\"){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, 10, \"hello\");\r\nint someMethod(int age, int argument1, string message = \"someMethod\"){\r\n}",
                    "someMethod", "reamedMethod")
                );
        }
    }
}