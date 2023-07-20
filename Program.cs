using System;
using System.ComponentModel.DataAnnotations;
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


        string noBlowoutVolList = "VI;12;8,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,120,50,120,120,120,120,120,120,120,120,120,120,120,120,120";
        string volListWithBlowout = "VI;12;8,280;25,2000;25,600;25,120;25,120;25,120;25,120;25,200;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,120;25,0;25";


        // (STEP 1) INTIALIZING NEW LYNX DOCOTOR CLASS ///////////////////////////////////////////////

        LynxVolumeDoctor doctor1 = new LynxVolumeDoctor(volListWithBlowout, 200.0, "96w MasterBlock");

        ///////////////////////////////////////////////////////////////////////////////////////////////

        // (STEP 2) VOLUME VERIFICATION //////////////////////////////////////////////////////////////

        doctor1.addNewPlate("96w PCR", 200.0);
        doctor1.addNewPlate("96w RoundBottom", 300.0);
        doctor1.addNewPlate("96w MasterBlock", 2000.0);
        doctor1.addNewPlate("96w FlatBottom", 200.0);
        doctor1.addNewPlate("384w FlatBottom", 80.0);
        bool validVolumes = doctor1.ValidMaxVolDoctorDriver();
        Console.WriteLine("Volumes Valid For Plate Type: " + validVolumes);

        ///////////////////////////////////////////////////////////////////////////////////////////////

        // (STEP 3) VOLUME STRING CREATION  ///////////////////////////////////////////////////////////

        if (validVolumes) // ONLY CREATES TRASNFER STRING IF VOLUMES ARE VALID
        {
            int currentCycleCount = 0;
            do
            {
                Console.WriteLine("\nCycle " + (currentCycleCount + 1) + ": ");
                string output = doctor1.VolumeDoctorDriver();
                Console.WriteLine("Transfer Loops Required: " + doctor1.TransferCycleCount.ToString());
                Console.WriteLine("Update Transfer String: " + output);
                Console.WriteLine();
                Console.Write("Split Occurance List: ");
                for (int j = 0; j < doctor1.SplitOccuranceList.Count(); j++) { Console.Write(doctor1.SplitOccuranceList[j] + ","); }
                Console.WriteLine();
                currentCycleCount++;
            }
            while (currentCycleCount < doctor1.TransferCycleCount);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        
        // (STEP 4) MIXING VOLUME ////////////////////////////////////////////////////////////////////

        string mixingString = doctor1.MixingDoctorDriver();
        int mixCycleCount = doctor1.MixCount;
        Console.WriteLine();
        Console.WriteLine("Mixing String: " + mixingString);
        Console.WriteLine("Mixing Cycle Count: " + mixCycleCount);

        //////////////////////////////////////////////////////////////////////////////////////////////

    }

}