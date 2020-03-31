using Godot;
using System;

public class ShotDetector : StaticBody
{
    public override void _Ready()
    {
        
    }

    public override void _InputEvent(
        Godot.Object camera, 
        InputEvent @event, 
        Vector3 clickPosition, 
        Vector3 clickNormal, 
        int shapeIdx
    ){
        if(@event.IsActionPressed("Shoot"))
        {
            var parent = (Player)FindParent("Player");
            parent.ShootTowards(clickPosition);
        }
    }
}
