using Godot;
using System;

public class GameOverMenu : PopupMenu
{
    private const int NEW_GAME_ID = 0;
    private const int CLOSE_GAME_ID = 1;

    public override void _Ready()
    {
        AddItem("New Game", NEW_GAME_ID);
        AddItem("Quit Game", CLOSE_GAME_ID);
    }

    public void _on_GameOverMenu_id_pressed(int id)
    {
        switch(id)
        {
        case NEW_GAME_ID:
            GetTree().ReloadCurrentScene(); 
            break;
        case CLOSE_GAME_ID:
            GetTree().Quit();
            break;
        }
    }
}
