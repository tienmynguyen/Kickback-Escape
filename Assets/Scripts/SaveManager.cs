
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveManager : MonoBehaviour
{
    public int currentSaveSlot = -1;
    public int level = -1;
    public static SaveManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Save(int level)
    {


        GameData data = new GameData()
        {
            level = level,

        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"SaveSlot_{currentSaveSlot}", json);
        PlayerPrefs.Save();
        this.level = level;
    }


    public void Load(int slot)
    {
        currentSaveSlot = slot;
        string json = PlayerPrefs.GetString($"SaveSlot_{slot}", "");
        if (string.IsNullOrEmpty(json))
        {
            SceneManager.LoadScene("CutSceneIntro");

        }
        else
        {
            GameData data = JsonUtility.FromJson<GameData>(json);

            Debug.Log($"Loaded from slot {slot}, Level: {data.level}");
            MusicManager.Instance.PlayMusic("Game");

            SceneManager.LoadScene("level " + data.level.ToString());
        }


    }
    public bool HasSaveSlot(int slot)
    {
        return PlayerPrefs.HasKey($"SaveSlot_{slot}");
    }
    public GameData LoadGameData(int slot)
    {
        if (!HasSaveSlot(slot)) return null;

        string json = PlayerPrefs.GetString($"SaveSlot_{slot}");
        return JsonUtility.FromJson<GameData>(json);
    }
    public void DeleteSaveSlot(int slot)
    {
        string key = $"SaveSlot_{slot}";
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"Slot {slot} deleted.");
        }


    }



}
