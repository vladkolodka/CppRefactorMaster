using CppRefactorMaster.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCppRefactorMaster
{
    [TestClass]
    public class RefactorUnitTest2 {
        [TestMethod]
        public void TestMethod1() {
            Assert.AreEqual(
                "double someMethod(double mass, double height){\n return mass * height * GRAVITATION_CONSTANT;\n}{\r\n}",
                RefactorUtils.MagicNumber(
                    "double someMethod(double mass, double height){\n return mass * height * 9.81;\n}{\r\n}", "9.81",
                    "GRAVITATION_CONSTANT"
                )
            );
        }

        [TestMethod]
        public void TestMethod2() {
            Assert.AreEqual(
                "void someMethod()\n{int k = 5;\nfor (int i = 0; i < VALUE; i++)\nk += VALUE;\nif (k % VALUE == 0)\nk -= 1;\n}",
                RefactorUtils.MagicNumber(
                    "void someMethod()\n{int k = 5;\nfor (int i = 0; i < 4; i++)\nk += 4;\nif (k % 4 == 0)\nk -= 1;\n}",
                    "4", "VALUE"
                ));
        }

        [TestMethod]
        public void TestMethod3() {
            Assert.AreEqual(
                "drawSprite(int k, SCR_X_WIDTH, int s);",
                RefactorUtils.MagicNumber(
                    "drawSprite(int k, 320, int s);", "320", "SCR_X_WIDTH"
                ));
        }

        [TestMethod]
        public void TestMethod4() {
            Assert.AreEqual(
                "double sferaVolume(double radius){\nreturn ((3 / 4) * radius * radius * radius * PI);\n}",
                RefactorUtils.MagicNumber(
                    "double sferaVolume(double radius){\nreturn ((3 / 4) * radius * radius * radius * 3.14);\n}", "3.14",
                    "PI"
                ));
        }

        [TestMethod]
        public void TestMethod5() {
            Assert.AreEqual(
                "double interestRate(int rate, int days, double startSumm){\nreturn (rate * days * startSumm / DAYS_IN_CURR_YEAR) / 100;\n}",
                RefactorUtils.MagicNumber(
                    "double interestRate(int rate, int days, double startSumm){\nreturn (rate * days * startSumm / 365) / 100;\n}",
                    "365", "DAYS_IN_CURR_YEAR"
                ));
        }

        [TestMethod]
        public void TestMetod1() {
            Assert.AreEqual(
                "if (a == 0)\n{\n\tb += 12;\n}",
                RefactorUtils.BlockFormat("if (a == 0)\n{\nb += 12;\n}")
            );
        }

        [TestMethod]
        public void TestMetod2() {
            Assert.AreEqual(
                "for(int i = 0; i<14; i++){\n\tk += i;\n\tk = k + 1;\n}",
                RefactorUtils.BlockFormat("for(int i = 0; i<14; i++){\nk += i;\nk = k + 1;\n}"));
        }

        [TestMethod]
        public void TestMetod3() {
            Assert.AreEqual(
                "for(int k = 5; k<90; k++){\n\tif (k % 2 == 0){\n\t\tk = k + 7;\n\t}\n\treturn 0;\n}",
                RefactorUtils.BlockFormat("for(int k = 5; k<90; k++){\nif (k % 2 == 0){\nk = k + 7;\n}\nreturn 0;\n}")
            );
        }

        [TestMethod]
        public void TestMetod4() {
            Assert.AreEqual(
                "switch(a)\n{\n\tcase 1:\n\ta++;\n\tcase 2:\n\ta++;\n\tcase 3:\n\ta++;\n}",
                RefactorUtils.BlockFormat("switch(a)\n{\ncase 1:\na++;\ncase 2:\na++;\ncase 3:\na++;\n}")
            );
        }

        [TestMethod]
        public void TestMetod5() {
            Assert.AreEqual(
                "for (int i = 0; i < 5; i++)\n{\n\tfor (int j = 0; j < 15; j++)\n\t{\n\t\tcout << '@';\n\t}\n\tcout << endl;\n}",
                RefactorUtils.BlockFormat(
                    "for (int i = 0; i < 5; i++)\n{\nfor (int j = 0; j < 15; j++)\n{\ncout << '@';\n}\ncout << endl;\n}")
            );
        }
    }
}