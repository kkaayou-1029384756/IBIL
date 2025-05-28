using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMain : MonoBehaviour
{
    [Header("Path Movement")]
    [SerializeField] Transform[] _movePoints;
    public float _moveSpeed = 5f;

    [Header("Charge Settings")]
    [SerializeField] Slider _chargeSlider;
    [SerializeField] float _maxCharge = 5f;
    public float _chargeSpeed = 2f;

    [Header("Launch Settings")]
    public float _launchDistance = 5f;      // 튀어나가는 거리 (고정)
    [SerializeField] float _launchSpeed = 20f;        // 튀어나가는 속도
    [SerializeField] float _returnSpeed = 10f;        // 돌아오는 속도

    int _currentIndex = 0;
    bool _isMoving = false;
    Vector3 _targetPosition;

    bool _isCharging = false;
    float _chargeAmount = 0f;
    bool _isLaunching = false;
    Vector3 _returnPosition;

    void Start()
    {
        _currentIndex = FindNearestPointIndex();
        transform.position = _movePoints[_currentIndex].position;
        _targetPosition = _movePoints[_currentIndex].position;

        if (_chargeSlider != null)
        {
            _chargeSlider.minValue = 0f;
            _chargeSlider.maxValue = _maxCharge;
            _chargeSlider.value = 0f;
        }
    }

    void Update()
    {
        if (_isLaunching)
        {
            LookAtMouse();
            return;
        }

        HandleInput();
        MovePlayer();
        LookAtMouse();
        HandleCharging();
    }

    void HandleInput()
    {
        if (_isMoving) return;

        if (Input.GetKey(KeyCode.A))
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                SetTargetPosition(_movePoints[_currentIndex].position);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (_currentIndex < _movePoints.Length - 1)
            {
                _currentIndex++;
                SetTargetPosition(_movePoints[_currentIndex].position);
            }
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            int nearest = FindNearestPointIndex();
            _currentIndex = nearest;
            SetTargetPosition(_movePoints[_currentIndex].position);
        }
    }

    void MovePlayer()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > 0.01f)
        {
            _isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = _targetPosition;
            _isMoving = false;
        }
    }

    void LookAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        direction.z = 0f;
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.up = direction.normalized;
        }
    }

    void HandleCharging()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _isCharging = true;
            _chargeAmount += _chargeSpeed * Time.deltaTime;
            _chargeAmount = Mathf.Clamp(_chargeAmount, 0f, _maxCharge);

            if (_chargeSlider != null)
                _chargeSlider.value = _chargeAmount;
        }

        if (_isCharging && Input.GetKeyUp(KeyCode.Space))
        {
            _isCharging = false;
            if (_chargeAmount > 0f)
            {
                StartCoroutine(LaunchAndReturn());
            }
        }
    }

    IEnumerator LaunchAndReturn()
    {
        _isLaunching = true;
        _returnPosition = transform.position;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (mousePos - transform.position).normalized;

        float ratio = _chargeAmount / _maxCharge;
        float actualDistance = _launchDistance * ratio;
        Vector3 launchTarget = transform.position + dir * actualDistance;

        float launchStartDistance = Vector3.Distance(transform.position, launchTarget);
        float returnStartDistance = Vector3.Distance(launchTarget, _returnPosition);

        float _minLaunchSpeed = 5f;  // 가까워질수록 속도 줄어드는 최소 속도
        float _minReturnSpeed = 2f;  // 돌아올 때 초반 느린 최소 속도

        // 빠르게 튀어나가기 → 가까워질수록 감속
        while (Vector3.Distance(transform.position, launchTarget) > 0.05f)
        {
            float currentDistance = Vector3.Distance(transform.position, launchTarget);
            float t = currentDistance / launchStartDistance; // 1 → 0으로 변함
            float speed = Mathf.Lerp(_minLaunchSpeed, _launchSpeed, t); // 멀면 빠름, 가까우면 느림
            transform.position = Vector3.MoveTowards(transform.position, launchTarget, speed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // 찰진 멈춤

        // 되돌아오기 → 처음엔 느리게 출발해서 가까워질수록 빨라짐
        while (Vector3.Distance(transform.position, _returnPosition) > 0.05f)
        {
            float currentDistance = Vector3.Distance(transform.position, _returnPosition);
            float t = 1f - (currentDistance / returnStartDistance); // 0 → 1로 변함
            float speed = Mathf.Lerp(_minReturnSpeed, _returnSpeed, t);
            transform.position = Vector3.MoveTowards(transform.position, _returnPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = _returnPosition;
        _chargeAmount = 0f;
        if (_chargeSlider != null)
            _chargeSlider.value = 0f;
        _isLaunching = false;
    }



    void SetTargetPosition(Vector3 newTarget)
    {
        _targetPosition = newTarget;
    }

    int FindNearestPointIndex()
    {
        int nearest = 0;
        float minDist = float.MaxValue;
        for (int i = 0; i < _movePoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, _movePoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = i;
            }
        }
        return nearest;
    }
}
