﻿using System;
using System.Collections;

public class LynxVolumeParser
{
    private const double MAX_TIP_VOL = 200.0;
    private int parseCount = 0;
    private string InputTransferVolumeString { get; set; }
    private string OutputTransferVolumeString { get; set; }
    private bool BlowoutPresent { get; set; }
    private bool NeedsAdjustment { get; set; }
    private List<double> VolumeList { get; set; }
    private List<int> VolumeFragmentsNeededList { get; set; }
    private List<string> BlowoutVolumeList { get; set; }
    private List<double> UpdatedVolumeList { get; set; }

    public List<int> SplitOccuranceList { get; set; }
    public int TransferCycleCount { get; set; }

    // Constructor
    public LynxVolumeParser(string inputVolList)
    {
        SplitOccuranceList = new List<int>();
        VolumeList = new List<double>();
        VolumeFragmentsNeededList = new List<int>();
        BlowoutVolumeList = new List<string>();
        UpdatedVolumeList = new List<double>();
        InputTransferVolumeString = inputVolList;
        SplitInput();
        for (int i = 0; i < 96; i++)
            SplitOccuranceList.Add(0);
    }

    private int ParseCount { 
        get { return this.parseCount; }
        set { this.parseCount = value; }
    }

    // Splits volume string into a List depending on if blowout is prestn or not
    public void SplitInput() // HELPER
    {
        if (InputTransferVolumeString.Substring(9).Contains(';')) { BlowoutPresent = true; }
        else { BlowoutPresent = false; }
        string[] tempList = InputTransferVolumeString.Split(',');
        if (!BlowoutPresent) { for (int i = 1; i < tempList.Length; i++) { VolumeList.Add(Convert.ToDouble(tempList[i])); } }
        else {
            for (int i = 1; i < tempList.Length; i++) {
                string[] dispenseTempList = tempList[i].Split(';');
                VolumeList.Add(Convert.ToDouble(dispenseTempList[0]));
                BlowoutVolumeList.Add(dispenseTempList[1]);
            }
        }
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
    // Max value of VolumeFragmentsNeededList will control the cycle count of trasnfer
    public void CreateFragmentsList() {
        for (int i = 0; i < VolumeList.Count; i++) {
            int volSplitCount_Temp = 0; 
            double currentVol = Convert.ToDouble(VolumeList[i]);
            volSplitCount_Temp = (int)(currentVol / MAX_TIP_VOL);
            VolumeFragmentsNeededList.Add(volSplitCount_Temp);
        }

        // Find max volume

        // For testing - delete later
        TransferCycleCount = VolumeFragmentsNeededList.Max();

        Console.Write("Split Count List: ");
        foreach (var item in VolumeFragmentsNeededList) {
            Console.Write(item + ", ");
        }
        Console.WriteLine();
        
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
                if (UpdatedVolumeList[i] > 0) { OutputTransferVolumeString += BlowoutVolumeList[i] + ","; }
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

    public String VolumeParserDriver() // Main?
    {
        Console.WriteLine("Strating...");
        CheckAdjustmentNeeds();
        if (NeedsAdjustment) {
            if (ParseCount == 0) { CreateFragmentsList(); } //Max value created here
            CreateNewVolList();
            CreateNewVolString();
            ParseCount++;
            return OutputTransferVolumeString;
        }
        else {  return InputTransferVolumeString; }
    }

}
