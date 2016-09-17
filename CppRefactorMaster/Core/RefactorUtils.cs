using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CppRefactorMaster.Core {
    public static class RefactorUtils {
        // TODO more keywords (read from file)
        private static readonly string[] Keywords = {"if", "for", "while", "switch", "do", "case"};

        public static string DeleteParams(string source, string methodName, params string[] paramNames) {
            return null;
        }

        public static string RenameMethod(string source, string oldMmethodName, string newMethodName) {
            if (Keywords.Contains(oldMmethodName)) throw new ArgumentException("Keyword");
            if (oldMmethodName == newMethodName) return source;

            var methodRegex = new Regex(@"(" + oldMmethodName + @")\s*\(.*\)");
            var code = new StringBuilder(source);

            var match = methodRegex.Match(code.ToString());

            // перебор найденных объявлений и вызовов методов
            while (match.Length != 0) {
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
                                    //cursor = 0;
                                    break;
                                case @"\r\n":
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
                                    //cursor = 0;

                                    break;
                                case "*/":
                                    cursor = 0;
                                    break;
                            }

                            break;
                    }
                    cursor--;
                }

                var offset = match.Groups[1].Index;

                if (skip) offset += oldMmethodName.Length;
                else {
                    // переименование метода
                    code.Replace(oldMmethodName, match.Groups[1].ToString(), match.Groups[1].Index,
                        oldMmethodName.Length);

                    offset += newMethodName.Length;
                }

                // новый поиск без учета текущего найденного результата
                match = methodRegex.Match(code.ToString(), offset);
            }

            return code.ToString();
        }

        private enum RefactorState {
            MethodLine,
            NewLine
        }
    }
}