using System;
using System.IO;
using static System.Collections.Specialized.BitVector32;


class Program
{
    static void Main()
    {
        /*
         * Data need externally to drive Lynx:
         *      Max transfer cycle count (LynxVolumeParser.TransferCycleCount)
         *      
         */


        string noBlowoutVolList = "VI;12;8,300,300,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,50,120,120,120,120,120,120,120,120,120,120,120,120,120";
        string volListWithBlowout = "VI;12;8,280;25,300;25,120;25,120;25,120;25,120;25,120;25,200;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25";


        // TESTING: PARSER  ///////////////////////////////////////////////////////////////////////////

        LynxVolumeParser parser1= new LynxVolumeParser(volListWithBlowout);
        //LynxVolumeParser parser1 = new LynxVolumeParser(noBlowoutVolList);

        int currentCycleCount = 0;
        do
        {
            Console.WriteLine("\nCycle " + (currentCycleCount + 1) + ": ");
            string output = parser1.VolumeParserDriver();
            Console.WriteLine(output);
            for (int j = 0; j < parser1.SplitOccuranceList.Count(); j++) { Console.Write(parser1.SplitOccuranceList[j] + ","); }
            Console.WriteLine();
            currentCycleCount++;
        }
        while (currentCycleCount < parser1.TransferCycleCount);

        //////////////////////////////////////////////////////////////////////////////////////////////
        
        // TESTING: VOLUME VERIFICATION //////////////////////////////////////////////////////////////

        LynxAllowedVolumes validator1 = new LynxAllowedVolumes();
        validator1.addNewPlate("96w PCR", 200.0);
        validator1.addNewPlate("96w RoundBottom", 300.0);
        validator1.addNewPlate("96w MasterBlock", 2000.0);
        validator1.addNewPlate("96w FlatBottom", 200.0);
        validator1.addNewPlate("384w FlatBottom", 80.0);
        //bool validVolumes = validator1.ValidationDriver(noBlowoutVolList, "96w RoundBottom");
        bool validVolumes = validator1.ValidationDriver(volListWithBlowout, "96w RoundBottom");
        Console.WriteLine(validVolumes);

        //////////////////////////////////////////////////////////////////////////////////////////////
    }

}