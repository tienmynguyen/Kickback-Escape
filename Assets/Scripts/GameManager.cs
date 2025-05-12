
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RectTransform cursorRect;
    [Range(0f, 1f)] public float opacity = 0.5f;
    [SerializeField] private Transform player; // Tham chiếu đến nhân vật
    [SerializeField] private float minDistance = 1f; // Khoảng cách tối thiểu (để chuột có màu xanh)
    [SerializeField] private float maxDistance = 10f; // Khoảng cách tối đa (để chuột có màu đỏ)

    void Start()
    {
        Cursor.visible = false;
        SetOpacity(opacity);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        cursorRect.position = mousePos;

        UpdateCursorColor();
    }
    void SetOpacity(float alpha)
    {
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
        // Lấy vị trí chuột trong thế giới
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // Loại bỏ Z vì game 2D

        // Lấy vị trí nhân vật
        Vector3 playerPos = player.position;

        // Tính khoảng cách
        float distance = Vector3.Distance(playerPos, mouseWorldPos);

        // Chuyển thành giá trị từ 0 đến 1 (0: gần, 1: xa)
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);

        // Màu từ xanh (gần) đến đỏ (xa)
        Color color = Color.Lerp(Color.green, Color.red, t);
        color.a = opacity; // giữ độ mờ như bạn đã đặt

        // Gán màu cho ảnh
        var image = cursorRect.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }
    }
}
