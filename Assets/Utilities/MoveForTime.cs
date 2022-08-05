using UnityEngine;

public class MoveForTime : MonoBehaviour
{
    public float time = 30f;
    public float speed = 10f;
    public Vector3 direction = Vector3.up;
    public float initialDelay = 0f;

    private float _timeSinceSpawn = 0f;

    private void Awake()
    {
        Destroy(gameObject, time + initialDelay);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _timeSinceSpawn += Time.deltaTime;

        if (_timeSinceSpawn >= initialDelay)
            transform.position += speed * direction * Time.deltaTime;
    }
}