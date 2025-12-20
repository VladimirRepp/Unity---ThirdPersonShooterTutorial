using UnityEngine;


/// <summary>
/// Базовый класс для всех предметов, с которыми можно взаимодействовать в игре для отображения UI и определения радиуса взаимодействия
/// </summary>
public class HintInteractionItem : MonoBehaviour
{
    [Header("Hint Interaction Item Settings")]
    [SerializeField] protected float interactionRange = 2f;
    [SerializeField] protected GameObject hintInteractionUI;

    protected SphereCollider _interactionCollider;
    protected bool _canViewInteraction = true;

    protected void Startup()
    {
        // Отключаем UI взаимодействия
        if (hintInteractionUI != null)
        {
            hintInteractionUI.SetActive(false);
        }

        _interactionCollider = GetComponent<SphereCollider>();
        _interactionCollider.radius = interactionRange;
    }

    protected void CloseViewInteraction()
    {
        if (hintInteractionUI != null)
        {
            hintInteractionUI.SetActive(false);
        }

        _canViewInteraction = false;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintInteractionUI != null && _canViewInteraction)
            {
                hintInteractionUI.SetActive(true);
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hintInteractionUI != null && _canViewInteraction)
            {
                hintInteractionUI.SetActive(false);
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        // Визуализация радиуса взаимодействия
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
