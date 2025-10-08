using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System.Collections;

public class GridPlayer : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap groundTilemap;   // тайлы, по которым можно ходить
    public Tilemap waterTilemap;    // тайлы, по которым нельзя ходить

    [Header("Movement Settings")]
    public float moveSpeed = 5f;    // скорость движения
    public int maxMoveDistance = 3; // максимальное количество клеток за ход

    [Header("Components")]
    public Animator animator;       // Animator с Idle, Enter и Exit
    private SpriteRenderer spriteRenderer; // спрайт игрока для скрытия под землей

    // Внутренние переменные
    private bool isMoving = false;      // true, если игрок сейчас движется
    private Vector3Int pendingCell;     // целевая клетка
    private bool pendingMove = false;   // флаг, что движение запланировано
    public infectedController infectionController;
    private Vector3Int currentCell;

    void Start()
    {
        // Получаем компонент спрайта и Animator, если не назначен
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();
        currentCell = groundTilemap.WorldToCell(transform.position);
    }

    // Метод для обработки клика мыши через новую систему Input
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed || isMoving || pendingMove)
            return;

        // Получаем позицию курсора в мире
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0;

        // Определяем клетку, на которую кликнули
        Vector3Int cellPos = groundTilemap.WorldToCell(mouseWorldPos);

        // Проверяем, что тайл существует и не вода
        if (!groundTilemap.HasTile(cellPos) || waterTilemap.HasTile(cellPos))
            return;

        // Проверяем дистанцию (только прямые линии, без диагоналей)
        Vector3Int currentCell = groundTilemap.WorldToCell(transform.position);
        int deltaX = Mathf.Abs(cellPos.x - currentCell.x);
        int deltaY = Mathf.Abs(cellPos.y - currentCell.y);
        if ((deltaX != 0 && deltaY != 0) || (deltaX + deltaY > maxMoveDistance))
            return;

        // Запоминаем клетку и запускаем корутину движения
        pendingCell = cellPos;
        StartCoroutine(MoveWithDig(pendingCell));
    }

    // Основная корутина для движения и анимаций
    private IEnumerator MoveWithDig(Vector3Int cellPos)
    {
        // 1️⃣ Погружение под землю (Enter анимация)
        animator.SetBool("isUnderground", true);
        yield return new WaitForSeconds(0.5f); // ждем конца анимации Enter

        // 2️⃣ Перемещение под землей (спрайт невидимый)
        spriteRenderer.color = new Color(1, 1, 1, 0); // делаем игрока прозрачным
        Vector3 targetPos = groundTilemap.GetCellCenterWorld(cellPos);
        targetPos.z = 0; // всегда 0 по Z, чтобы не было проблем с порядком слоев

        // Плавное движение к целевой клетке
        while ((transform.position - targetPos).sqrMagnitude > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null; // ждем следующий кадр
        }

        // 3️⃣ Появление и анимация выкапывания (Exit)
        spriteRenderer.color = new Color(1, 1, 1, 1); // показываем игрока
        animator.Play("Exit"); // проигрываем Exit клип
        yield return new WaitForSeconds(0.5f); // длительность Exit (подстрой под свою анимацию)

        // 4️⃣ Возврат в Idle
        animator.SetBool("isUnderground", false);

        Vector3Int finalCell = groundTilemap.WorldToCell(transform.position);
        infectionController.InfectedCell(finalCell);
    }
}
