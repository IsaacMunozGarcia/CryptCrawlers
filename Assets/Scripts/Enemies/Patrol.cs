using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform patrolPath;
    [SerializeField] private float speed = 2f;

    private readonly List<Vector3> patrolPoints = new();
    private int currentIndex;

    private void Awake()
    {
        foreach (Transform point in patrolPath)
        {
            patrolPoints.Add(point.position);
        }
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentIndex], speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, patrolPoints[currentIndex]) < 0.01f)
        {
            SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        currentIndex = (currentIndex + 1) % patrolPoints.Count;
        transform.eulerAngles = transform.position.x > patrolPoints[currentIndex].x ? new Vector3(0f, 180f, 0f) : Vector3.zero;
    }
}