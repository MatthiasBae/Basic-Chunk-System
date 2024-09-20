using Godot;
using System;
using System.Collections.Generic;

public partial class TileMapLayer2D : Node2D {

    [Export(PropertyHint.File, "*.tscn")]
    public PackedScene TileMapLayerChunkPrefab;
    
    private Dictionary<Vector2I, TileMapLayer> TileMapLayerChunks = new();
    
    
}
