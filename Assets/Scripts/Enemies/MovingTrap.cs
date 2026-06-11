using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private Vector3 initDirection;
    [SerializeField] private float travelTime;
    private float timer;
    private Vector3 currentDirection;
    void Start()
    {
        currentDirection = initDirection;
    }
    

    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate ( currentDirection * speed * Time.deltaTime);
        if (timer >= travelTime)
        {
            currentDirection *= -1;
            timer = 0;
        }
    }
}