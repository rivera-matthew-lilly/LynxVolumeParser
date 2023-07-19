using System;

public class FormatLynxVolume
{
    // Check for blowout
    public static bool ContainsBlowout(string volumeString) 
    {
        if (volumeString.Substring(9).Contains(';')) { return true; }
        else { return false; }
    }

    // Format to volume only list 
    public static List<double> ExtractVolList(string volumeString) 
    {
        List<double> volumeList = new List<double>();
        string[] tempList = volumeString.Split(',');
        if (!ContainsBlowout(volumeString))
        {
            for (int i = 1; i < tempList.Length; i++)
            {
                volumeList.Add(Convert.ToDouble(tempList[i]));
            }
        }
        else 
        {
            for (int i = 1; i < tempList.Length; i++)
            {
                string[] dispenseTempList = tempList[i].Split(';');
                volumeList.Add(Convert.ToDouble(dispenseTempList[0]));
            }
        }
        return volumeList;
    }

    // Format blowout volume only list
    public static List<double> ExtractBlowoutVolList(string volumeString)
    {
        List<double> blowoutVolumeList = new List<double>();
        string[] tempList = volumeString.Split(',');
        if (ContainsBlowout(volumeString))
        {
            for (int i = 1; i < tempList.Length; i++)
            {
                string[] dispenseTempList = tempList[i].Split(';');
                blowoutVolumeList.Add(Convert.ToDouble(dispenseTempList[1]));
            }
        }
        else
        {
            return blowoutVolumeList;
        }
        return blowoutVolumeList;
    }

    public static string ConvertListToVolString(string orginalVolumeString, List<double> volList, List<double> blowoutVolList = null, bool containsBlowout = false)
    {
        string outputTransferVolumeString = "";
        outputTransferVolumeString = orginalVolumeString.Substring(0, 8);
        if (containsBlowout)
        {
            for (int i = 0; i < volList.Count(); i++)
            {
                outputTransferVolumeString += Convert.ToString(volList[i]) + ";";
                if (volList[i] > 0) { outputTransferVolumeString += blowoutVolList[i] + ","; }
                else { outputTransferVolumeString += "0.0,"; }
            }
        }
        else
        {
            for (int i = 0; i < volList.Count(); i++)
            {
                outputTransferVolumeString += Convert.ToString(volList[i]) + ",";
            }
        }
        // Remove trailing comma
        outputTransferVolumeString = outputTransferVolumeString.Substring(0, outputTransferVolumeString.Length - 1);
        return outputTransferVolumeString;
    }

}
