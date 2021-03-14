using UnityEngine;

public class DataCenter : MonoBehaviour
{
    public static DataCenter Instance { get; private set; }
    public static bool ready;

    [SerializeField] private DataProfile defaultData;
    [SerializeField] private DataProfile liveData;
    [SerializeField] private DataProfile saveData;

    [SerializeField] private int maxSaveRetry = 10;
    [SerializeField] private int maxLoadRetry = 10;

    public static GameData LiveData => Instance.liveData.gameData;
    public static GameData SaveData => Instance.saveData.gameData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadGameData();
        ready = true;
    }

    private void OnApplicationQuit()
    {
        defaultData.CopyTo(liveData);
        defaultData.CopyTo(saveData);
    }

    public static void ResetLiveGameData()
    {
        Instance.defaultData.CopyTo(Instance.liveData);
    }

    public static void SaveGameData()
    {
        GameData SaveBackup = new GameData();
        SaveData.CopyTo(SaveBackup);

        SaveData.distance = Mathf.Max(LiveData.distance, SaveData.distance);
        SaveData.coin = Mathf.Max(LiveData.coin, SaveData.coin);

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            PlayerPrefs.SetFloat("distance", Instance.saveData.gameData.distance);
            PlayerPrefs.SetInt("coin", Instance.saveData.gameData.coin);
        }
        else
        {
            bool saveReturn = false;
            int saveTry = 0;
            while (!saveReturn && saveTry < Instance.maxSaveRetry)
            {
                saveReturn = SaveLoadManager.Save("GameData", Instance.saveData.gameData);
                saveTry++;
            }
            if (saveReturn)
            {
                Debug.LogWarning("Data Save Success.");
            }
            else
            {
                SaveBackup.CopyTo(SaveData);
                Debug.LogError("Data Save Fail.");
            }
        }
    }

    private void LoadGameData()
    {
        bool loadReturn = false;
        GameData loadData = new GameData();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            loadData.distance = PlayerPrefs.GetFloat("distance", defaultData.gameData.distance);
            loadData.coin = PlayerPrefs.GetInt("coin", defaultData.gameData.coin);
            loadReturn = true;
        }
        else
        {
            int loadTry = 0;
            while (!loadReturn && loadTry < maxLoadRetry)
            {
                try
                {
                    loadData = SaveLoadManager.Load("GameData") as GameData;
                    if (loadData == null)
                    {
                        loadData = new GameData();
                        loadData.distance = defaultData.gameData.distance;
                        loadData.coin = defaultData.gameData.coin;
                    }

                    loadReturn = true;
                }
                catch
                {
                    loadReturn = false;
                }
                loadTry++;
            }
        }
        if (loadReturn)
        {
            saveData.gameData = loadData;
            saveData.CopyTo(liveData);
            Debug.LogWarning("Data Load Success.");
        }
        else
        {
            Debug.LogError("Data Load Fail.");
        }
    }
}
