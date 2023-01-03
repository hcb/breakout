using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(512, 512);

    IGame currentGame = null;

    Font font = Engine.LoadFont("arcade.ttf", 24);

    public void Update()
    {
        if (currentGame == null)
        {
            Engine.DrawString("1) Breakout", new Vector2(20, 20), Color.White, font);
            Engine.DrawString("2) Tetris", new Vector2(20, 40), Color.White, font);

            if (Engine.GetKeyDown(Key.NumRow1))
            {
                currentGame = new BreakoutGame();
            }
            else if (Engine.GetKeyDown(Key.NumRow2))
            {
                currentGame = new TetrisGame();
            }
        }
        else
        {
            currentGame.Update();
        }
    }
}
