using Godot;
using System;

public class ScoreCount : Label
{
    private ulong score;

    public override void _Ready()
    {
        score = 0;
    }

    public void IncrementScore()
    {
        score++;

        this.Text = $"Score: {score}";
    }
}
