using System;
using System.Collections;

public class LynxVolumeParser
{
	private string inputTransferVolumeString;
    private string outputTransferVolumeString;
    private bool blowoutPresent;
    private bool needsAdjustment;
    private const double MAX_TIP_VOL = 200.0;
    private int parseCount = 0;
    private List<int> splitOccuranceList = new List<int>();
    private List<string> volumeList = new List<string>(); //noBlowoutVolList_Parsed & volListWithBlowout_Parsed
    private List<int> volumeFragmentsNeededList = new List<int>();  //volListWithBlowout_SplitList & newVolList_volListWithBlowout
    private List<string> blowoutVolumeList = new List<string>(); // new
    private List<double> updatedVolumeList = new List<double>(); //new

    public string InputTransferVolumeString {
        get { return this.inputTransferVolumeString; }
        set { this.inputTransferVolumeString = value;} 
    }

    public string OutputTransferVolumeString {
        get { return this.outputTransferVolumeString; }
        set { this.outputTransferVolumeString = value;}
    }

    public bool BlowoutPresent { 
        get { return this.blowoutPresent;}
        set { this.blowoutPresent = value;}
    }

    private bool NeedsAdjustment { 
        get { return this.needsAdjustment; }
        set { this.needsAdjustment = value; }
    }

    private int ParseCount { 
        get { return this.parseCount; }
        set { this.parseCount = value; }
    }

    public List<int> SplitOccuranceList {
        get { return this.splitOccuranceList; }
        set { this.splitOccuranceList = value;}
    }

    public List<string> VolumeList {
        get { return this.volumeList; }
        set { this.volumeList = value;}
    }

    public List<int> VolumeFragmentsNeededList {   
        get { return this.volumeFragmentsNeededList; }
        set { this.volumeFragmentsNeededList = value;}
    }

    public List<string> BlowoutVolumeList {
        get { return this.blowoutVolumeList; }
        set { this.blowoutVolumeList = value;}
    }

    public List<double> UpdatedVolumeList {
        get { return this.updatedVolumeList; }
        set { this.updatedVolumeList = value; }
    }


    // Constructor
    public LynxVolumeParser(string inputVolList) { 
        this.inputTransferVolumeString = inputVolList;
        for (int i = 0; i < 96; i++)
            splitOccuranceList.Add(0);
    }

    // Splits volume string into a List depending on if blowout is prestn or not
    public void SplitInput()
    {
        if (InputTransferVolumeString.Substring(9).Contains(';')) { BlowoutPresent = true; }
        else { BlowoutPresent = false; }
        string[] tempList = InputTransferVolumeString.Split(',');
        if (!BlowoutPresent) { for (int i = 1; i < tempList.Length; i++) { VolumeList.Add(tempList[i]); } }
        else {
            for (int i = 1; i < tempList.Length; i++) {
                string[] dispenseTempList = tempList[i].Split(';');
                VolumeList.Add(dispenseTempList[0]);
                BlowoutVolumeList.Add(dispenseTempList[1]);
            }
        }

        // For testing - delete later
        /*
        foreach (var item in volumeList) {
            Console.Write(item + ", ");
        }
        Console.WriteLine();
        */
    }

    public void CheckAdjustmentNeeds()
    {
        for (int i = 0; i < VolumeList.Count; i++)
        {
            if (Convert.ToDouble(VolumeList[i]) >= MAX_TIP_VOL) { NeedsAdjustment = true; break; }
            else { NeedsAdjustment = false; }
        }
    }

    // Creates a list of need iteration of aspiration and dispensing
    public void CreateFragmentsList() {
        for (int i = 0; i < VolumeList.Count; i++) {
            int volSplitCount_Temp = 0; 
            double currentVol = Convert.ToDouble(VolumeList[i]);
            volSplitCount_Temp = (int)(currentVol / MAX_TIP_VOL);
            VolumeFragmentsNeededList.Add(volSplitCount_Temp);
        }

        // For testing - delete later
        /*
        Console.Write("Split Count List: ");
        foreach (var item in VolumeFragmentsNeededList) {
            Console.Write(item + ", ");
        }
        Console.WriteLine();
        */
    }

    // Creates updated volume list
    public void CreateNewVolList() {
        UpdatedVolumeList.Clear();
        int maxSplitOccurance = SplitOccuranceList.Max();
        for (int i = 0; i < VolumeList.Count(); i++) {
            if (VolumeFragmentsNeededList[i] != SplitOccuranceList[i]) {
                double currnetSplitCount = Convert.ToDouble(VolumeFragmentsNeededList[i]);
                if (currnetSplitCount == 1) { currnetSplitCount++; }
                double newVol = Convert.ToInt32(VolumeList[i]) / currnetSplitCount;
                UpdatedVolumeList.Add(newVol);
                SplitOccuranceList[i] = SplitOccuranceList[i] + 1;
            }
            else if (maxSplitOccurance == 0) { UpdatedVolumeList.Add(Convert.ToDouble(VolumeList[i])); }
            else { UpdatedVolumeList.Add(0.0); }
        }
    }

    // Creates transfer string, Combines blowout volume if present
    public void CreateNewVolString() {
        OutputTransferVolumeString = "";
        OutputTransferVolumeString = InputTransferVolumeString.Substring(0, 8);
        if (BlowoutPresent) {
            for (int i = 0; i < UpdatedVolumeList.Count(); i++) {
                OutputTransferVolumeString += Convert.ToString(UpdatedVolumeList[i]) + ";";
                if (UpdatedVolumeList[i] > 0) { OutputTransferVolumeString += blowoutVolumeList[i] + ","; }
                else { OutputTransferVolumeString += "0.0,"; }
            }
        }
        else {
            for (int i = 0; i < UpdatedVolumeList.Count(); i++){
                OutputTransferVolumeString += Convert.ToString(UpdatedVolumeList[i]) + ",";
            }
        }
        // Remove trailing comma
        OutputTransferVolumeString = OutputTransferVolumeString.Substring(0, OutputTransferVolumeString.Length - 1);
    }

    public String VolumeParserDriver() {
        Console.WriteLine("Strating...");
        if (ParseCount == 0) { SplitInput(); }
        CheckAdjustmentNeeds();
        if (NeedsAdjustment) {
            if (ParseCount == 0) { CreateFragmentsList(); }
            CreateNewVolList();
            CreateNewVolString();
            ParseCount++;
            return OutputTransferVolumeString;
        }
        else {  return InputTransferVolumeString; }
    }

}
