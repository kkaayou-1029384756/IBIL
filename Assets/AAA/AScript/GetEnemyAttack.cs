using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class GetEnemyAttack : MonoBehaviour
{
    [SerializeField] ParticleSystem particlePrefab;
    SpriteRenderer spriteRenderer;

    private EnemyMovement movement;
    private Rigidbody2D rb;

    public float pushForce = 20f;         // ���ϰ� �и����� �ʱ� �ӵ� ����
    public float decelerationRate = 1.2f; // ������ ������ �ϱ� ���� ��

    private bool isPushing = false;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPushing) return;

        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Tower"))
        {
            spriteRenderer.color = Color.red;
            new WaitForSeconds(2f);
            isPushing = true;
            movement.stop = 0f;  // �̵� ���߱�

            Vector2 pushDir = (rb.position - (Vector2)collision.transform.position).normalized;

            // AddForce�� ���� ��� �о�� ����
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

            // PushBack ���� ����
            StartCoroutine(PushBackGradually(pushDir));
        }

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerHP>().Attack(1)  ;
            isPushing = true;
            movement.stop = 0f;  // �̵� ���߱�

            Vector2 pushDir = (rb.position - (Vector2)collision.transform.position).normalized;

            // AddForce�� ���� ��� �о�� ����
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

            // PushBack ���� ����
            StartCoroutine(PushBackGradually(pushDir));
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            isPushing = true;
            movement.stop = 0f;  // �̵� ���߱�

            Vector2 pushDir = (rb.position - (Vector2)collision.transform.position).normalized;

            // AddForce�� ���� ��� �о�� ����
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

            // PushBack ���� ����
            StartCoroutine(PushBackGradually(pushDir));
        }
    }

    IEnumerator PushBackGradually(Vector2 direction)
    {
        float currentSpeed = pushForce;

        // ������ ���� ������ ����ǵ��� ����
        while (currentSpeed > 0.5f)  // ���� ������ �� ���� ����
        {
            rb.MovePosition(rb.position + direction * currentSpeed * Time.deltaTime); // ������ ��ġ �̵�
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * decelerationRate); // �ε巴�� ����
            yield return null;
        }

        // ���� �� ��ƼŬ ���� �� ������Ʈ ����
        ParticleSystem particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        particleInstance.Play();
        Destroy(particleInstance.gameObject, particleInstance.main.duration);

        Destroy(gameObject);
    }
}
