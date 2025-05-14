
using UnityEditor;
using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    public float speed;
    Vector3 targetPos;
    private LineRenderer lineRenderer;
    public GameObject ways;
    public Transform[] wayPoints;
    int pointIndex;
    int pointCount;
    int direction = 1;

    private void Awake()
    {
        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }
    private void Start()
    {
        pointCount = wayPoints.Length;
        pointIndex = 1;
        targetPos = wayPoints[pointIndex].transform.position;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointCount;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // hoặc Legacy Shaders/Particles/Alpha Blended
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    private void Update()
    {

        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        if (transform.position == targetPos)
        {
            NextPoint();
        }
        for (int i = 0; i < pointCount; i++)
        {
            lineRenderer.SetPosition(i, wayPoints[i].position);
        }
    }

    void NextPoint()
    {
        if (pointIndex == pointCount - 1)
        {
            direction = -1;
        }
        if (pointIndex == 0)
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;


    }

}
