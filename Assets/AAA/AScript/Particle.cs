using UnityEngine;

public class Particle : MonoBehaviour
{
    private Particle particle;
    private void Awake()
    {
        particle = GetComponent<Particle>();
    }
    private void Start()
    {
        particle.Start();
        Destroy(particle.gameObject, particleI.main.duration);
    }
}
