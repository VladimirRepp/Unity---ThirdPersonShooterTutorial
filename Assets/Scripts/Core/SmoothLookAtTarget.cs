using UnityEngine;

public class SmoothLookAtTarget : MonoBehaviour
{
    [Header("Настройки цели")]
    [SerializeField] private Transform target;
    [SerializeField] private bool useMainCameraAsTarget = false;

    [Header("Настройки поворота")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private bool rotateOnlyYAxis = false;
    [SerializeField] private bool lookAwayFromTarget = false;

    [Header("Смещение взгляда")]
    [SerializeField] private Vector3 lookOffset = Vector3.zero;

    private Quaternion targetRotation;
    private Vector3 targetDirection;

    private void Start()
    {
        if (useMainCameraAsTarget && Camera.main != null)
        {
            target = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (target == null) return;

        SmoothLookAt();
    }

    private void SmoothLookAt()
    {
        // Вычисляем направление к цели
        Vector3 targetPos = target.position + lookOffset;
        targetDirection = targetPos - transform.position;

        if (lookAwayFromTarget)
        {
            targetDirection = -targetDirection;
        }

        // Если нужно поворачивать только по Y оси
        if (rotateOnlyYAxis)
        {
            targetDirection.y = 0;
        }

        // Вычисляем целевой поворот
        if (targetDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(targetDirection * -1f);

            // Плавный поворот
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    // Резкий поворот (без плавности)
    public void SnapToTarget()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + lookOffset;
        Vector3 direction = targetPos - transform.position;

        if (rotateOnlyYAxis)
        {
            direction.y = 0;
        }

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    // Установить новую цель
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Включить/выключить поворот
    public void SetRotationEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position + lookOffset);
            Gizmos.DrawWireSphere(target.position + lookOffset, 0.2f);
        }
    }
}