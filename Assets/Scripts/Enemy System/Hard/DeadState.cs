using UnityEngine;

public class DeadState : MonoBehaviour, IEnemyZombieState
{
    public void EnterState(HardEnemyBehavior enemy, Animator animator)
    {
        // Сбросить все параметры перед смертью
        animator.ResetTrigger("TakeDamage");
        animator.ResetTrigger("Attack");
        animator.SetFloat("Speed", 0f);

        // Запустить смерть через триггер
        animator.SetTrigger("Die");
    }

    public void ExitState(HardEnemyBehavior enemy)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
    }
}
