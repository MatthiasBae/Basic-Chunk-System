using Godot;
using System;
using System.Collections.Generic;

public partial class ChunkStore : Node2D {
    public static ChunkStore Instance;
    
    public Dictionary<Vector2I, Chunk> Chunks = new();
    
    public override void _EnterTree() {
        Instance = this;
    }
    
    public bool TryAddChunk(Vector2I sector, Chunk chunk) {
        var addedToList = this.Chunks.TryAdd(sector, chunk);
        if(addedToList) {
            this.AddChild(chunk);
        }
        
        return addedToList;
    }
    
    public bool TryGetChunk(Vector2I sector, out Chunk chunk) {
        return this.Chunks.TryGetValue(sector, out chunk);
    }
    
    public IEnumerable<Chunk> GetChunksEnumerable() {
        foreach(var chunk in this.Chunks.Values) {
            yield return chunk;
        }
    }

    public void RemoveChunk(Chunk chunk) {
        this.Chunks.Remove(chunk.Sector);
        this.RemoveChild(chunk);
    }
    
    public void RemoveChunk(Vector2I sector) {
        this.Chunks.Remove(sector);
    }
    
    public bool HasChunk(Vector2I sector) {
        return this.Chunks.ContainsKey(sector);
    }
}
