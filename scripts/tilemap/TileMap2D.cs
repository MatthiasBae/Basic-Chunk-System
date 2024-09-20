using Godot;
using System;
using System.Collections.Generic;

public partial class TileMap2D : Node2D {
    private Dictionary<string, TileMapLayer2D> _Layers = new();
}
