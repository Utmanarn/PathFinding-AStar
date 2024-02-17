using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    [SerializeField,Tooltip("Enable if you want to pause between each step")] bool pauseAfterStep = true;
        
    [SerializeField,Tooltip("Insert Marker Prefab")] GameObject marker;
    [SerializeField,Tooltip("Insert Marker Open Material")] Material openMaterial;
    [SerializeField,Tooltip("Insert Marker Closed Material")] Material closedMaterial;
    [SerializeField,Tooltip("Insert Marker End Material")] Material endMaterial;

    Board board;
    bool done;
    
    Tile startTile;
    Tile currentTile;
    Tile goalTile;
    Tile newTile;

    List<Tile> open = new List<Tile>();
    readonly List<Tile> closed = new List<Tile>();
    readonly List<GameObject> markers = new List<GameObject>();

    void Start()
    {
        board = GetComponent<Board>();
        DestroyMarkers();
        CreateMarkersForTiles();
        ClearNodeLists();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            done = false;
            ClearMarkers();
            currentTile = startTile;
            open.Add(currentTile);
        }

        if (!done)
        {
            RecalculateMarkerMaterials();
            if (!pauseAfterStep) FindNewTile();
            else if (Input.GetKeyDown(KeyCode.P)) FindNewTile();
        }
    }
    void FindNewTile()
    {
        CalculateNeighbours();

        closed.Add(currentTile);
        open.Remove(currentTile);
        RecalculateOpenTilesF();
        open = open.OrderBy(tile => tile.F).ToList();
        currentTile = open.ElementAt(0);
        if (currentTile == goalTile) EndSearch();
    }

    void CalculateNeighbours()
    {
        foreach (Vector2Int dir in board.directions)
        {
            if (!board.TryGetTile(currentTile.coordinate + dir, out newTile)) continue;
            if (newTile.IsBlocked) continue;
            if (IsOpen(newTile)) continue;
            if (IsClosed(newTile)) continue;
            open.Add(newTile);
            newTile.parent = currentTile;
        }
    }

    void RecalculateMarkerMaterials()
    {
        foreach (Tile tile in open)
        {
            tile.marker.material = openMaterial;
            tile.marker.enabled = true;
        }

        foreach (Tile tile in closed)
        {
            tile.marker.material = closedMaterial;
            tile.marker.enabled = true;
        }
    }

    void RecalculateOpenTilesF()
    {
        foreach (Tile tile in open)
        {
            tile.CalculateF(goalTile);
        }
    }
    
    public bool IsClosed(Tile tile)
    {
        if (closed.Contains(tile)) return true;
        return false;
    }

    public bool IsOpen(Tile tile)
    {
        if (open.Contains(tile)) return true;
        return false;
    }

    void EndSearch()
    {
        print("done");
        ClearMarkers();

        currentTile.marker.enabled = true;
        currentTile.marker.material = endMaterial;
        currentTile = currentTile.parent;
        while (currentTile.parent != currentTile)
        {
            currentTile.marker.material = openMaterial;
            currentTile.marker.enabled = true;
            currentTile = currentTile.parent;
        }

        currentTile.marker.material = endMaterial;
        currentTile.marker.enabled = true;
        done = true;
    }

    void CreateMarkersForTiles()
    {
        foreach (Tile tile in board.Tiles)
        {
            markers.Add(Instantiate(marker, tile.transform.position, Quaternion.identity));
            tile.marker = markers.Last().GetComponent<MeshRenderer>();
            tile.marker.enabled = false;
        }
    }
    void DestroyMarkers()
    {
        foreach (GameObject mark in markers)
        {
            Destroy(mark);
        }
    }
    
    void ClearMarkers()
    {
        ClearNodeLists();
        foreach (Tile tile in board.Tiles)
        {
            tile.marker.enabled = false;
        }
    }
    
    void ClearNodeLists()
    {
        closed.Clear();
        open.Clear();
    }

    public void SetStartTile(Tile tile)
    {
        startTile = tile;
    }
    public void SetGoalTile(Tile tile)
    {
        goalTile = tile;
    }
}