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
        _target = GameObject.FindGameObjectWithTag("Tower").transform;
    }

    private void Update()
    {
        //�ٶ󺼶�
        Vector3 direction = _target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //ȸ���ڵ�

        _moveDir = (_target.position - transform.position).normalized;
        _rb.linearVelocity = _moveDir * _speed * stop;
    }
}
