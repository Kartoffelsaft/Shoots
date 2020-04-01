using Godot;
using System;

public class BulletPhysics : KinematicBody
{
    [Export] public float speed;
    [Export] public float timeout;
    [Export] public float explosionSize;
    [Export] public float explosionTimeout;

    private float age;
    private float explosionTime;
    private bool exploded;

    private Position3D bulletParent;
    private MeshInstance explosion;

    public override void _Ready()
    {
        age = 0;
        exploded = false;
        bulletParent = (Position3D)GetParent();
        explosion = bulletParent.GetNode<MeshInstance>("Explosion");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!exploded)
        {
            Vector3 velocity = Vector3.Forward * speed;

            var collision = MoveAndCollide(velocity * delta, testOnly: true);

            if (collision != null)
            {
                Explode();
                if (collision.Collider is CollisionObject
                    && !(collision.Collider is BulletPhysics)
                ){ ((CollisionObject)(collision.Collider)).QueueFree(); }

                if(collision.Collider is Enemy)
                {
                    var scoreCount = GetTree().Root.GetNode<ScoreCount>("World/Camera/ScoreCount");
                    scoreCount.IncrementScore();
                }
            }

            bulletParent.Translate(velocity * delta);
        }
    }

    public override void _Process(float delta)
    {
        age += delta;

        if(age > timeout)
        { Explode(); }

        if(exploded)
        {
            var explosionAge = age - explosionTime;

            if (explosionAge > explosionTimeout)
            { bulletParent.QueueFree(); }

            explosion.Scale = Vector3.One * (explosionAge/explosionTimeout) * explosionSize;
        }
    }

    private void Explode()
    {
        if(!exploded)
        {
            explosionTime = age;
            exploded = true;
            explosion.Visible = true;

            var sound = bulletParent.GetNode<AudioStreamPlayer3D>("ExplosionNoise");
            sound.Play();
        }
    }
}
