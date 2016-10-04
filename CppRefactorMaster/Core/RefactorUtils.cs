using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CppRefactorMaster.Core {
    public static class RefactorUtils {
        // TODO more keywords (read from file)
        private static readonly string[] Keywords = {"if", "for", "while", "switch", "do", "case"};

        private static void LoadKeywordsFromFile(string fileName) {
            //            Keywords = File.ReadAllLines(fileName);
        }

        public static string DeleteParams(string source, string methodName, params string[] paramNames) {
            return null;
        }

        public static string RenameMethod(string source, string oldMmethodName, string newMethodName) {
            if (Keywords.Contains(oldMmethodName)) throw new ArgumentException("Keyword");
            if (oldMmethodName == newMethodName) return source;

            if (oldMmethodName.Length == 0 || newMethodName.Length == 0) return source;

            var code = new StringBuilder(source);
            var offset = 0;
            var matches = Regex.Matches(code.ToString(), @"(" + oldMmethodName + @")\s*\(.*\)");

            // перебор найденных объявлений и вызовов методов
            foreach (Match match in matches) {
                var methodName = match.Groups[1].ToString();

                // позиция первого символа найденной подстроки
                var cursor = match.Groups[1].Index - 2;

                var skip = false;
                var state = RefactorState.MethodLine;

                // проверка принадлежности кода к комментариям
                while (cursor >= 0 && skip == false) {
                    var exp = code.ToString(cursor, 2);

                    switch (state) {
                        case RefactorState.MethodLine:

                            switch (exp) {
                                case "//":
                                case "/*":
                                    skip = true;
                                    break;
                                case "\r\n":
                                    state = RefactorState.NewLine;
                                    break;
                                case "*/":
                                    cursor = 0;
                                    break;
                            }

                            break;
                        case RefactorState.NewLine:

                            switch (exp) {
                                case "/*":
                                    skip = true;
                                    break;
                                case "*/":
                                    cursor = 0;
                                    break;
                            }

                            break;
                    }
                    cursor--;
                }

                if (!skip) {
                    // переименование метода
                    code.Replace(oldMmethodName, newMethodName, match.Groups[1].Index + offset,
                        oldMmethodName.Length);

                    offset += newMethodName.Length - methodName.Length;
                }

            }

            return code.ToString();
        }

        private enum RefactorState {
            MethodLine,
            NewLine
        }
    }
}