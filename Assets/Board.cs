using BoardGame;
using UnityEngine;
using System.Collections.Generic;

public class Board : BoardParent {
    
    [SerializeField] GameObject marker;
    public List<Vector2Int> directions = new List<Vector2Int> {
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0)
    };

    // This function is called whenever the board or any tile inside the board
    // is modified.
    public override void SetupBoard() {
        
        // 1. Get the size of the board
        var boardSize = BoardSize;
        
        // 2. Iterate over all tiles
        foreach (Tile tile in Tiles) {

        }

        // 3. Find a tile with a particular coordinate
        Vector2Int coordinate = new Vector2Int(2, 1);
        if (TryGetTile(coordinate, out Tile tile2)) {
            
        }
    }
}
