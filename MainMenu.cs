using Godot;
using System;

public class MainMenu : Control
{
    public override void _Ready()
    {
        
    }

    public void _on_StartGame_pressed()
    {
        GetTree().ChangeScene("res://World.tscn");
    }

    public void _on_QuitGame_pressed()
    {
        GetTree().Quit();
    }
}
