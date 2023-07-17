using System;

public class LynxAllowedVolumes
{
    private Dictionary<string, double> PlateDictionary { get; set; }
    private bool ValidVolumes { get; set; }
    private string InputTransferVolumeString { get; set; }
    private List<string> VolumeList { get; set; }
    private string PlateName { get; set; }

    public LynxAllowedVolumes() {
        PlateDictionary = new Dictionary<string, double>();
        VolumeList = new List<string>();
    }

    public void addNewPlate(string plateName, double maxVolume) {
        if (!plateName.Equals("") && maxVolume > 0) { PlateDictionary.Add(plateName, maxVolume); }
    }

    public void removePlate(string plateName) {
        if (!plateName.Equals("")) { PlateDictionary.Remove(plateName); }
    }

    public void SplitInput() {
        bool blowOutPresent;
        if (InputTransferVolumeString.Substring(9).Contains(';')) { blowOutPresent = true; }
        else { blowOutPresent = false; }
        string[] tempList = InputTransferVolumeString.Split(',');
        if (!blowOutPresent) { for (int i = 1; i < tempList.Length; i++) { VolumeList.Add(tempList[i]); } }
        else {
            for (int i = 1; i < tempList.Length; i++) {
                string[] dispenseTempList = tempList[i].Split(';');
                VolumeList.Add(dispenseTempList[0]);
            }
        }
    }

    public void ValidateVolume() {
        double maxVolumeInputString = Convert.ToDouble(VolumeList.Max());
        double maxVolumePlateType = PlateDictionary[PlateName];
        if (maxVolumeInputString <= maxVolumePlateType) { ValidVolumes = true; }
        else { ValidVolumes = false; }
    }

    public bool ValidationDriver(string volumesString, string plateName) {
        InputTransferVolumeString = volumesString;
        PlateName = plateName;
        SplitInput();
        ValidateVolume();
        if (ValidVolumes) { return true; } 
        else {  return false; }
    }
}

/*
 * dictionary to hold plate name and max volume 
 * ability to change and add new data to that dictionary
 * boolean method that checks if max volume of transfer string is less than max volume of plate
 * 
 */ 