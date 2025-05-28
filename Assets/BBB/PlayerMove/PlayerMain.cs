using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMain : MonoBehaviour
{
    [Header("Path Movement")]
    [SerializeField] Transform[] _movePoints;
    public float _moveSpeed = 3f;

    [Header("Charge Settings")]
    [SerializeField] Slider _chargeSlider;
    [SerializeField] float _maxCharge = 5f;
    public float _chargeSpeed = 2f;

    [Header("Launch Settings")]
    public float _launchDistance = 4f;
    [SerializeField] float _launchSpeed = 20f;
    [SerializeField] float _returnSpeed = 10f;

    int _currentIndex = 0;
    bool _isMoving = false;
    Vector3 _targetPosition;

    bool _isCharging = false;
    float _chargeAmount = 0f;

    enum LaunchState { Idle, Launching, Returning, WaitingToReturn }
    LaunchState _launchState = LaunchState.Idle;

    Vector3 _returnPosition;
    Vector3 _launchTarget;
    float _launchRatio;

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
        LookAtMouse();

        switch (_launchState)
        {
            case LaunchState.Launching:
                HandleLaunching();
                return;

            case LaunchState.Returning:
                HandleReturning();
                return;

            case LaunchState.WaitingToReturn:
                return;
        }

        HandleCharging();

        if (!_isMoving && _launchState == LaunchState.Idle)
        {
            if (Input.GetKey(KeyCode.A)) StartCoroutine(MoveStep(-1));
            else if (Input.GetKey(KeyCode.D)) StartCoroutine(MoveStep(1));
        }
    }

    IEnumerator MoveStep(int direction)
    {
        _isMoving = true;

        int nextIndex = _currentIndex + direction;
        if (nextIndex >= 0 && nextIndex < _movePoints.Length)
        {
            _currentIndex = nextIndex;
            _targetPosition = _movePoints[_currentIndex].position;

            while (Vector3.Distance(transform.position, _targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = _targetPosition;
        }

        _isMoving = false;
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
                PrepareLaunch();
        }
    }

    void PrepareLaunch()
    {
        _launchRatio = Mathf.Clamp01(_chargeAmount / _maxCharge);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (mousePos - transform.position).normalized;

        _launchTarget = transform.position + dir * _launchDistance * _launchRatio;
        _returnPosition = transform.position;
        _launchState = LaunchState.Launching;
    }

    void HandleLaunching()
    {
        float dist = Vector3.Distance(transform.position, _launchTarget);
        if (dist < 0.05f)
        {
            transform.position = _launchTarget;
            StartCoroutine(DelayBeforeReturn());
            _launchState = LaunchState.WaitingToReturn;
        }
        else
        {
            float t = dist / (_launchDistance * _launchRatio);
            float speed = Mathf.Lerp(5f, _launchSpeed, t);
            transform.position = Vector3.MoveTowards(transform.position, _launchTarget, speed * Time.deltaTime);
        }
    }

    IEnumerator DelayBeforeReturn()
    {
        yield return new WaitForSeconds(0.1f);
        _launchState = LaunchState.Returning;
    }

    void HandleReturning()
    {
        float dist = Vector3.Distance(transform.position, _returnPosition);
        if (dist < 0.05f)
        {
            transform.position = _returnPosition;
            _launchState = LaunchState.Idle;
            _chargeAmount = 0f;
            if (_chargeSlider != null)
                _chargeSlider.value = 0f;
        }
        else
        {
            float t = 1f - (dist / (_launchDistance * _launchRatio));
            float speed = Mathf.Lerp(2f, _returnSpeed, t);
            transform.position = Vector3.MoveTowards(transform.position, _returnPosition, speed * Time.deltaTime);
        }
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
