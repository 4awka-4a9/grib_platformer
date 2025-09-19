using Unity.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float movingSpeed = 5f;

    [Header("Y sensitivity")]
    [SerializeField] private float yDeadZone = 1f; // допустимое отклонение по Y
    [SerializeField] private float yLerpSpeed = 2f; // скорость, с которой камера подтягивает Y

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // Базовая цель: камера следует по X всегда
        float targetX = playerTransform.position.x;
        float targetY = transform.position.y; // по умолчанию фиксируем Y
        float targetZ = -10f;

        // Проверяем, вышел ли игрок за пределы "мёртвой зоны" по Y
        float yDelta = playerTransform.position.y - transform.position.y;

        if (Mathf.Abs(yDelta) > yDeadZone)
        {
            // Плавно подтягиваем Y, если игрок сильно ушёл
            targetY = Mathf.Lerp(transform.position.y, playerTransform.position.y, yLerpSpeed * Time.deltaTime);
        }

        Vector3 target = new Vector3(targetX, targetY, targetZ);
        transform.position = Vector3.Lerp(transform.position, target, movingSpeed * Time.deltaTime);
    }
}
