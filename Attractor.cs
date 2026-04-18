using UnityEngine;

public class Attractor : MonoBehaviour
{
    public static Vector3 POS = Vector3.zero;

    [Header("Orbit Settings")]
    [Min(0f)] public float radius = 10f;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    void Awake()
    {
        UpdatePosition();
    }

    void OnValidate()
    {
        radius = Mathf.Max(0f, radius);
        UpdatePosition();
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        Vector3 scale = transform.localScale;
        Vector3 nextPos = new Vector3(
            Mathf.Sin(xPhase * Time.time) * radius * scale.x,
            Mathf.Sin(yPhase * Time.time) * radius * scale.y,
            Mathf.Sin(zPhase * Time.time) * radius * scale.z
        );

        transform.position = nextPos;
        POS = nextPos;
    }
}
