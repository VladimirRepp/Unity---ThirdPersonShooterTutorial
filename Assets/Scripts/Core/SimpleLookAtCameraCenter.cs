using UnityEngine;

public class SimpleLookAtCameraCenter : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Получаем луч в центр экрана
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        // Вычисляем точку на луче на расстоянии 100 единиц
        Vector3 targetPoint = ray.GetPoint(100f);

        // Поворачиваем объект к этой точке
        transform.LookAt(targetPoint);
    }
}