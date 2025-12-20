using UnityEngine;

/// <summary>
/// Дверь, которую можно открыть с помощью ключевого предмета
/// </summary>
public class OpenDoor : HintInteractionItem
{
    [Header("Door Settings")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openAnimationTrigger = "Open";
    [SerializeField] private AudioClip doorOpenSound;

    private bool _isOpen = false;

    public bool IsOpen => _isOpen;

    private void Start()
    {
        base.Startup();
    }

    public bool TryOpen(ItemSO keyItem)
    {
        if (keyItem == null)
        {
            return false;
        }

        if (!keyItem.isUsable || keyItem.itemType != ItemType.Key)
        {
            return false;
        }

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(openAnimationTrigger);
        }

        if (doorOpenSound != null)
        {
            AudioSource.PlayClipAtPoint(doorOpenSound, transform.position);
        }

        CloseViewInteraction();
        _isOpen = true;

        return true;
    }

}
