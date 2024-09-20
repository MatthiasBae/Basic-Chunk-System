using Godot;
using System;

public partial class Chunk : Node2D {
    public Vector2I Sector;

    public ProcessState ProcessState;
    public ProgressState ProgressState;
}

public enum ProcessState {
    Prepare,
    Load,
    Render,
    EntitySpawn,
    ActorSpawn,
    ActorDespawn,
    EntityDespawn,
    Unrender,
    Unload,
    
}

public enum ProgressState {
    NotStarted,
    InProgress,
    Completed
}