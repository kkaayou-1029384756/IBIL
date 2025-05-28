using UnityEngine;

public class TowerHP : MonoBehaviour
{
    [SerializeField] ParticleSystem particlePrefab;
    [SerializeField] GameObject gameOverUI;

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
            new WaitForSeconds(1f);
            StartCoroutine(gameOverUI.GetComponent<GameOver>().Fade());
            Destroy(particleInstance.gameObject, particleInstance.main.duration);
            Destroy(gameObject);
        }
    }
}
