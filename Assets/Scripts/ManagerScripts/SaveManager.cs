
using System;
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
    public void AddDeath()
    {
        Debug.Log("AddDeath called");
        // Load dữ liệu hiện tại
        GameData gameData = LoadGameData(currentSaveSlot);
        if (gameData == null)
        {
            gameData = new GameData();
            gameData.deathByLevel = new DeathByLevel[0];
        }

        gameData.deathCount++; // Tổng số lần chết

        bool levelFound = false;

        for (int i = 0; i < gameData.deathByLevel.Length; i++)
        {
            if (gameData.deathByLevel[i].level == level)
            {
                gameData.deathByLevel[i].deathCount++;
                levelFound = true;
                break;
            }
        }

        if (!levelFound)
        {
            DeathByLevel newDeath = new DeathByLevel
            {
                level = level,
                deathCount = 1
            };

            int oldLength = gameData.deathByLevel.Length;
            DeathByLevel[] updated = new DeathByLevel[oldLength + 1];
            gameData.deathByLevel.CopyTo(updated, 0);
            updated[oldLength] = newDeath;
            gameData.deathByLevel = updated;
        }

        // Lưu lại dữ liệu mới
        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString($"SaveSlot_{currentSaveSlot}", json);
        PlayerPrefs.Save();
    }

    public int GetCurrentLevelDeathCount()
    {
        GameData data = LoadGameData(currentSaveSlot);
        if (data == null || data.deathByLevel == null)
            return 0;

        foreach (var d in data.deathByLevel)
        {
            if (d.level == level) // 'level' là level hiện tại đã lưu
                return d.deathCount;
        }

        return 0; // Nếu chưa có dữ liệu level hiện tại
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
