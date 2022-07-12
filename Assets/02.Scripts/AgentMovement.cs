using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AgentMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [Range(0, 10)]
    public float maxSpeed = 5;

    [Range(0.1f, 100f)]
    public float acceleration = 50, deAcceleration = 50;

    protected float _currentVelocity = 3f;
    protected Vector2 _movementDirection;

    public UnityEvent<float> OnVelocityChange; //�÷��̾� �ӵ��� �ٲ� ����� �̺�Ʈ
    
    #region �˹� ���� �Ķ����
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
        OnVelocityChange?.Invoke(_currentVelocity);

        if (!_isKnockback)
            _rigidbody.AddForce(new Vector2(_movementDirection.x * _currentVelocity, 0));
    }


    //�˹鱸���� �� ����� �Ŵ�.
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
