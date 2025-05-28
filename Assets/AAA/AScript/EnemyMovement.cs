using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform _target;

    private Vector2 _moveDir;

    private Rigidbody2D _rb;

    public float _speed = 3;

    public float stop = 1;

    private float rotationSpeed = 4f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (GameObject.FindGameObjectWithTag("Tower") != null)
        {
            _target = GameObject.FindGameObjectWithTag("Tower").transform;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Tower") == null)
        {
            Destroy(gameObject);
        }
        //바라볼때
        Vector3 direction = _target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //회전코드

        _moveDir = (_target.position - transform.position).normalized;
        _rb.linearVelocity = _moveDir * _speed * stop;
    }
}
