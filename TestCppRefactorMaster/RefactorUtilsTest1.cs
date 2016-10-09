using CppRefactorMaster.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCppRefactorMaster
{
    [TestClass]
    public class RefactorUtilsTest1
    {
        [TestMethod]
        public void TestDeleteParam_On_Off_Commetns()
        {
            Assert.AreEqual(
                "someMethod();\r\n// someMethod()\r\nint someMethod(){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5);\r\n// someMethod(int argument1)\r\nint someMethod(int argument1){\r\n}", "someMethod",
                    "argument1")
                );
        }

        [TestMethod]
        public void TestDeleteParam1()
        {
            Assert.AreEqual(
                "someMethod();\r\nint someMethod(){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5);\r\nint someMethod(int argument1){\r\n}", "someMethod",
                    "argument1")
                );
        }


        [TestMethod]
        public void TestDeleteParam2()
        {
            Assert.AreEqual(
                "someMethod(5);\r\nint someMethod(int argument1){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5, \"hello\");\r\nint someMethod(int argument1, string message){\r\n}", "someMethod", "message")
                );
        }

        [TestMethod]
        public void TestDeleteParam3()
        {
            Assert.AreEqual(
                "someMethod(5, 10, 5.5);\r\nint someMethod(int age, int argument1, double price){\r\n}\r\nint x=0;\r\nsomeMethod(5, 10, 5.5);",
                RefactorUtils.DeleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}\r\nint x=0;\r\nsomeMethod(5, 10, \"hello\", 5.5);", "someMethod",
                    "message")
                );
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "Keyword")]
        public void TestDeleteParam4()
        {
            Assert.AreEqual(
                "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}", "someMethod",
                    "int")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
        "Keyword")]
        public void TestDeleteParam5()
        {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5, 10, \"hello\", 5.5);\r\nint someMethod(int age, int argument1, string message, double price){\r\n}", "someMethod",
                    "int")
                );
        }



        [TestMethod]
        public void TestDeleteParam6()
        {
            Assert.AreEqual(
                "someMethod();\r\nint someMethod(){\r\n}// someMethod(int argument1)",
                RefactorUtils.DeleteParams(
                    "someMethod(5);\r\nint someMethod(int argument1){\r\n}// someMethod(int argument1)", "someMethod",
                    "argument1")
                );
        }

        [TestMethod]
        public void TestDeleteParam7()
        {
            Assert.AreEqual(
                "someMethod();\r\n// someMethod(int argument1)\r\nint someMethod(){\r\n}",
                RefactorUtils.DeleteParams(
                    "someMethod(5);\r\n// someMethod(int argument1)\r\nint someMethod(int argument1){\r\n}", "someMethod",
                    "argument1")
                );
        }

        [TestMethod]
        public void TestDeleteParam8()
        {
            Assert.AreEqual(
                "// someMethod(int argument1)\r\nsomeMethod();\r\nint someMethod(){\r\n}",
                RefactorUtils.DeleteParams(
                    "// someMethod(int argument1)\r\nsomeMethod(5);\r\nint someMethod(int argument1){\r\n}", "someMethod",
                    "argument1")
                );
        }

        [TestMethod]
        public void TestDeleteParam9()
        {
            Assert.AreEqual(
               "someMethod(5);// some comments\r\nint someMethod(int argument1)// there some\r\n{\r\n}",
               RefactorUtils.DeleteParams(
                   "someMethod(5, \"hello\");// some comments\r\nint someMethod(int argument1, string message)// there some\r\n{\r\n}", "someMethod", "message")
               );
        }

        [TestMethod]
        public void TestRenameMethod1()
        {
            Assert.AreEqual(
                "renamedMethod(5, \"hello\", 5.5);\r\nint renamedMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "someMethod", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod2()
        {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "methodName", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod3()
        {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "methodName", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod4()
        {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "methodName", "")
                );
        }

        [TestMethod]
        public void TestRenameMethod5()
        {
            Assert.AreEqual(
                "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod6()
        {
            Assert.AreEqual(
                "renamedMethod(5, \"hello\", 5.5);\r\nint renamedMethod(int age, string message, double price){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, \"hello\", 5.5);\r\nint someMethod(int age, string message, double price){\r\n}",
                    "someMethod", "renamedMethod")
                );
        }

        [TestMethod]
        public void TestRenameMethod7()
        {
            Assert.AreEqual(
                "reamedMethod(5, 10, \"hello\");\r\nint reamedMethod(int age, int argument1, string message = \"someMethod\"){\r\n}",
                RefactorUtils.RenameMethod(
                    "someMethod(5, 10, \"hello\");\r\nint someMethod(int age, int argument1, string message = \"someMethod\"){\r\n}",
                    "someMethod", "reamedMethod")
                );
        }
    }
}