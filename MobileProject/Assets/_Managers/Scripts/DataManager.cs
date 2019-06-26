using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class PersistentData
{
    public int highScore;
    public float musicVolume;
    public float sfxVolume;
    public bool musicOnOff;
    public bool sfxOnOff;

    public PersistentData()
    {
        highScore = 0;
        musicVolume = 1;
        sfxVolume = 1;
        musicOnOff = false;
        sfxOnOff = false;
    }

    public PersistentData(GameManager gm, AudioManager am)
    {
        highScore = gm.highScore;
        musicVolume = am.musicSource.volume;
        sfxVolume = am.sfxSource.volume;
        musicOnOff = am.musicSource.mute;
        sfxOnOff = am.sfxSource.mute;
    }
}

public static class DataManager {
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter(); 
        string path = Application.persistentDataPath + "/GameData.bj";
        FileStream stream = new FileStream(path, FileMode.Create);

        PersistentData data = new PersistentData(GameManager.instance, AudioManager.instance);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PersistentData LoadGame()
    {
        string path = Application.persistentDataPath + "/GameData.bj";
        if(File.Exists(path) && new FileInfo(path).Length != 0)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PersistentData data = new PersistentData();
            try
            {
                data = formatter.Deserialize(stream) as PersistentData;
            }catch (System.TypeLoadException e)
            {
                Debug.LogError("Programmer Error: Class type - " + data.GetType() + "does not match class type in persistent data file at - " + path +
                	"\n<color=red>deleting persistent data file:</color> " + path +"\nFunction returning new PersistentData class!" +
                    "\n<color=red>Stack Trace:</color> " + e);
                File.Delete(path);
            }catch(System.Exception e)
            {
                Debug.LogError("<color=yellow>Programmer Error:</color> Exception thrown from LoadGame() function in DataManager class\n" +
                	"<color=red>Stack Trace:</color> " + e);
            }
            stream.Close();
            return data;

        }
        PersistentData newData = new PersistentData();
        return newData;
    }
}
