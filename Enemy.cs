using Godot;
using System;

public class Enemy : KinematicBody
{
    [Export] public float directionChangeInterval;
    [Export] public float speed;
    [Export] public float shootInterval;
    [Export] public float inaccuracyMax;

    private float timeSinceLastShot;
    private float timeSinceLastDirectionChange;
    private Vector3 movementDirection;

    private Random rng;

    private Spatial world;
    private Player targetPlayer;
    private PackedScene bulletScene;

    public override void _Ready()
    {
        timeSinceLastShot = 0;
        //set it to this instead of 0 so that it immediately changes direction
        timeSinceLastDirectionChange = directionChangeInterval; 
        movementDirection = Vector3.Forward;
        rng = new Random();
        world = (Spatial)GetTree().CurrentScene;
        if(world.HasNode("Player"))
        { targetPlayer = GetTree().CurrentScene.GetNode<Player>("Player"); }
        bulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
    }

    public override void _Process(float delta)
    {
        timeSinceLastShot += delta;

        if(timeSinceLastShot > shootInterval && 
           world.HasNode("Player"))
        {
            timeSinceLastShot -= shootInterval;
            ShootVaguelyTowards(targetPlayer.Translation);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        timeSinceLastDirectionChange += delta;

        if(timeSinceLastDirectionChange > directionChangeInterval)
        {
            timeSinceLastDirectionChange -= directionChangeInterval;
            movementDirection = movementDirection.Rotated(
                Vector3.Up, 
                (float)(rng.NextDouble() * 360)
            );
        }

        MoveAndCollide(movementDirection * speed * delta);
    }

    private void ShootVaguelyTowards(Vector3 where)
    {
        var target = where;
        target.x += (float)(inaccuracyMax * rng.NextDouble());
        target.x -= (float)(inaccuracyMax * rng.NextDouble());
        target.z += (float)(inaccuracyMax * rng.NextDouble());
        target.z -= (float)(inaccuracyMax * rng.NextDouble());

        Position3D bullet = (Position3D)bulletScene.Instance();

        bullet.LookAtFromPosition(
            this.Translation,
            target,
            Vector3.Up
        );

        var newBulletPhysics = bullet.GetNode<PhysicsBody>("BulletPhysics");
        newBulletPhysics.SetCollisionLayerBit(2, true);

        world.AddChild(bullet);
    }
}
