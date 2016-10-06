using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CppRefactorMaster.Core
{
    public static class RefactorUtils
    {
        // TODO more keywords (read from file)
        private static readonly string[] Keywords = { "if", "for", "while", "switch", "do", "case" };

        private static void LoadKeywordsFromFile(string fileName)
        {
            //            Keywords = File.ReadAllLines(fileName);
        }

        public static string DeleteParams(string source, string methodName, params string[] paramNames)
        {
            // Get a method declaration
            foreach (string param in paramNames)
            {
                if (Keywords.Contains(param))
                {
                    throw new ArgumentException("Keyword");
                }
            }
            if (methodName.Length == 0 || methodName.Length == 0)
            {
                return source;
            }
            // some code there
            StringBuilder code = new StringBuilder(source);
            

            // there someregex
            // regex for taking mathod declaration
            Regex GetMethod = new Regex(@"(" + methodName + @")\s*\((.*)\)\s*\n*\{");

            // regex for taking call method
            Regex GetMethodCall = new Regex(@"(?<=" + methodName + @"\s*\().*(?=\)(;|,|\.))");

            // regex for cooments
            Regex GetCooments = new Regex(@"(\/\*(\n*.*)*\*\/)|(\/\/.*)");

            // regex for deleting parmas from method declaration

            // get code witout comments
            StringBuilder codeNOComm = new StringBuilder(Regex.Replace(code.ToString(), GetCooments.ToString(), String.Empty));
            //Console.WriteLine(codeNOComm.ToString());

            // get all method declaration
            var methodDecl = Regex.Matches(codeNOComm.ToString(), GetMethod.ToString());
            int comaCounter = 0;
            foreach (Match match in methodDecl)
            {
                //Console.WriteLine(match.Groups[2].ToString());
                // get coutn befor deleting params from 
                for (int i = 0; i < match.Groups[2].ToString().Length; i++)
                {
                    if (match.Groups[2].ToString()[i] == ',')
                    {
                        comaCounter++;
                    }
                }
                Console.WriteLine(match.Groups[2].ToString());
            }

            List<int> indexes = new List<int>();

            // there we change declaration
            StringBuilder codeWithChangedDeclarations = codeNOComm;
            foreach (string arg in paramNames)
            {

                // regex for taking inform about deleted args
                Regex GetInfromAboutDel = new Regex(@".*(?=" + arg + @")");
                var deleted = Regex.Matches(codeNOComm.ToString(), GetInfromAboutDel.ToString());
                int counter = 0;
                foreach (Match match in deleted)
                {
                    Console.WriteLine(match.Groups[0].ToString());

                    for (int i = 0; i < match.Groups[0].ToString().Length; i++)
                    {
                        if (match.Groups[0].ToString()[i] == ',')
                        {
                            counter++;
                        }
                    }
                }
                indexes.Add(counter);


                Regex argRegex = new Regex(@"(?<=" + methodName + @")(\s*\w+((\d*|\w*)*)\s*" + arg +
                @"\b\s*(?=\)))|(\s*\w+((\d*|\w*)*)\s*" + arg + @"\b\s*\,\s*)" +
                @"|(\,\s*\w+((\d*|\w*)*)\s*" + arg + @"\b\s*)|(\,\s*\w+((\d*|\w*)*)\s*" +
                arg + @"\b\s*(?=\)))");

                codeWithChangedDeclarations = new StringBuilder(Regex.Replace(codeWithChangedDeclarations.ToString(), argRegex.ToString(), String.Empty));
            }


            Console.WriteLine(codeWithChangedDeclarations.ToString());

            // get all method calls
            var callMethods = Regex.Matches(codeNOComm.ToString(), GetMethodCall.ToString());
            foreach (Match match in callMethods)
            {
                //Console.WriteLine(match.Groups[0].ToString());
                Regex splitter = new Regex(@"(\" + '"' + @".*\" + '"' + @")|(\b[^\,]*(?=\,))|((?<=\,)\b[^\,]*)");
                List<string> words = new List<string>();
                var tempArgs = Regex.Matches(match.Groups[0].ToString(), splitter.ToString());
                foreach (Match temMatch in tempArgs)
                {
                    if (temMatch.Groups[0].ToString() != "")
                    {
                        words.Add(temMatch.Groups[0].ToString());
                        //Console.WriteLine(temMatch.Groups[0].ToString());
                    }
                }
                for (int i = 0; i < indexes.Count; i++)
                {
                    Regex deleteArgsCallMet = new Regex(@"(\s*" + words[indexes[i]] +
                    @"\s*(?=\)))|(\s*" + words[indexes[i]] + @"\s*\,\s*)" +
                    @"|(\,\s*" + words[indexes[i]] + @"\s*)|(\,\s*" +
                    words[indexes[i]] + @"\s*(?=\)))");
                    Console.WriteLine(Regex.Replace(codeWithChangedDeclarations.ToString(),
                        deleteArgsCallMet.ToString(), String.Empty));
                    codeWithChangedDeclarations = new StringBuilder(Regex.Replace(codeWithChangedDeclarations.ToString(),
                        deleteArgsCallMet.ToString(), String.Empty));
                }

                //Console.WriteLine(codeWithChangedDeclarations.ToString());
            };
            return codeWithChangedDeclarations.ToString();
        }

        public static string RenameMethod(string source, string oldMmethodName, string newMethodName)
        {
            if (Keywords.Contains(oldMmethodName)) throw new ArgumentException("Keyword");
            if (oldMmethodName == newMethodName) return source;

            if (oldMmethodName.Length == 0 || newMethodName.Length == 0) return source;

            var code = new StringBuilder(source);
            var offset = 0;
            var matches = Regex.Matches(code.ToString(), @"(" + oldMmethodName + @")\s*\(.*\)");

            // перебор найденных объявлений и вызовов методов
            foreach (Match match in matches)
            {
                var methodName = match.Groups[1].ToString();

                // позиция первого символа найденной подстроки
                var cursor = match.Groups[1].Index - 2 + offset;

                var skip = false;
                var state = RefactorState.MethodLine;

                // проверка принадлежности кода к комментариям
                while (cursor >= 0 && skip == false)
                {
                    var exp = code.ToString(cursor, 2);

                    switch (state)
                    {
                        case RefactorState.MethodLine:

                            switch (exp)
                            {
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

                            switch (exp)
                            {
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

                if (!skip)
                {
                    // переименование метода
                    code.Replace(oldMmethodName, newMethodName, match.Groups[1].Index + offset,
                        oldMmethodName.Length);

                    offset += newMethodName.Length - methodName.Length;
                }

            }

            return code.ToString();
        }

        private enum RefactorState
        {
            MethodLine,
            NewLine
        }
    }
}