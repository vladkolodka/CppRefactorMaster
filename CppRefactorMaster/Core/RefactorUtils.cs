using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;

namespace CppRefactorMaster.Core {
    public static class RefactorUtils {
        // TODO more keywords (read from file)
        private static readonly string[] Keywords;

        static RefactorUtils()
        {
            if (File.Exists("KeyWords.txt"))
            {
                Keywords = File.ReadAllLines("KeyWords.txt");
            }
        }

        public static bool OnComments { get; set; } = true;

        public static string DeleteParams(string source, string methodName, string paramName)
        {
            // check for existence of the keywords as a paramName
            if (Keywords.Contains(paramName))
            {
                throw new ArgumentException("Keyword");
            }
            // check for incorrect entered params for refactoring
            if (methodName.Length == 0 || paramName.Length == 0)
            {
                return source;
            }

            // extract hole code into a string bilder
            StringBuilder code = new StringBuilder(source);
            // regex for taking all comments
            Regex GetCooments = new Regex(@"(\/\*(\n*.*)*\*\/)|(\/\/.*)");

            // Dictionary
            Dictionary<int, List<string>> DeclarCalls = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> MethodsCalls = new Dictionary<int, List<string>>();
            List<string> copyMethods = new List<string>();

            List<KeyValuePair<KeyValuePair<int, string>, int>> methodsAfter = new List<KeyValuePair<KeyValuePair<int, string>, int>>();

            StringBuilder workCode;


            if (OnComments == false)
            {
                //workCode = new StringBuilder(Regex.Replace(code.ToString(), GetCooments.ToString(), String.Empty));
                StringBuilder tempCode = new StringBuilder(code.ToString());
                foreach (Match match in Regex.Matches(code.ToString(), GetCooments.ToString()))
                {
                    StringBuilder spacees = new StringBuilder();
                    foreach (char c in match.ToString())
                    {
                        spacees.Append(" ");
                    }
                    tempCode.Replace(match.ToString(), spacees.ToString(), match.Index, match.ToString().Length);
                }
                workCode = new StringBuilder(tempCode.ToString());

            }
            else
            {
                workCode = new StringBuilder(code.ToString());
            }
            List<string> methodsBeforeChanges = new List<string>();
            List<string> methodsDeclarsBeforeCh = new List<string>();
            List<string> MethodsCall = new List<string>();

            // list of global offsets
            List<int> globalOffsetDec = new List<int>();
            List<int> globalOffsetCal = new List<int>();

            List<int> decOffset = new List<int>();
            List<int> calOffset = new List<int>();


            // regex for extracting method signature
            Regex getMethods = new Regex(@"(" + methodName + @")\s*\(.*\)");
            Regex getDeclarMethods = new Regex(@"(" + methodName + @")\s*\(.*(\s*\w+((\d*|\w*)*)*\s*" + paramName + @".*)\)(?=\s*\n*\{|\;)");
            Regex argRegex = new Regex(@"(?<=" + methodName + @"s\*)(\s*\w+((\d*|\w*)*)\s*" + paramName +
            @"\b\s*(?=\)))|(\s*\w+((\d*|\w*)*)\s*" + paramName + @"\b\s*\,\s*)" +
            @"|(\,\s*\w+((\d*|\w*)*)\s*" + paramName + @"\b\s*)|(\,\s*\w+((\d*|\w*)*)\s*" +
            paramName + @"\b\s*(?=\)))");

            var Methods = Regex.Matches(workCode.ToString(), getMethods.ToString());
            var DeclarMethodsColl = Regex.Matches(workCode.ToString(), getDeclarMethods.ToString());
            Console.WriteLine("There are all methods!!!");
            foreach (Match match in Methods)
            {
                methodsBeforeChanges.Add(match.Groups[0].ToString());
                copyMethods.Add(match.Groups[0].ToString());
                Console.WriteLine(match.Groups[0].ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Methods declaration");


            foreach (Match match in DeclarMethodsColl)
            {
                int comaCounter = 0;
                Console.WriteLine(match.Groups[0].ToString());
                methodsDeclarsBeforeCh.Add(match.Groups[0].ToString());
                decOffset.Add(match.Groups[0].Index);
                for (int i = 0; i < match.Groups[0].ToString().Length; i++)
                {
                    if (match.Groups[0].ToString()[i] == ',')
                    {
                        comaCounter++;
                    }
                }
                if (DeclarCalls.ContainsKey(comaCounter) == false)
                {
                    DeclarCalls.Add(comaCounter, new List<string>());
                    DeclarCalls[comaCounter].Add(match.Groups[0].ToString());
                }
                else
                {
                    DeclarCalls[comaCounter].Add(match.Groups[0].ToString());
                }
            }

            Console.WriteLine();
            Console.WriteLine("There all methods calls");
            foreach (KeyValuePair<int, List<string>> pair in DeclarCalls)
            {
                List<string> tempCalls = new List<string>(pair.Value);
                foreach (string str1 in tempCalls)
                {
                    foreach (string str in pair.Value)
                    {
                        if (str == str1)
                        {
                            methodsBeforeChanges.Remove(str1);
                        }
                    }
                }
            }
            foreach (string s in methodsBeforeChanges)
            {
                foreach (Match match in Methods)
                {
                    if (s == match.Groups[0].ToString())
                    {
                        calOffset.Add(match.Groups[0].Index);
                    }
                }
            }
            foreach (string str in methodsBeforeChanges)
            {
                int comaCounter = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == ',')
                    {
                        comaCounter++;
                    }
                }
                if (MethodsCalls.ContainsKey(comaCounter) == false)
                {
                    MethodsCalls.Add(comaCounter, new List<string>());
                    MethodsCalls[comaCounter].Add(str);
                }
                else
                {
                    MethodsCalls[comaCounter].Add(str);
                }
            }
            int listIteratorDec = 0;
            int listIteratorCal = 0;

            foreach (KeyValuePair<int, List<string>> pair1 in DeclarCalls)
            {
                foreach (string stringBuff in pair1.Value)
                {
                    List<string> methodsDeclarsAfterCh = new List<string>();
                    List<string> MethodsCallAfter = new List<string>();
                    // list for the parameters' position needed for deleting
                    List<int> indexes = new List<int>();
                    if (pair1.Key == 0)
                    {
                        globalOffsetDec.Add(0);
                        string tempPair1string = stringBuff;
                        var declarArg = Regex.Matches(tempPair1string, @"(?<=\().*(?=\))");
                        foreach (Match match in declarArg)
                        {
                            globalOffsetDec[listIteratorDec] += match.Groups[0].Length;
                        }
                        methodsDeclarsAfterCh.Add(Regex.Replace(tempPair1string, @"(?<=\().*(?=\))", String.Empty));
                        listIteratorDec++;
                    }
                    else
                    {
                        // regex for taking inform about deleted args
                        Regex GetInfromAboutDel = new Regex(@"(?<=" + methodName + @"\s*\().*(?=" + paramName + @")");

                        foreach (string str in methodsDeclarsBeforeCh)
                        {
                            string temp = str;
                            methodsDeclarsAfterCh.Add(Regex.Replace(temp, argRegex.ToString(), String.Empty));
                            // get coolection of substring without 1 of parameters needed for deleting  
                            var deleted = Regex.Matches(temp, GetInfromAboutDel.ToString());
                            var declarMatch = Regex.Matches(temp, argRegex.ToString());

                            globalOffsetDec.Add(0);
                            foreach (Match match in declarMatch)
                            {
                                // inccrease value of the offset
                                globalOffsetDec[listIteratorDec] += match.Groups[0].Length;
                                Console.WriteLine(match.Groups[0].ToString());
                            }
                            listIteratorDec++;
                            // counter for counting meetting comaas
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
                            // add parameter's position wich will be deleted 
                            indexes.Add(counter);
                        }
                        // counter for looking throw the list's offsets
                    }
                    foreach (KeyValuePair<int, List<string>> pair2 in MethodsCalls)
                    {
                        if (pair1.Key != pair2.Key)
                        {
                            continue;
                        }
                        else
                        {
                            if (pair2.Key == 0)
                            {
                                foreach (string str in pair2.Value)
                                {
                                    globalOffsetCal.Add(0);
                                    string temp = str;
                                    var tempMatch = Regex.Matches(temp, @"(?<=\().*(?=\))");
                                    foreach (Match match in tempMatch)
                                    {
                                        globalOffsetCal[listIteratorCal] += match.Groups[0].Length;
                                    }
                                    listIteratorCal++;
                                    MethodsCallAfter.Add(Regex.Replace(temp, @"(?<=\().*(?=\))", String.Empty));
                                }
                            }
                            else
                            {

                                foreach (string str in pair2.Value)
                                {
                                    string temp = str;
                                    globalOffsetCal.Add(0);
                                    Regex args = new Regex("(?<=" + methodName + @"\s*\().*(?=\))");
                                    var argsCall = Regex.Matches(temp, args.ToString());
                                    StringBuilder argsTemp = new StringBuilder();
                                    foreach (Match match in argsCall)
                                    {
                                        argsTemp.Append(match.Groups[0].ToString());
                                    }
                                    // regex for extracting params in a separete way
                                    Regex splitter = new Regex(@"(\" + '"' + @".*\" +
                                        '"' + @")|(\b[^\,]*(?=\,))|((?<=\,)\b[^\,]*)");
                                    // list for the parameters of the method's call
                                    List<string> words = new List<string>();

                                    // get a collection of  the parameters of the method's call
                                    var tempArgs = Regex.Matches(argsTemp.ToString(), splitter.ToString());
                                    foreach (Match temMatch in tempArgs)
                                    {
                                        if (temMatch.Groups[0].ToString() != "")
                                        {
                                            words.Add(temMatch.Groups[0].ToString());
                                        }
                                    }
                                    for (int i = 0; i < indexes.Count; i++)
                                    {
                                        // regex for extracting needed data 
                                        Regex deleteArgsCallMet = new Regex(@"(\s*" + words[indexes[i]] +
                                        @"\s*(?=\)))|(\s*" + words[indexes[i]] + @"\s*\,\s*)" +
                                        @"|(\,\s*" + words[indexes[i]] + @"\s*)|(\,\s*" +
                                        words[indexes[i]] + @"\s*(?=\)))");

                                        // get coollection of extracted
                                        var deletedArgs = Regex.Matches(temp, deleteArgsCallMet.ToString());
                                        foreach (Match tempMatch in deletedArgs)
                                        {
                                            // increase offset's value
                                            globalOffsetCal[listIteratorCal] += tempMatch.Groups[0].Length;
                                        }
                                        // delete params
                                        MethodsCallAfter.Add((Regex.Replace(temp.ToString(),
                                            deleteArgsCallMet.ToString(), String.Empty)));
                                    }
                                    // go ahead in the list
                                    listIteratorCal++;
                                }
                                Console.WriteLine();
                                Console.WriteLine("There some changings");
                                foreach (string str in MethodsCallAfter)
                                {
                                    Console.WriteLine(str);
                                }
                                foreach (string str in methodsDeclarsAfterCh)
                                {
                                    Console.WriteLine(str);
                                }
                            }
                        }
                        if (OnComments == true)
                        {
                            foreach (string str1 in pair2.Value)
                            {
                                foreach (string str2 in MethodsCallAfter)
                                {
                                    workCode.Replace(str1, str2);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < MethodsCallAfter.Count; i++)
                            {
                                methodsAfter.Add(new KeyValuePair<KeyValuePair<int, string>, int>
                                    (new KeyValuePair<int, string>(calOffset[i], MethodsCallAfter[i].ToString()),
                                    globalOffsetCal[i]));
                            }
                        }

                    }

                    if (OnComments == true)
                    {
                        foreach (string str1 in methodsDeclarsAfterCh)
                        {
                            workCode.Replace(stringBuff, str1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < methodsDeclarsAfterCh.Count; i++)
                        {
                            methodsAfter.Add(new KeyValuePair<KeyValuePair<int, string>, int>
                                    (new KeyValuePair<int, string>(decOffset[i], methodsDeclarsAfterCh[i].ToString()),
                                    globalOffsetDec[i]));
                        }
                    }
                }
            }
            int addedOfsset = 0;
            if (OnComments == false)
            {
                methodsAfter.Sort(delegate (KeyValuePair<KeyValuePair<int, string>, int> pair1,
                    KeyValuePair<KeyValuePair<int, string>, int> pair2)
                {
                    return pair1.Key.Key.CompareTo(pair2.Key.Key);
                });
                int iterator = 0;
                foreach (KeyValuePair<KeyValuePair<int, string>, int> pair in methodsAfter.Distinct())
                {
                    code.Replace(copyMethods[iterator],
                        pair.Key.Value,
                        pair.Key.Key - addedOfsset, copyMethods[iterator].Length);
                    iterator++;
                    addedOfsset += pair.Value;

                }
                return code.ToString();
            }
            return workCode.ToString();
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
                var cursor = match.Groups[1].Index - 2 + offset;

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