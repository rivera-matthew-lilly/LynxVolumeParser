using System;
using System.IO;
using static System.Collections.Specialized.BitVector32;


class Program
{
    static void Main()
    {
        
        string noBlowoutVolList = "VI;12;8,300,300,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120";
        string volListWithBlowout = "VI;12;8,600;25,300;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25";

        LynxVolumeParser parser1= new LynxVolumeParser(volListWithBlowout);

        Console.WriteLine("\nCycle 1: ");
        string output = parser1.VolumeParserDriver();
        Console.WriteLine(output);
        for (int i = 0; i < parser1.SplitOccuranceList.Count(); i++) { Console.Write(parser1.SplitOccuranceList[i] + ","); }

        Console.WriteLine("\nCycle 2: ");
        output = parser1.VolumeParserDriver();
        Console.WriteLine(output);
        for (int i = 0; i < parser1.SplitOccuranceList.Count(); i++) { Console.Write(parser1.SplitOccuranceList[i] + ","); }

        Console.WriteLine("\nCycle 3: ");
        output = parser1.VolumeParserDriver();
        Console.WriteLine(output);
        for (int i = 0; i < parser1.SplitOccuranceList.Count(); i++) { Console.Write(parser1.SplitOccuranceList[i] + ","); }
    }

}