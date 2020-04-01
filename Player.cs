using Godot;
using System;

public class Player : KinematicBody
{
    [Export] public float speed;
    [Export] public float weight;
    [Export] public float friction;

    private Vector3 velocity = new Vector3();
    private PackedScene bulletScene;

	public override void _Ready()
	{
        bulletScene = GD.Load<PackedScene>("res://Bullet.tscn");
	}

    public override void _Process(float delta)
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        velocity -= friction * velocity / weight;

        if (Input.IsActionPressed("Foward"))
        { velocity.x += speed / weight; }
        if (Input.IsActionPressed("Backward"))
        { velocity.x -= speed / weight; }
        if (Input.IsActionPressed("Right"))
        { velocity.z += speed / weight; }
        if (Input.IsActionPressed("Left"))
        { velocity.z -= speed / weight; }

        if (velocity.Length() > speed)
        { velocity = velocity.Normalized() * speed; }

        MoveAndCollide(velocity * delta);
    }

    public override void _ExitTree()
    {
        var gameOverMenu = GetTree().Root.GetNode<PopupMenu>("World/Camera/GameOverMenu");
        gameOverMenu.Visible = true;
    }

    public void ShootTowards(Vector3 where)
    {
        var newBullet = (Position3D)bulletScene.Instance();
        var world = (Spatial)GetParent();

        newBullet.LookAtFromPosition(
            this.Translation,
            where,
            Vector3.Up
        );

        var newBulletPhysics = newBullet.GetNode<PhysicsBody>("BulletPhysics");
        newBulletPhysics.SetCollisionLayerBit(0, true);

        world.AddChild(newBullet);
    }
}
