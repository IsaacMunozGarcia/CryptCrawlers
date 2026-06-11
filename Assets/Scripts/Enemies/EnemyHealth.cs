using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private GameObject deathDrop;
    [SerializeField] private ParticleSystem damageParticles;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            TakeDamage(1);
            SoundEffectManager.Play("Enemy");
            damageParticles.Play();
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathDrop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
}