using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class AgentMovement : MonoBehaviour
{

    private Rigidbody2D _rigidbody;
    private float _offsetSpeedValue = 1;
    [Range(0, 10)]
    public float maxSpeed = 5;

    [Range(0.1f, 100f)]
    public float acceleration = 50, deAcceleration = 50;

    protected float _currentVelocity = 3f;
    protected Vector2 _movementDirection;
    protected bool _isSlowness = false;
    public UnityEvent<float> OnVelocityChange; //플레이어 속도가 바뀔때 실행될 이벤트
    
    #region 넉백 관련 파라메터
    [SerializeField]
    protected bool _isKnockback = false;
    protected Coroutine _knockbackCo = null;
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void MoveAgent(Vector2 movementInput)
    {

         if (movementInput.sqrMagnitude > 0)
        {
            if (Vector2.Dot(movementInput, _movementDirection) < 0)
            {
                _currentVelocity = 0;
            }
            _movementDirection = movementInput.normalized;
        }
        _currentVelocity = CalculateSpeed(movementInput);
    }

    private float CalculateSpeed(Vector2 movementInput)
    {
        if (movementInput.sqrMagnitude > 0)
        {
            _currentVelocity += acceleration * Time.deltaTime;
        }
        else
        {
            _currentVelocity -= deAcceleration * Time.deltaTime;
        }

        return Mathf.Clamp(_currentVelocity, 0, maxSpeed);
    }

    private void FixedUpdate()
    {
        if (GameManager.Inst.gameState == GameManager.GameState.Throwing)
            _currentVelocity = 0f;
        OnVelocityChange?.Invoke(_currentVelocity);

        if (!_isKnockback)
            _rigidbody.velocity = new Vector2(_movementDirection.x * _currentVelocity * _offsetSpeedValue, _rigidbody.velocity.y);
    }

    public void SwampStateEnemyRun(float percent, float duration)
    {
        if (_isSlowness) return;
        StartCoroutine(SlownessCoroutine(percent, duration));
    }

    public IEnumerator SlownessCoroutine(float percent, float duration)
    {
        _isSlowness = true;
        _offsetSpeedValue = 1 - percent;
        yield return new WaitForSeconds(duration);
        _offsetSpeedValue = 1;
        _isSlowness = false;
    }
    //넉백구현할 때 사용할 거다.
    public void StopImmediatelly()
    {
        _currentVelocity = 0;
        _rigidbody.velocity = Vector2.zero;
    }

    public void KnockBack(Vector2 dir, float power, float duration)
    {
        if (!_isKnockback)
        {
            _isKnockback = true;
            _knockbackCo = StartCoroutine(KnockbackCorutine(dir, power, duration));
        }
    }
    public IEnumerator KnockbackCorutine(Vector2 dir, float power, float duration)
    {
        _rigidbody.AddForce(dir.normalized * power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        ResetKnockbackParam();
    }
    public void ResetKnockbackParam()
    {
        _currentVelocity = 0;
        _rigidbody.velocity = Vector2.zero;
        _isKnockback = false;
        _rigidbody.gravityScale = 1;
    }
}
