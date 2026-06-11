using Unity.Cinemachine;
using UnityEngine;

public class SwordProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    private float lifeTimer;

    private Vector2 direction;

    private void Start()
    {
        lifeTimer = lifeTime;
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}