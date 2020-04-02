using Godot;
using System;

public class EnemySpawner : Position3D
{
    [Export] public float spawnInterval;
    [Export] public float spawnIntervalChangeRate;
    [Export] public float spawnRadius;

    private float timeSinceLastSpawn;

    private Random rng;

    private PackedScene enemyScene;
    private Spatial world;

    public override void _Ready()
    {
        timeSinceLastSpawn = 0;
        rng = new Random();
        enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
        world = GetParent<Spatial>();
    }

    public override void _Process(float delta)
    {
        timeSinceLastSpawn += delta;

        // roughly exponential decrease
        spawnInterval += spawnIntervalChangeRate * spawnInterval * delta;

        if(timeSinceLastSpawn > spawnInterval)
        {
            timeSinceLastSpawn -= spawnInterval;
            Spawn();
        }
    }

    private void Spawn()
    {
        var spawnLocation = Vector3.Forward * (float)(rng.NextDouble() * spawnRadius);
        spawnLocation = spawnLocation.Rotated(Vector3.Up, (float)(rng.NextDouble() * 360));
        spawnLocation += this.Transform.origin;

        var newEnemy = (KinematicBody)enemyScene.Instance();
        newEnemy.Translation = spawnLocation;
        world.AddChild(newEnemy);
    }
}
