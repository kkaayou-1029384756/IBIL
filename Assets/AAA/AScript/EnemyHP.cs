using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    public int maxHealth;
    public int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(int damage, GameObject target)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            ParticleSystem particleInstance = Instantiate(particle, transform.position, Quaternion.identity);
            particleInstance.Play();
            Destroy(particleInstance.gameObject, particleInstance.main.duration);
            Destroy(gameObject);
        }
    }
}
