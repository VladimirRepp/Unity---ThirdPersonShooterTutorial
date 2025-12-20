using UnityEngine;

public class TakeDamageState : MonoBehaviour, IEnemyZombieState
{
    public void EnterState(HardEnemyBehavior enemy, Animator animator)
    {
        animator.SetFloat("Speed", -1f);
        animator.SetTrigger("TakeDamage");
    }

    public void ExitState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
    }
}
