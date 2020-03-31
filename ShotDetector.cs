using Godot;
using System;

public class ShotDetector : StaticBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
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
