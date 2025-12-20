using UnityEngine;

public class PatrolState : MonoBehaviour, IEnemyZombieState
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform[] patrolPoints;

    private bool _canPatrol = false;
    private int _currentPatrolIndex = 0;

    public void EnterState(HardEnemyBehavior enemy, Animator animator)
    {
        _canPatrol = true;
        animator.SetFloat("Speed", 1f);
        animator.ResetTrigger("Attack");
    }

    public void ExitState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState(HardEnemyBehavior enemy = null)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (_canPatrol)
            Patrolling();
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
}
