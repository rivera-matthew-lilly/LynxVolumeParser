using System;

public class LynxMixingVolumes
{
    private const double MAX_TIP_VOL = 200.0;
    private const double MIX_SCALING_FACTOR = 0.75;
    private const int UPPER_MIX_COUNT = 8;
    private const int LOWER_MIX_COUNT = 5;
    private string InputTransferVolumeString { get; set; }
    private List<double> VolumeList { get; set; }
    private int MixCount { get; set; }
    private List<double> MixVolumeList { get; set; }
    private string MixVolumeString { get; set; }

    public LynxMixingVolumes()
	{
        VolumeList = new List<double>();
    }

    public void SplitInput()
    {
        bool blowOutPresent;
        if (InputTransferVolumeString.Substring(9).Contains(';')) { blowOutPresent = true; }
        else { blowOutPresent = false; }
        string[] tempList = InputTransferVolumeString.Split(',');
        if (!blowOutPresent) { for (int i = 1; i < tempList.Length; i++) { VolumeList.Add(Convert.ToDouble(tempList[i])); } }
        else
        {
            for (int i = 1; i < tempList.Length; i++)
            {
                string[] dispenseTempList = tempList[i].Split(';');
                VolumeList.Add(Convert.ToDouble(dispenseTempList[0]));
            }
        }
    }

    public void setMixingCycle() {
        double maxVol = VolumeList.Max();
        if (maxVol > MAX_TIP_VOL) { MixCount = UPPER_MIX_COUNT; }
        else { MixCount = LOWER_MIX_COUNT; }
    }
    public void CreateMixVolList() {
        MixVolumeList.Clear();
        for (int i = 0; i < VolumeList.Count(); i++) {
            if (VolumeList[i] > MAX_TIP_VOL) {
                MixVolumeList.Add(VolumeList[i] * MIX_SCALING_FACTOR);
            }
            else { MixVolumeList.Add(MAX_TIP_VOL); }
        }
    }

    public void CreateNewMixVolString()
    {
        MixVolumeString = "";
        MixVolumeString = InputTransferVolumeString.Substring(0, 8);
        for (int i = 0; i < MixVolumeList.Count(); i++)
        {
        MixVolumeString += Convert.ToString(MixVolumeList[i]) + ",";
        }
        // Remove trailing comma
        MixVolumeString = MixVolumeString.Substring(0, MixVolumeString.Length - 1);
    }

}


/*
 * mixing volume:
 *		above 200uL = 8mixes.
 *		none above 200uL = 5 mixes.
 *		75% total volume for mix for volumes under 200uL.
 *		200uL mix for volumes above 200uL.
 *		
 *	Need:
 *		mixing cycle count
 *		mixing volume string
 */