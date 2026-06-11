using Unity.VisualScripting;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField] private ParticleSystem shieldBlock;
    [SerializeField] private Transform player;

    private void Update()
    {
        transform.position = player.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("EnemyBullet"))
        {
            shieldBlock.Play();
            SoundEffectManager.Play("Block");
            Destroy(other.gameObject);
        }
    }
}
