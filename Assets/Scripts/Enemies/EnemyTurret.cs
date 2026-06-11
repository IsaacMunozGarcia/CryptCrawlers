using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] private Transform shootpoint;
    [SerializeField] private float firerate;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= firerate)
        {
            Instantiate(projectile, shootpoint.position, shootpoint.rotation);
            timer = 0;
        }
    }

}
