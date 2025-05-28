using System.Collections;
using UnityEngine;

public class TowerHP : MonoBehaviour
{
    [SerializeField] ParticleSystem particlePrefab;
    [SerializeField] GameObject[] Hearts;
    [SerializeField] GameObject gameOver;

    public int maxHp;
    public int currentHp;
    protected virtual void Start()
    {
        currentHp = maxHp;
    }
    public void Attack(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            //���⼭ Ÿ�� ����
            ParticleSystem particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            particleInstance.Play();
            Destroy(particleInstance.gameObject, particleInstance.main.duration);
            gameOver.SetActive(true);
            gameOver.GetComponent<GameOver>().Fade();
            Destroy(gameObject);
        }

        for (int i = 0; i < maxHp - currentHp; i++)
        {
            Hearts[i].SetActive(false);
        }
    }
}
