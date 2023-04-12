using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Djo
{
    internal class Program
    {
        static void show(List<int> l)
        {
            for(int i = 0; i < l.Count; i+=2)
            {
                Console.WriteLine("{0,-2} {1}",l[i],l[i + 1]);
            }            
        }
        static void show(int[,] mas)
        {
            for(int i = 0; i < mas.GetLength(0); i++)
            {
                for(int j = 0; j < mas.GetLength(1); j++)
                {
                    Console.Write(mas[i,j] + " ");
                }
                Console.WriteLine();
            }
        }    
        static void readFiles(List<string> startDate)
        {
            string[] file = File.ReadAllLines("Изначальное распределение.txt");
            for (int i = 0; i < file.Length; i++)
            {
                startDate.AddRange(file[i].Split(' '));
            }
        }
        static void twoListAboutMachine(List<string> startDate, List<int> OneMachine, List<int> TwoMachine)
        {
            string[,] startTable = new string[startDate.Count / 2, 2];
            for (int ind = 0, i = 0; ind < startDate.Count; ind += 2, i++)
            {
                startTable[i, 0] = startDate[ind];
                startTable[i, 1] = startDate[ind + 1];
                Console.WriteLine(startTable[i, 0] + " " + startTable[i, 1]);
            }
            while (true)
            {
                if (OneMachine.Count + TwoMachine.Count != startDate.Count)
                {
                    for (int i = 0; i < startTable.GetLength(0); i++)
                    {
                        if (Convert.ToInt32(startTable[i, 0]) < Convert.ToInt32(startTable[i, 1]))
                        {
                            OneMachine.Add(Convert.ToInt32(startTable[i, 0]));
                            OneMachine.Add(Convert.ToInt32(startTable[i, 1]));
                        }
                        else
                        {
                            TwoMachine.Add(Convert.ToInt32(startTable[i, 0]));
                            TwoMachine.Add(Convert.ToInt32(startTable[i, 1]));
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        static void convertListToInt(List<int> listmachine, int[,] masmaschine)
        {
            for (int i = 0, j=0; i < masmaschine.GetLength(0); i++,j++)
            {
                masmaschine[i, 0] = listmachine[j];
                j++;
                masmaschine[i, 1] = listmachine[j];                
            }
        }
        static void sortList(List<int> OneMachine, List<int> TwoMachine, int[,] finalyDate)
        {
            int[,] One = new int[OneMachine.Count / 2, 2];
            int[,] Two = new int[TwoMachine.Count / 2, 2];
            convertListToInt(OneMachine, One);
            convertListToInt(TwoMachine, Two);
            Console.WriteLine("SORTING");
            int indexe = 0;
            for (int i=0; i < One.GetLength(0); i++)
            {
                for (int j = i + 1; j < One.GetLength(0); j++)
                {
                    if (One[i, 0] > One[j, 0])
                    {
                        int temp = One[i, 0];
                        One[i, 0] = One[j , 0];
                        One[j , 0] = temp;
                        temp = One[i, 1];
                        One[i, 1] = One[j , 1];
                        One[j , 1] = temp;
                    }
                    else if (One[i, 0] == One[j , 0])
                    {
                        if (One[i, 1] < One[j , 1])
                        {
                            int temp = One[i, 0];
                            One[i, 0] = One[j , 0];
                            One[j , 0] = temp;
                            temp = One[i, 1];
                            One[i, 1] = One[j , 1];
                            One[j , 1] = temp;
                        }
                    }
                }
                File.AppendAllText("Финальное распределение.txt", $"{One[i, 0],-2} {One[i, 1]}");
                File.AppendAllText("Финальное распределение.txt", Environment.NewLine);
                Console.WriteLine("{0,-2} {1}", One[i, 0], One[i, 1]);
                finalyDate[indexe, 0] = One[i, 0];
                finalyDate[indexe, 1] = One[i, 1];
                indexe ++;
            }
            for (int i = 0; i < Two.GetLength(0); i++)
            {
                for (int j = i + 1; j < Two.GetLength(0); j++)
                {
                    if (Two[i, 1] < Two[j , 1])
                    {
                        int temp = Two[i, 0];
                        Two[i, 0] = Two[j , 0];
                        Two[j , 0] = temp;
                        temp = Two[i, 1];
                        Two[i, 1] = Two[j , 1];
                        Two[j , 1] = temp;
                    }
                    else if (Two[i, 1] == Two[j , 1])
                    {
                        if (Two[i, 0] > Two[j , 0])
                        {
                            int temp = Two[i, 0];
                            Two[i, 0] = Two[j , 0];
                            Two[j , 0] = temp;
                            temp = Two[i, 1];
                            Two[i, 1] = Two[j , 1];
                            Two[j , 1] = temp;
                        }
                    }
                }
                File.AppendAllText("Финальное распределение.txt", $"{Two[i, 0],-2} {Two[i, 1]}");
                File.AppendAllText("Финальное распределение.txt", Environment.NewLine);
                Console.WriteLine("{0,-2} {1}", Two[i, 0], Two[i, 1]);
                finalyDate[indexe, 0] = Two[i, 0];
                finalyDate[indexe, 1] = Two[i, 1];
                indexe++;
            } 
        }
        static void SearchOptimalTime(int[,] finalyDate, List<string> startDate)
        {
            int sum = finalyDate[0,0];
            int max = finalyDate[0, 0];
            for (int i = 1; i < finalyDate.GetLength(0); i++)
            {
                sum += finalyDate[i,0]-finalyDate[i-1,1];
                if (max < sum)
                {
                    max = sum;
                }
            }
            Console.WriteLine("Время простоя при оптимальной перестановке: " + max);
            int[,] mas = new int[startDate.Count / 2, 2];
            for (int i = 0, j = 0; i < mas.GetLength(0); i++, j++)
            {                
                mas[i, 0] = Convert.ToInt32(startDate[j]);               
                j++;
                mas[i, 1] = Convert.ToInt32(startDate[j]);
            }
            sum = mas[0, 0];
            max = mas[0, 0];
            for (int i = 1; i < mas.GetLength(0); i++)
            {
                sum += mas[i, 0] - mas[i - 1, 1];
                if (max < sum)
                {
                    max = sum;
                }
            }
            Console.WriteLine("Время простоя второй машины при первичном порядке: " + max);
        }

        static void Main(string[] args)
        {
            File.WriteAllText("Финальное распределение.txt", "");
            List<string> startDate = new List<string>();
            readFiles(startDate);
            List<int> OneMachine = new List<int>();
            List<int> TwoMachine = new List<int>();
            twoListAboutMachine(startDate, OneMachine,TwoMachine);
            int[,] finalyDate = new int[startDate.Count/2, 2];
            sortList(OneMachine, TwoMachine, finalyDate);
            SearchOptimalTime(finalyDate, startDate);          
            Console.ReadKey();

        }
    }
}

