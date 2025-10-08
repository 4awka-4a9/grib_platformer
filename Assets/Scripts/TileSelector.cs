using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class TileSelector : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap waterTilemap;
    public Transform player;       // ссылка на игрока
    public int maxMoveDistance = 3;
    public Sprite normalSprite;     // для доступных клеток
    public Sprite restrictedSprite; // для воды или недоступных
    public float moveSpeed = 10f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite;
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0;

        Vector3Int cellPos = groundTilemap.WorldToCell(mouseWorldPos);
        Vector3 targetPos = groundTilemap.GetCellCenterWorld(cellPos);
        targetPos.z = -1;

        // плавное движение селектора
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // проверяем доступность клетки
        bool canMoveHere = true;

        // тайл существует и не вода
        if (!groundTilemap.HasTile(cellPos) || waterTilemap.HasTile(cellPos))
            canMoveHere = false;

        // проверка дистанции и диагонали
        Vector3Int playerCell = groundTilemap.WorldToCell(player.position);
        int deltaX = Mathf.Abs(cellPos.x - playerCell.x);
        int deltaY = Mathf.Abs(cellPos.y - playerCell.y);
        if ((deltaX != 0 && deltaY != 0) || (deltaX + deltaY > maxMoveDistance))
            canMoveHere = false;

        // ставим спрайт
        spriteRenderer.sprite = canMoveHere ? normalSprite : restrictedSprite;
    }
}
