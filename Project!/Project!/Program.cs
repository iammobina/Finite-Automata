using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_
{

    class CreateGraph
    {
        public string Startstate;
        public string Input;
        public string Nextstate;

        public CreateGraph(string startstate, string input, string nextstate)
        {
            Startstate = startstate;
            Input = input;
            Nextstate = nextstate;
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            //بخش صفر
            FileStream nfaFile = new FileStream("nfa.txt", FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(nfaFile);

            //string[] text = File.ReadAllLines(@"E:\Project1\input2.txt", Encoding.UTF8);

            //int NumberofState = int.Parse(text[0]);
            //List<char> alphabet = new List<char>();

            int NumofStates = int.Parse(reader.ReadLine());
            List<string> alphabets1 = new List<string>();
            string[] alphabets2 = reader.ReadLine().Split(',');
            for (int i = 0; i < alphabets2.Length; i++)
            {
                alphabets1.Add(alphabets2[i]);
            }
            List<List<string>> States = new List<List<string>>();
            List<string> State = new List<string>();
            string statesString;
            for (int i = 0; (statesString = reader.ReadLine()) != null; i++)
            {
                State = statesString.Split(',').ToList();
                States.Add(State);
            }

            //List<string> Start_State = new List<string>();
            ////Tuple<string, string> connection = new Tuple<string, string>();

            //foreach (var charecter in text[1])
            //{
            //    if (charecter != ',')
            //        alphabet.Add(charecter);
            //}

            //foreach (var str in text.Skip(2))
            //{
            //    if (str.Contains("->"))
            //    {

            //        var newstr = str.Trim(new char[] { '-', '>' });
            //        if (!Start_State.Contains(newstr[0].ToString() + newstr[1].ToString()))
            //            Start_State.Add(newstr[0].ToString() + newstr[1].ToString());

            //        states.Add(newstr);

            //    }

            //    else if (str.Contains("*"))
            //    {
            //        int Finalindex = str.IndexOf("*");
            //        if (!Final_State.Contains(str[Finalindex + 1].ToString() + str[Finalindex + 2].ToString()))
            //            Final_State.Add(str[Finalindex + 1].ToString() + str[Finalindex + 2].ToString());

            //        states.Add(str.Remove(Finalindex, 1));
            //    }
            //    else
            //    {
            //        states.Add(str);
            //    }
            //}

            //List<CreateGraph> connection = new List<CreateGraph>();
            ////for(int i=0;i<states.Count();i++)
            ////{
            ////    states.Select(state => new CreateGraph{ Startstate = states[i].Substring(0, 1).ToString(), Input = states[i].Substring(4, 5).ToString(), Nextstate = states[i].Substring(7, 8).ToString() }).ToList());
            ////    //connection[i].Startstate = states[i].Substring(0, 1).ToString();
            ////    //connection[i].Input = states[i].Substring(4, 5).ToString();
            ////    //connection[i].Nextstate = states[i].Substring(7, 8).ToString();
            ////}

            Console.ReadKey();
        }

        /// <summary>
        /// بخش اول
        /// </summary>
        /// <param name="Connections"> روابط بین حالت ها </param>
        /// <param name="NumberOfStates">تعداد حالت ها</param>
        /// <param name="alphabets1">الفبای زبان</param>
        public static void NfaToDfa(List<List<string>> Connections, int NumberOfStates, List<string> alphabets1)
        {
            string Start_State = "i";
            List<string> Final_State = new List<string>();
            List<List<string>> LandaTransition = new List<List<string>>();

            /// مشخص کردن حالت اولیه
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i][0].Substring(0, 2) == "->")
                {
                    Start_State = Connections[i][0].Substring(2);
                    Connections[i][0] = Start_State;
                }
            }

            /// مشخص کردن حالت نهایی
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i][0].Substring(0, 1) == "*" && !Final_State.Contains(Connections[i][0]))
                    Final_State.Add(Connections[i][0]);

                if (Connections[i][2].Substring(0, 1) == "*" && !Final_State.Contains(Connections[i][2]))
                    Final_State.Add(Connections[i][2]);
            }

            /// تعیین لاندا
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i][1] == "_")
                {
                    List<string> Landa = new List<string>();
                    Landa.Add(Connections[i][0]);
                    Landa.Add(Connections[i][2]);
                    LandaTransition.Add(Landa);
                }

            }

            bool isFinalState = false;

            List<string> Component = new List<string>();
            Component.Add(Start_State);

            List<List<string>> groups = new List<List<string>>();
            groups.Add(Component);

            List<List<string>> results = new List<List<string>>();

            Component = new List<string>();

            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < alphabets1.Count; j++)
                {
                    for (int k = 0; k < groups[i].Count; k++)
                    {
                        for (int l = 0; l < Connections.Count; l++)
                        {
                            if (Connections[l][1] == "_" && Connections[l][0] == groups[i][k])
                                LandaTransitions(Connections, Component, Connections[l][2], alphabets1[j],
                                    Final_State, ref isFinalState, groups[i][k]);

                            else if (Connections[l][0] == groups[i][k] &&
                                    (Connections[l][1] == alphabets1[j]) &&
                                    !Component.Contains(Connections[l][2]))
                            {
                                for (int x = 0; x < LandaTransition.Count; x++)
                                {
                                    if (Connections[l][2] == LandaTransition[x][0] && !Component.Contains(LandaTransition[x][1]))
                                    {
                                        if (Final_State.Contains(LandaTransition[x][1]))
                                            isFinalState = true;
                                        Component.Add(LandaTransition[x][1]);
                                    }
                                }

                                if (Final_State.Contains(groups[i][k]))
                                    isFinalState = true;

                                Component.Add(Connections[l][2]);

                            }
                        }
                    }

                    if (!DuplicateAvoider(groups, Component) && Component.Count != 0)
                    {
                        List<string> result = new List<string>();
                        if (isFinalState)
                        {
                            result.Add($"*q{i}");
                            isFinalState = false;
                        }
                        else
                            result.Add($"q{i}");

                        result.Add(alphabets1[j]);
                        result.Add($"q{groups.Count}");
                        results.Add(result);
                        groups.Add(Component);
                    }
                    else
                    {
                        List<string> result = new List<string>();
                        if (isFinalState)
                        {
                            result.Add($"*q{i}");
                            isFinalState = false;
                        }
                        else
                            result.Add($"q{i}");

                        result.Add(alphabets1[j]);
                        result.Add($"q{Duplicate(groups, Component)}");
                        results.Add(result);
                    }
                    Component = new List<string>();
                }
            }

            List<string> final = new List<string>();

            for (int i = 0; i < results.Count; i++)
                if (results[i][0].Substring(0, 1) == "*" && !final.Contains(results[i][0]))
                    final.Add(results[i][0].Substring(1));

            for (int i = 0; i < results.Count; i++)
            {
                if (final.Contains(results[i][2]))
                    results[i][2] = $"*{results[i][2]}";

                if (final.Contains(results[i][0]) && results[i][0].Substring(0, 1) != "*")
                    results[i][0] = $"*{results[i][0]}";
            }

            results[0][0] = $"->{results[0][0]}";
            List<List<string>> finalResults = new List<List<string>>();
            List<string> temp = new List<string>();
            temp.Add(NumberOfStates.ToString());
            finalResults.Add(temp);
            temp = new List<string>();
            for (int i = 0; i < alphabets1.Count; i++)
                temp.Add(alphabets1[i]);
            finalResults.Add(temp);

            for (int i = 0; i < results.Count; i++)
            {
                List<string> list = new List<string>();
                list.Add(results[i][0]);
                list.Add(results[i][1]);
                list.Add(results[i][2]);
                finalResults.Add(list);
            }

            List<string> Answer = new List<string>();
            Answer.Add($"{groups.Count}");
            string alphabet = "";
            for (int i = 0; i < alphabets1.Count; i++)
            {
                alphabet += alphabets1[i];
                if (i != alphabets1.Count - 1)
                    alphabet += ',';
            }
            Answer.Add(alphabet);
            for (int i = 0; i < results.Count; i++)
            {
                Answer.Add($"{results[i][0]},{results[i][1]},{results[i][2]}");
            }
            using (StreamWriter outputFile = new StreamWriter(@"C:\Users\Mobina\Documents\Visual Studio 2017\Projects\Project!\Project!\bin\Debug\Output1.txt", true))
            {
                foreach (string line in Answer.ToArray())
                    outputFile.WriteLine(line);
            }
            // return finalResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private static bool DuplicateAvoider(List<List<string>> groups, List<string> group)
        {
            for (int i = 0; i < groups.Count; i++)
                if (groups[i].All(group.Contains) && group.All(groups[i].Contains))
                    return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        private static int Duplicate(List<List<string>> groups, List<string> group)
        {
            for (int i = 0; i < groups.Count; i++)
                if (groups[i].All(group.Contains) && group.All(groups[i].Contains))
                    return i;
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Connection">روابط بین حالت ها</param>
        /// <param name="group"></param>
        /// <param name="t"></param>
        /// <param name="s"></param>
        /// <param name="finalStates">حالت نهایی</param>
        /// <param name="isFinalState">آیا حالت نهای است یا خیر؟</param>
        /// <param name="str"></param>
        private static void LandaTransitions(List<List<string>> Connection, List<string> group, string t,
            string s, List<string> finalStates, ref bool isFinalState, string str)
        {
            for (int i = 0; i < Connection.Count; i++)
            {
                if (t == Connection[i][0] && s == Connection[i][1] && !group.Contains(Connection[i][2]))
                {
                    group.Add(Connection[i][2]);
                    if (finalStates.Contains(Connection[i][2]))
                        isFinalState = true;
                }
            }
        }
    }
}
