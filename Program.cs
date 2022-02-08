using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
namespace BerwyngroupEvaluaion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Path to read file from:");
            string pathin = Console.ReadLine();
            Console.WriteLine("Path to write file to:");
             string pathout = Console.ReadLine();
            List<Information> info = File.ReadAllLines(pathin)
                .Skip(1)
                .Select(x => Information.ReadFromFile(x))
                .ToList();
            int NumberOfElement = info.Count;          
            float average = Average(info);
            List<string>  result = Duplicate(info);
            Console.WriteLine("Tota Number Of Element: " + NumberOfElement);
            Compare(info);
            Console.WriteLine("Average Length of Val3 Accross all input rows: " + average);
            Console.WriteLine("Total Number of Duplicated Item: " + result.Count);
            for (int q = 0; q < result.Count; q++)
            {
                Console.WriteLine(result[q]);
            }
            WriteToFile(info, pathout);
            Console.ReadLine();

        }
        static void Compare(List<Information> List)
        {
            long Largest = List[0].total;
            string GUID = List[0].GUID;
            for(int i = 1; i < List.Count; i++)
            {
                if (List[i].total > List[i - 1].total)
                {
                    Largest = List[i].total;
                    GUID = List[i].GUID;
                }
            }
            Console.WriteLine("Largest Total val1 + Val2 is: " + Largest);
            Console.WriteLine("At GUID: " + GUID);
        }
        static List<string> Duplicate(List<Information> list)
        {
            List<Information> Checker = new List<Information>(list);
            var DupeList = Checker.GroupBy(x => x.GUID)
                .Where(y => y.Count() > 1)
                .Select(a => a.Key).ToList();
            return DupeList;
        }
        static float Average(List<Information> List)
        {
            int total = 0;
            for(int i = 0; i < List.Count; i++)
            {
                total += List[i].Val3.Length;
            }
            float average = total / List.Count;
            return average;
        }
        static void WriteToFile(List<Information> List,string pathout)
        {
            var filepath = pathout;
            List<string> DupeList = Duplicate(List);
            float Avg = Average(List);
            var csv = new StringBuilder();
            string first;
            string second;
            string third;
            string fourth;
            using (StreamWriter writer = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("GUID, Val1+Val2, IsDuplicated, >Val3.Length?");
                for (int i = 0; i < List.Count; i++)
                {
                    first = List[i].GUID.ToString();
                    second = List[i].total.ToString();
                    bool checker = DupeList.Contains(List[i].GUID);
                    if (List[i].Val3.Length > Avg)
                    {
                        fourth = "Y";
                    }
                    else
                        fourth = "N";
                    if (checker == true)
                    {
                        third = "Y";
                    }
                    else
                    {
                        third = "N";
                    }
                    writer.WriteLine(first + ", " + second + ", " + third + ", " + fourth);
                }
            }
        }
    }
    class Information
    {
        public string GUID;
        public long Val1;
        public long Val2;
        public string Val3;
        public long total;

        public static Information ReadFromFile (string Line)
        {
            string[] info = Line.Split(',');
            for(int i = 0; i < info.Length; i++)
            {
                info[i] = info[i].Trim(new char[] { ' ', '"' });
            }
            Information input = new Information();
            input.GUID = Convert.ToString(info[0]);
            input.Val1 = Convert.ToInt64(info[1]);
            input.Val2 = Convert.ToInt64(info[2]);
            input.Val3 = Convert.ToString(info[3]);
            input.total = input.Val1 + input.Val2;
            return input;
        }
    }
}
