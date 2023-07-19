using System;

public class LynxVolumeDoctor
{
    private double TipVolMax;
    private string InputTransferVolumeString { get; set; }
    private string PlateType { get; set; }
    private int ParseCount { get; set; }
    private List<double> VolumeList { get; set; }
    private string OutputTransferVolumeString { get; set; }
    private bool NeedsAdjustment { get; set; }
    private List<int> VolumeFragmentsNeededList { get; set; }
    private List<double> BlowoutVolumeList { get; set; }
    private List<double> UpdatedVolumeList { get; set; }
    public List<int> SplitOccuranceList { get; set; }
    public int TransferCycleCount { get; set; }

    public LynxVolumeDoctor(string inputVolList, double tipVolMax, string plateType)
    {
        InputTransferVolumeString = inputVolList;
        TipVolMax = tipVolMax;
        PlateType = plateType;
        ParseCount = 0;
        VolumeList = FormatLynxVolume.ExtractVolList(inputVolList);
        VolumeFragmentsNeededList = new List<int>();
        BlowoutVolumeList = new List<double>();
        UpdatedVolumeList = new List<double>();
        SplitOccuranceList = new List<int>();
        for (int i = 0; i < 96; i++)
            SplitOccuranceList.Add(0);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////// START: VOLUME PARSING AND REFACTORING /////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void CheckAdjustmentNeeds()
    {
        for (int i = 0; i < VolumeList.Count; i++)
        {
            if (Convert.ToDouble(VolumeList[i]) >= TipVolMax) { NeedsAdjustment = true; break; }
            else { NeedsAdjustment = false; }
        }
    }

    // Creates a list of need iteration of aspiration and dispensing
    // Max value of VolumeFragmentsNeededList will control the cycle count of trasnfer
    public void CreateFragmentsList()
    {
        for (int i = 0; i < VolumeList.Count; i++)
        {
            int volSplitCount_Temp = 0;
            double currentVol = Convert.ToDouble(VolumeList[i]);
            if (currentVol <= TipVolMax) { volSplitCount_Temp = 0; }
            else { volSplitCount_Temp = (int)(currentVol / TipVolMax); }

            if (volSplitCount_Temp == 1) { volSplitCount_Temp++; }
            VolumeFragmentsNeededList.Add(volSplitCount_Temp);
        }

        // Find max volume
        TransferCycleCount = VolumeFragmentsNeededList.Max();
        Console.WriteLine(TransferCycleCount.ToString());
    }

    // Creates updated volume list
    public void UpdateVolList()
    {
        UpdatedVolumeList.Clear();
        int maxSplitOccurance = SplitOccuranceList.Max();
        for (int i = 0; i < VolumeList.Count(); i++)
        {
            if (VolumeFragmentsNeededList[i] != SplitOccuranceList[i])
            {
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


    public String VolumeDoctorDriver()
    {
        Console.WriteLine("Strating...");
        CheckAdjustmentNeeds();
        if (NeedsAdjustment)
        {
            if (ParseCount == 0) { CreateFragmentsList(); } //Max value created here
            if (FormatLynxVolume.ContainsBlowout(InputTransferVolumeString))
            {
                BlowoutVolumeList = FormatLynxVolume.ExtractBlowoutVolList(InputTransferVolumeString);
                UpdateVolList();
                OutputTransferVolumeString = FormatLynxVolume.ConvertListToVolString(InputTransferVolumeString, UpdatedVolumeList, BlowoutVolumeList, true);
            }
            else 
            {
                UpdateVolList();
                OutputTransferVolumeString = FormatLynxVolume.ConvertListToVolString(InputTransferVolumeString, UpdatedVolumeList);

            }
            ParseCount++;
            return OutputTransferVolumeString;
        }
        else { return InputTransferVolumeString; }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////// END: VOLUME PARSING AND REFACTORING ///////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





}
