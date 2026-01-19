using UnityEngine;
using System.Collections;

/// <summary>
/// Только визуальная реализация состояния получения урона
/// Логика получения урона реализована в классе DamageableObject
/// Наносит урон тот, кто вызывает метод TakeDamage у DamageableObject
/// </summary>
public class TakeDamageState : MonoBehaviour, IEnemyZombieState
{
    private Coroutine _waitAndChangeStateCoroutine = null;
    private HardEnemyBehavior _controller;
    private Transform _currentTarget;

    public void EnterState(HardEnemyBehavior enemy, Animator animator)
    {
        _controller = enemy;
        _currentTarget = enemy.Target?.transform;
        _controller.IsLockChangeState = true;

        animator.SetFloat("Speed", -1f);
        animator.SetTrigger("TakeDamage");

        if (_waitAndChangeStateCoroutine != null)
        {
            StopCoroutine(_waitAndChangeStateCoroutine);
            _waitAndChangeStateCoroutine = null;
        }

        // TODO: подправить время ожидания в зависимости от анимации урона
        _waitAndChangeStateCoroutine = StartCoroutine(WaitAndChangeStateCoroutine(1f,
        _currentTarget != null ? EStateEnemy.Attack : EStateEnemy.Patrol));
    }

    public void ExitState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator WaitAndChangeStateCoroutine(float waitTime, EStateEnemy newState)
    {
        yield return new WaitForSeconds(waitTime);
        _controller.IsLockChangeState = false;
        _controller.StateChanges(newState);
    }
}
