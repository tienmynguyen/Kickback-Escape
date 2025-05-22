using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelManager : MonoBehaviour
{
    [Header("Thông tin Level")]
    public int levelNumber;
    public TMP_Text level;
    public TMP_Text deathsCount;
    private void Start()
    {
        // Cập nhật level hiện tại vào SaveManager (nếu cần)
        SaveManager.Instance.level = levelNumber;
        level.text = "Level " + SaveManager.Instance.level.ToString();
    }
    private void Update()
    {
        deathsCount.text = SaveManager.Instance.GetCurrentLevelDeathCount().ToString();
    }
}
