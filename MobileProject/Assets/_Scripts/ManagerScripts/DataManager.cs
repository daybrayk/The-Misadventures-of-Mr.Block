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
    /*public class DataManager : MonoBehaviour {

        private static DataManager _instance = null;
        public delegate void ScoreChange();
        public static event ScoreChange scoreChange;
        public bool scoreStart;
        private int _currency;
        private int _score;
        private float _timer;
        private float _scoreTimer;
        // Use this for initialization
        void Awake () {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else if (_instance != this)
                DestroyImmediate(gameObject);
            scoreStart = false;
            LoadGameData();
            _score = 0;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            _scoreTimer += Time.deltaTime;
            if (_timer > 30f)
                SaveGameData();
            if (_scoreTimer > 2.0f && scoreStart)
            {
                score += 1;
                _scoreTimer = 0;
            }
        }*/

    /*************** Data Persistance ***************/
    /*public void SaveGameData()
    {
        if(score > highScore)
            PlayerPrefs.SetInt("highScore", score);
        PlayerPrefs.SetInt("currency", currency);
        PlayerPrefs.SetFloat("musicVol", AudioManager.instance.musicSource.volume);
        PlayerPrefs.SetFloat("sfxVol", AudioManager.instance.sfxSource.volume);
        PlayerPrefs.SetInt("musicOnOff", AudioManager.instance.musicSource.mute == true ? 0 : 1);
        PlayerPrefs.SetInt("sfxOnOff", AudioManager.instance.sfxSource.mute == true ? 0 : 1);
        PlayerPrefs.Save();
        _timer = 0f;
    }

    public void LoadGameData()
    {
        Debug.Log("<color=blue>Programmer Log</color> : Loading Player Data");
        if (PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore");
            Debug.LogFormat("<color=green>DataManager:</color> " + "Loaded High Score: " + highScore);
        }

        if (PlayerPrefs.HasKey("currency"))
        {
            _currency = PlayerPrefs.GetInt("currency");
            Debug.LogFormat("<color=green>DataManager:</color> " + "Loaded Currency: " + _currency);
        }

        if (PlayerPrefs.HasKey("musicOnOff"))
        {
            if (PlayerPrefs.GetInt("musicOnOff") == 0)
            {
                AudioManager.instance.musicSource.mute = true;
            }
            else
                AudioManager.instance.musicSource.mute = false;
        }

        if (PlayerPrefs.HasKey("musicVol"))
        {
            AudioManager.instance.musicSource.volume = PlayerPrefs.GetFloat("musicVol");
            Debug.Log("<color=green>DataManager:</color> " + "Music Volume: " + AudioManager.instance.musicSource.volume);
        }

        if(PlayerPrefs.HasKey("sfxOnOff"))
        {
            if (PlayerPrefs.GetInt("sfxOnOff") == 0)
            {
                AudioManager.instance.sfxSource.mute = true;
            }
            else
                AudioManager.instance.sfxSource.mute = false;
            Debug.Log("<color=green>DataManager:</color> " + "Loaded SFX On Off: " + AudioManager.instance.sfxSource.mute);
        }

        if (PlayerPrefs.HasKey("sfxVol"))
        {
            AudioManager.instance.sfxSource.volume = PlayerPrefs.GetFloat("sfxVol");
            Debug.Log("<color=green>DataManager:</color> " + "Loaded SFX Volume: " + AudioManager.instance.sfxSource.volume);
        }
    }*/

    /*************** Getters and Setters ***************/
    /*public static DataManager instance{
        get { return _instance; }
    }

    public int highScore { get; private set; }

    public int currency
    {
        get { return _currency; }
        set { _currency += value; }
    }

    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            if (score > highScore)
                highScore = score;
            if(scoreChange != null)
                scoreChange();
        }
    }*/
}
