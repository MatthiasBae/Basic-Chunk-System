using Godot;
using System;
using System.Collections.Generic;

public partial class ChunkController : Node2D {
    
    public const int CHUNK_SIZE = 16;
    public const int FIELD_SIZE = 16;
    
    [Export]
    public PackedScene ChunkPrefab;
    
    [Export]
    public int AreaRadius;
    
    [Export]
    public ChunkStore ChunkStore;

    public Vector2I PreviousAreaCenter;
    public Vector2I PreviousAreaStart;
    public Vector2I PreviousAreaEnd;
    
    public Vector2I AreaCenter;
    public Vector2I AreaStart;
    public Vector2I AreaEnd;
    
    private void _UpdateArea(Vector2I enteredSector) {
        this.PreviousAreaCenter = this.AreaCenter;
        this.PreviousAreaStart = this.AreaStart;
        this.PreviousAreaEnd = this.AreaEnd;
        
        this.AreaCenter = enteredSector;
        this.AreaStart = enteredSector - new Vector2I(this.AreaRadius, this.AreaRadius);
        this.AreaEnd = enteredSector + new Vector2I(this.AreaRadius, this.AreaRadius);
    }
    private void _CreateChunks() {
        for(var x = this.AreaStart.X; x <= this.AreaEnd.X; x++) {
            for(var y = this.AreaStart.Y; y <= this.AreaEnd.Y; y++) {
                var sector = new Vector2I(x, y);
                if (this.ChunkStore.HasChunk(sector)) {
                    continue;
                }
                
                this._CreateChunk(sector);
            }
        }
    }
    
    //@TODO: Object pooling implementation
    private void _RemoveChunks() {
        foreach(var chunk in this.ChunkStore.GetChunksEnumerable()) {
            if (this.IsChunkInArea(chunk.Sector)) {
                continue;
            }
            
            this.ChunkStore.RemoveChunk(chunk);
            chunk.QueueFree();
        }
    }
    
    private void _CreateChunk(Vector2I sector) {
        var chunk = this.ChunkPrefab.Instantiate<Chunk>();
        chunk.Name = $"Chunk_X{sector.X}_Y{sector.Y}";
        chunk.Sector = sector;
        var added = this.ChunkStore.TryAddChunk(sector, chunk);
        
        if (!added) {
            return;
        }
        
        chunk.GlobalPosition = CoordinatesHelper.SectorToSectorCenterPosition(sector);
    }
    
    public bool IsChunkInArea(Vector2I sector) {
        return sector.X >= this.AreaStart.X && sector.X <= this.AreaEnd.X && sector.Y >= this.AreaStart.Y && sector.Y <= this.AreaEnd.Y;
    }
    
    private void _OnPlayerTrackerSectorEntered(Vector2I enteredSector) {
        this._UpdateArea(enteredSector);
        
        this._CreateChunks();
    }
    
    private void _OnPlayerTrackerSectorExited(Vector2I exitedSector) {
        GD.Print($"Exited sector: {exitedSector}");
        this._RemoveChunks();
    }
}