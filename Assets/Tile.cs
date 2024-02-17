using BoardGame;
using UnityEngine;

public class Tile : TileParent {
    
    [SerializeField] AStarPathfinding aStarPathfinding;
    [SerializeField] public MeshRenderer marker;
    [SerializeField] public Tile parent;

    public float G, H, F;


    // 1. TileParent extends MonoBehavior, so you can add member variables here
    // to store data.
    public Material regularMaterial;
    public Material blockedMaterial;


    void Start() {
        G = 1;
    }

    // This function is called when something has changed on the board. All 
    // tiles have been created before it is called.
    public override void OnSetup(Board board) {
        aStarPathfinding = board.GetComponent<AStarPathfinding>();
        // 2. Each tile has a unique 'coordinate'
        Vector2Int key = Coordinate;
        // 3. Tiles can have different modifiers
        if (IsBlocked) {
            
        }

        if (IsObstacle(out int penalty)) {
            
        }

        if (IsCheckPoint) {
            
        }

        if (IsStartPoint) {
            
        }

        if (IsPortal(out Vector2Int destination)) {
            
        }

        // 4. Other tiles can be accessed through the 'board' instance
        if (board.TryGetTile(new Vector2Int(2, 1), out Tile otherTile)) {
            
        }
    }

    // This function is called during the regular 'Update' step, but also gives
    // you access to the 'board' instance.
    public override void OnUpdate(Board board) {
        // 5. Change the material color if this tile is blocked
        if (TryGetComponent<MeshRenderer>(out var meshRenderer)) {
            if (IsBlocked) {
                meshRenderer.material = blockedMaterial;
            }
            else {
                meshRenderer.material = regularMaterial;
            }

            if (IsStartPoint) {
                aStarPathfinding.SetStartTile(this);
                G = 0;
                parent = this;
            }

            if (IsCheckPoint) {
                aStarPathfinding.SetGoalTile(this);
            }
        }
    }

    public void CalculateF(Tile goalTile) {
        H = Mathf.Abs(coordinate.x - goalTile.coordinate.x) + Mathf.Abs(coordinate.y - goalTile.coordinate.y);
        G = parent.G + 1;
        if (IsObstacle(out int pen)) G = parent.G + 1 + pen;
        F = H + G;
    }
}