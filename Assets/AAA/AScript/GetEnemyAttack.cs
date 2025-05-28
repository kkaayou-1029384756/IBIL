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

    public float pushForce = 20f;         // 강하게 밀리도록 초기 속도 증가
    public float decelerationRate = 1.2f; // 감속을 느리게 하기 위한 값

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
            movement.stop = 0f;  // 이동 멈추기

            Vector2 pushDir = (rb.position - (Vector2)collision.transform.position).normalized;

            // AddForce를 통해 즉시 밀어내기 시작
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

            // PushBack 과정 시작
            StartCoroutine(PushBackGradually(pushDir));
        }

        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerHP>().Attack(1)  ;
            isPushing = true;
            movement.stop = 0f;  // 이동 멈추기

            Vector2 pushDir = (rb.position - (Vector2)collision.transform.position).normalized;

            // AddForce를 통해 즉시 밀어내기 시작
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

            // PushBack 과정 시작
            StartCoroutine(PushBackGradually(pushDir));
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            isPushing = true;
            movement.stop = 0f;  // 이동 멈추기

            Vector2 pushDir = (rb.position - (Vector2)collision.transform.position).normalized;

            // AddForce를 통해 즉시 밀어내기 시작
            rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);

            // PushBack 과정 시작
            StartCoroutine(PushBackGradually(pushDir));
        }
    }

    IEnumerator PushBackGradually(Vector2 direction)
    {
        float currentSpeed = pushForce;

        // 감속이 점점 느리게 진행되도록 설정
        while (currentSpeed > 0.5f)  // 거의 멈췄을 때 종료 기준
        {
            rb.MovePosition(rb.position + direction * currentSpeed * Time.deltaTime); // 강제로 위치 이동
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * decelerationRate); // 부드럽게 감속
            yield return null;
        }

        // 감속 후 파티클 생성 및 오브젝트 제거
        ParticleSystem particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        particleInstance.Play();
        Destroy(particleInstance.gameObject, particleInstance.main.duration);

        Destroy(gameObject);
    }
}
