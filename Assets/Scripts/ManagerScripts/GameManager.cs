using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public RectTransform cursorRect;
    [Range(0f, 1f)] public float opacity = 0.5f;
    [SerializeField] private Transform player;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] GameObject pauseMenu;

    public static GameManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.visible = false;
        SetOpacity(opacity);
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (cursorRect == null || player == null) return;

        Vector2 mousePos = Input.mousePosition;
        cursorRect.position = mousePos;

        UpdateCursorColor();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    void SetOpacity(float alpha)
    {
        if (cursorRect == null) return;
        var image = cursorRect.GetComponent<Image>();
        if (image != null)
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        }
    }

    void UpdateCursorColor()
    {
        if (cursorRect == null || player == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 playerPos = player.position;
        float distance = Vector3.Distance(playerPos, mouseWorldPos);

        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        Color color = Color.Lerp(Color.green, Color.red, t);
        color.a = opacity;

        var image = cursorRect.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }
    }

    // Gán lại các đối tượng sau khi chuyển scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Tự động tìm lại các đối tượng theo tag hoặc name
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        GameObject cursorObj = GameObject.Find("Cursor"); // hoặc gán theo tag
        if (cursorObj != null) cursorRect = cursorObj.GetComponent<RectTransform>();

        SetOpacity(opacity); // Gán lại độ mờ nếu cần
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");

    }



}
