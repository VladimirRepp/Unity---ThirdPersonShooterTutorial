using System.Collections;
using UnityEngine;

/// <summary>
/// Все состояния врага реализованы в этом классе в отдельных методах
/// </summary>
public class SimpleEnemyBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform[] patrolPoints;

    [Header("Attack Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 10;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;

    [Header("Debug views-only")]
    [SerializeField] private EStateEnemy _currentState = EStateEnemy.Startup;

    private DamageableObject _myDamageable;
    private SphereCollider _detectionCollider;

    private int _currentPatrolIndex = 0;
    private GameObject _currentTarget = null;
    private Coroutine _attackCoroutine = null;

    private Coroutine _waitAndChangeStateCoroutine = null;
    private bool _isLockChangeState = false;

    private void Awake()
    {
        _myDamageable = GetComponent<DamageableObject>();
        _detectionCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        if (_myDamageable != null)
        {
            _myDamageable.OnDeath += HandleDeath;
            _myDamageable.OnHealthChanged += HandleHealthChanged;
        }
    }

    private void OnDisable()
    {
        if (_myDamageable != null)
        {
            _myDamageable.OnDeath -= HandleDeath;
            _myDamageable.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void Start()
    {
        StateChanges(EStateEnemy.Patrol);
        _detectionCollider.radius = attackRange;
    }

    private void Update()
    {
        if (_currentState == EStateEnemy.Patrol)
            Patrolling();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _currentTarget = other.gameObject;
            StateChanges(EStateEnemy.Attack);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            StateChanges(EStateEnemy.Patrol);
        }
    }

    private void HandleDeath()
    {
        StateChanges(EStateEnemy.Dead);
    }

    private void HandleHealthChanged(float health_value)
    {
        StateChanges(EStateEnemy.TakeDamage);
    }

    private void StateChanges(EStateEnemy newState)
    {
        if (_currentState == EStateEnemy.Dead)
            return;

        if (_isLockChangeState && newState != EStateEnemy.Dead)
            return;

        if (_currentState == newState)
            return;

        if (_currentState == EStateEnemy.Attack &&
            newState != EStateEnemy.Attack)
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _attackCoroutine = null;
            _currentTarget = null;
        }

        _currentState = newState;

        if (_currentState == EStateEnemy.TakeDamage)
        {
            TakeDamageState();
            return;
        }

        if (_currentState == EStateEnemy.Attack)
        {
            AttackState(_currentTarget);
            return;
        }

        if (_currentState == EStateEnemy.Patrol)
        {
            PatrolState();
            // Patrolling(); - called in Update
            return;
        }

        if (_currentState == EStateEnemy.Dead)
        {
            DeadState();
            return;
        }
    }

    #region States Implementations

    private void AttackState(GameObject target)
    {
        animator.SetFloat("Speed", -1f);

        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);

        _attackCoroutine = StartCoroutine(AttackRoutine(target));
    }

    private IEnumerator AttackRoutine(GameObject target)
    {
        transform.LookAt(target.transform);

        while (true)
        {
            // todo: нанести урон цели
            if (target.TryGetComponent<DamageableObject>(out DamageableObject damageable))
                damageable.TakeDamage(damage);

            // Атака с передышкой
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private void PatrolState()
    {
        animator.SetFloat("Speed", 1f);
    }

    private void Patrolling()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[_currentPatrolIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.LookAt(targetPoint);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            int prev_index = _currentPatrolIndex;
            _currentPatrolIndex = Random.Range(0, patrolPoints.Length);

            if (prev_index == _currentPatrolIndex)
                _currentPatrolIndex = (prev_index + 1) % patrolPoints.Length;
        }
    }

    private void DeadState()
    {
        _isLockChangeState = true;
        // Сбросить все параметры перед смертью
        animator.ResetTrigger("TakeDamage");
        animator.SetFloat("Speed", 0f);

        // Запустить смерть через триггер
        animator.SetTrigger("Die");

        this.enabled = false;
    }

    private void TakeDamageState()
    {
        _isLockChangeState = true;
        animator.SetFloat("Speed", -1f);
        animator.SetTrigger("TakeDamage");

        if (_waitAndChangeStateCoroutine != null)
        {
            StopCoroutine(_waitAndChangeStateCoroutine);
            _waitAndChangeStateCoroutine = null;
        }

        // TODO: подправить время ожидания в зависимости от анимации урона
        _waitAndChangeStateCoroutine = StartCoroutine(WaitAndChangeStateCoroutine(1f, _currentTarget != null ? EStateEnemy.Attack : EStateEnemy.Patrol));
    }

    private IEnumerator WaitAndChangeStateCoroutine(float waitTime, EStateEnemy newState)
    {
        yield return new WaitForSeconds(waitTime);
        _isLockChangeState = false;
        StateChanges(newState);
    }
    #endregion
}
