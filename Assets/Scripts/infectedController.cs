using UnityEngine;
using UnityEngine.Tilemaps;

public class infectedController : MonoBehaviour
{
    public Tilemap infectedTilemap;
    public TileBase infectedTile;

    public void InfectedCell(Vector3Int cellPos)
    {
        infectedTilemap.SetTile(cellPos, infectedTile);
    }

    public bool isCellInfected(Vector3Int cellpos)
    {
        return infectedTilemap.GetTile(cellpos) != null;
    }
}
