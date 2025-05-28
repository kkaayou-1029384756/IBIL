using UnityEngine;

public class TowerHP : MonoBehaviour
{
    [SerializeField] ParticleSystem particlePrefab;

    public int maxHp;
    public int currentHp;
    protected virtual void Start()
    {
        currentHp = maxHp;
    }
    public void Attack(int damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            //여기서 타워 디짐
            ParticleSystem particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            particleInstance.Play();
            Destroy(particleInstance.gameObject, particleInstance.main.duration);
            Destroy(gameObject);
        }
    }
}
