using System.Collections;
using UnityEngine;

public class AttackState : MonoBehaviour, IEnemyZombieState
{
    [Header("Attack Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private int damage = 10;

    private Animator _animator;
    private HardEnemyBehavior _controller;
    private Coroutine _attackCoroutine;

    public float AttackRange => attackRange;
    public string PlayerTag => playerTag;

    public void EnterState(HardEnemyBehavior enemy, Animator animator)
    {
        _animator = animator;
        _controller = enemy;

        animator.SetFloat("Speed", -1f);

        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);

        _attackCoroutine = StartCoroutine(AttackRoutine(_controller.Target));
    }

    public void ExitState(HardEnemyBehavior enemy = null)
    {
        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);
    }

    public void UpdateState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
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
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackCooldown);
        }
    }
}
