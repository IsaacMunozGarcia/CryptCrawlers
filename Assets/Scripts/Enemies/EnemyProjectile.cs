using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private Vector3 initDirection;
    [SerializeField] private float travelTime;
    private float timer;
    private Vector3 currentDirection;
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate ( initDirection * speed * Time.deltaTime);
        if (timer >= travelTime)
        {
            Destroy(gameObject);
        }
    }
}
