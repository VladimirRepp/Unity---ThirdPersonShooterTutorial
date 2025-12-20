using System;
using UnityEngine;

public interface IEnemyZombieState
{
    void EnterState(HardEnemyBehavior enemy, Animator animator = null);
    void UpdateState(HardEnemyBehavior enemy = null);   // todo: можно реализовать через события
    void ExitState(HardEnemyBehavior enemy = null);     // todo: можно реализовать через события
}
