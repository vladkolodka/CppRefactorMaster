using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CppRefactorMaster.Core {
    public static class RefactorUtils {
        // TODO more keywords (read from file)
        private static readonly string[] Keywords;

        static RefactorUtils() {
            if (File.Exists("KeyWords.txt"))
                Keywords = File.ReadAllLines("KeyWords.txt");
        }

        public static bool OnComments { get; set; } = false;

        public static string DeleteParams(string source, string methodName, string paramName) {
            // check for existence of the keywords as a paramName
            if (Keywords.Contains(paramName) || Keywords.Contains(methodName))
                throw new ArgumentException("Keyword");
            // check for incorrect entered params for refactoring
            if ((methodName.Length == 0) || (paramName.Length == 0))
                return source;

            // extract hole code into a string bilder
            var code = new StringBuilder(source);
            // regex for taking all comments
            var GetCooments = new Regex(@"(\/\*(\n*.*)*\*\/)|(\/\/.*)");

            // Dictionary
            var DeclarCalls = new Dictionary<int, List<string>>();
            var MethodsCalls = new Dictionary<int, List<string>>();
            var copyMethods = new List<string>();

            var methodsAfter = new List<KeyValuePair<KeyValuePair<int, string>, int>>();

            StringBuilder workCode;


            if (OnComments == false) {
                //workCode = new StringBuilder(Regex.Replace(code.ToString(), GetCooments.ToString(), String.Empty));
                var tempCode = new StringBuilder(code.ToString());
                foreach (Match match in Regex.Matches(code.ToString(), GetCooments.ToString())) {
                    var spacees = new StringBuilder();
                    foreach (var c in match.ToString())
                        spacees.Append(" ");
                    tempCode.Replace(match.ToString(), spacees.ToString(), match.Index, match.ToString().Length);
                }
                workCode = new StringBuilder(tempCode.ToString());
            }
            else {
                workCode = new StringBuilder(code.ToString());
            }
            var methodsBeforeChanges = new List<string>();
            var methodsDeclarsBeforeCh = new List<string>();
            var MethodsCall = new List<string>();

            // list of global offsets
            var globalOffsetDec = new List<int>();
            var globalOffsetCal = new List<int>();

            var decOffset = new List<int>();
            var calOffset = new List<int>();


            // regex for extracting method signature
            var getMethods = new Regex(@"(" + methodName + @")\s*\(.*\)");
            var getDeclarMethods =
                new Regex(@"(?<=\w+((\d*|\w*)*)*\s+)(" + methodName + @")\s*\(.*" + paramName + @"\b.*\)");
            var argRegex = new Regex(@"(?<=" + methodName + @"s\*)(\s*\w+((\d*|\w*)*)\s*" + paramName +
                                     @"\b\s*(?=\)))|(\s*\w+((\d*|\w*)*)\s*" + paramName + @"\b\s*\,\s*)" +
                                     @"|(\,\s*\w+((\d*|\w*)*)\s*" + paramName + @"\b\s*)|(\,\s*\w+((\d*|\w*)*)\s*" +
                                     paramName + @"\b\s*(?=\)))");

            var Methods = Regex.Matches(workCode.ToString(), getMethods.ToString());
            foreach (Match match in Methods) {
                methodsBeforeChanges.Add(match.Groups[0].ToString());
                copyMethods.Add(match.Groups[0].ToString());
            }

            var DeclarMethodsColl = Regex.Matches(workCode.ToString(), getDeclarMethods.ToString());
            foreach (Match match in DeclarMethodsColl) {
                var comaCounter = 0;
                Console.WriteLine(match.Groups[0].ToString());
                methodsDeclarsBeforeCh.Add(match.Groups[0].ToString());
                decOffset.Add(match.Groups[0].Index);
                for (var i = 0; i < match.Groups[0].ToString().Length; i++)
                    if (match.Groups[0].ToString()[i] == ',')
                        comaCounter++;
                if (DeclarCalls.ContainsKey(comaCounter) == false) {
                    DeclarCalls.Add(comaCounter, new List<string>());
                    DeclarCalls[comaCounter].Add(match.Groups[0].ToString());
                }
                else {
                    DeclarCalls[comaCounter].Add(match.Groups[0].ToString());
                }
            }

            foreach (var pair in DeclarCalls) {
                var tempCalls = new List<string>(pair.Value);
                foreach (var str1 in tempCalls)
                    foreach (var str in pair.Value)
                        if (str == str1)
                            methodsBeforeChanges.Remove(str1);
            }

            foreach (var s in methodsBeforeChanges)
                foreach (Match match in Methods)
                    if (s == match.Groups[0].ToString())
                        calOffset.Add(match.Groups[0].Index);

            foreach (var str in methodsBeforeChanges) {
                var comaCounter = 0;
                for (var i = 0; i < str.Length; i++)
                    if (str[i] == ',')
                        comaCounter++;
                if (MethodsCalls.ContainsKey(comaCounter) == false) {
                    MethodsCalls.Add(comaCounter, new List<string>());
                    MethodsCalls[comaCounter].Add(str);
                }
                else {
                    MethodsCalls[comaCounter].Add(str);
                }
            }
            var listIteratorDec = 0;
            var listIteratorCal = 0;

            foreach (var pair1 in DeclarCalls)
                foreach (var stringBuff in pair1.Value) {
                    var methodsDeclarsAfterCh = new List<string>();
                    var MethodsCallAfter = new List<string>();
                    // list for the parameters' position needed for deleting
                    var indexes = new List<int>();
                    if (pair1.Key == 0) {
                        globalOffsetDec.Add(0);
                        var tempPair1string = stringBuff;
                        var declarArg = Regex.Matches(tempPair1string, @"(?<=\().*(?=\))");
                        foreach (Match match in declarArg)
                            globalOffsetDec[listIteratorDec] += match.Groups[0].Length;
                        methodsDeclarsAfterCh.Add(Regex.Replace(tempPair1string, @"(?<=\().*(?=\))", string.Empty));
                        listIteratorDec++;
                    }
                    else {
                        // regex for taking inform about deleted args
                        var GetInfromAboutDel = new Regex(@"(?<=" + methodName + @"\s*\().*(?=" + paramName + @")");

                        foreach (var str in methodsDeclarsBeforeCh) {
                            var temp = str;
                            methodsDeclarsAfterCh.Add(Regex.Replace(temp, argRegex.ToString(), string.Empty));
                            // get coolection of substring without 1 of parameters needed for deleting  
                            var deleted = Regex.Matches(temp, GetInfromAboutDel.ToString());
                            var declarMatch = Regex.Matches(temp, argRegex.ToString());

                            globalOffsetDec.Add(0);
                            foreach (Match match in declarMatch) {
                                // inccrease value of the offset
                                globalOffsetDec[listIteratorDec] += match.Groups[0].Length;
                                Console.WriteLine(match.Groups[0].ToString());
                            }
                            listIteratorDec++;
                            // counter for counting meetting comaas
                            var counter = 0;
                            foreach (Match match in deleted) {
                                Console.WriteLine(match.Groups[0].ToString());

                                for (var i = 0; i < match.Groups[0].ToString().Length; i++)
                                    if (match.Groups[0].ToString()[i] == ',')
                                        counter++;
                            }
                            // add parameter's position wich will be deleted 
                            indexes.Add(counter);
                        }
                        // counter for looking throw the list's offsets
                    }
                    foreach (var pair2 in MethodsCalls) {
                        if (pair1.Key != pair2.Key)
                            continue;
                        if (pair2.Key == 0) {
                            foreach (var str in pair2.Value) {
                                globalOffsetCal.Add(0);
                                var temp = str;
                                var tempMatch = Regex.Matches(temp, @"(?<=\().*(?=\))");
                                foreach (Match match in tempMatch)
                                    globalOffsetCal[listIteratorCal] += match.Groups[0].Length;
                                listIteratorCal++;
                                MethodsCallAfter.Add(Regex.Replace(temp, @"(?<=\().*(?=\))", string.Empty));
                            }
                        }
                        else {
                            foreach (var str in pair2.Value) {
                                var temp = str;
                                globalOffsetCal.Add(0);
                                var args = new Regex("(?<=" + methodName + @"\s*\().*(?=\))");
                                var argsCall = Regex.Matches(temp, args.ToString());
                                var argsTemp = new StringBuilder();
                                foreach (Match match in argsCall)
                                    argsTemp.Append(match.Groups[0]);
                                // regex for extracting params in a separete way
                                var splitter = new Regex(@"(\" + '"' + @".*\" +
                                                         '"' + @")|(\b[^\,]*(?=\,))|((?<=\,)\b[^\,]*)");
                                // list for the parameters of the method's call
                                var words = new List<string>();

                                // get a collection of  the parameters of the method's call
                                var tempArgs = Regex.Matches(argsTemp.ToString(), splitter.ToString());
                                foreach (Match temMatch in tempArgs)
                                    if (temMatch.Groups[0].ToString() != "")
                                        words.Add(temMatch.Groups[0].ToString());
                                for (var i = 0; i < indexes.Count; i++) {
                                    // regex for extracting needed data 
                                    var deleteArgsCallMet = new Regex(@"(\s*" + words[indexes[i]] +
                                                                      @"\s*(?=\)))|(\s*" + words[indexes[i]] +
                                                                      @"\s*\,\s*)" +
                                                                      @"|(\,\s*" + words[indexes[i]] + @"\s*)|(\,\s*" +
                                                                      words[indexes[i]] + @"\s*(?=\)))");

                                    // get coollection of extracted
                                    var deletedArgs = Regex.Matches(temp, deleteArgsCallMet.ToString());
                                    foreach (Match tempMatch in deletedArgs)
                                        globalOffsetCal[listIteratorCal] += tempMatch.Groups[0].Length;
                                    // delete params
                                    MethodsCallAfter.Add(Regex.Replace(temp,
                                        deleteArgsCallMet.ToString(), string.Empty));
                                }
                                // go ahead in the list
                                listIteratorCal++;
                            }
                            Console.WriteLine();
                            Console.WriteLine("There some changings");
                            foreach (var str in MethodsCallAfter)
                                Console.WriteLine(str);
                            foreach (var str in methodsDeclarsAfterCh)
                                Console.WriteLine(str);
                        }
                        if (OnComments)
                            foreach (var str1 in pair2.Value)
                                foreach (var str2 in MethodsCallAfter)
                                    workCode.Replace(str1, str2);
                        else
                            for (var i = 0; i < MethodsCallAfter.Count; i++)
                                methodsAfter.Add(new KeyValuePair<KeyValuePair<int, string>, int>
                                (new KeyValuePair<int, string>(calOffset[i], MethodsCallAfter[i]),
                                    globalOffsetCal[i]));
                    }

                    if (OnComments)
                        foreach (var str1 in methodsDeclarsAfterCh)
                            workCode.Replace(stringBuff, str1);
                    else
                        for (var i = 0; i < methodsDeclarsAfterCh.Count; i++)
                            methodsAfter.Add(new KeyValuePair<KeyValuePair<int, string>, int>
                            (new KeyValuePair<int, string>(decOffset[i], methodsDeclarsAfterCh[i]),
                                globalOffsetDec[i]));
                }
            var addedOfsset = 0;
            if (OnComments == false) {
                methodsAfter.Sort(delegate(KeyValuePair<KeyValuePair<int, string>, int> pair1,
                    KeyValuePair<KeyValuePair<int, string>, int> pair2) {
                    return pair1.Key.Key.CompareTo(pair2.Key.Key);
                });
                var iterator = 0;
                foreach (var pair in methodsAfter.Distinct()) {
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

            if ((oldMmethodName.Length == 0) || (newMethodName.Length == 0)) return source;

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
                while ((cursor >= 0) && (skip == false)) {
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


        public static string MagicNumber(string code, string number, string nameConst) {
            var str = new StringBuilder(code);
            //            var fFloat = nameConst.Any(t => t == '.');

            str.Replace(number, nameConst);


            /*if (fFloat == false)
                str.Insert(0, "private const int " + nameConst + " = " + number + ";\n");
            else
                str.Insert(0, "private const float " + nameConst + " = " + number + ";\n");*/

            return str.ToString();
        }

        public static string BlockFormat(string code) {
            var counter = 0;
            // index count of \tb
            var insertPair = new List<KeyValuePair<int, int>>();
            var globalOffset = 0;
            for (var i = 0; i < code.Length; i++) {
                switch (code[i]) {
                    case '{':
                        counter++;
                        continue;
                    case '}':
                        counter--;
                        continue;
                }
                if ((counter <= 0) || (code[i] != '\n')) continue;
                insertPair.Add(code[i + 1] != '}'
                    ? new KeyValuePair<int, int>(i + 1, counter)
                    : new KeyValuePair<int, int>(i + 1, counter - 1));
            }
            foreach (var t in insertPair) {
                var tempStr = code;
                var insStr = new StringBuilder();
                insStr.Append('\t', t.Value);
                code = tempStr.Insert(t.Key + globalOffset, insStr.ToString());
                globalOffset += t.Value;
            }
            /*List<string> strings = new List<string>();
            string[] strin = code.Split('\n');
            int cursor = 0;
            for (var i = 0; i<code.Length; i++)
            {
                char temp = code[i];
                if (temp != '\n') continue;
                string tempStr = code.Substring(cursor, i-cursor);
                strings.Add(tempStr);
                cursor = i+1;
            }
            foreach(var str in strin)
                Console.WriteLine(str);*/
            return code;
        }

        private enum RefactorState {
            MethodLine,
            NewLine
        }
    }
}