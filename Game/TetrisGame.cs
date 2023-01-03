using System;
using System.Collections.Generic;

class TetrisGame : IGame
{

    public static readonly float blockSize = 16;
    public static readonly Vector2 Resolution = new Vector2(512, 512);

    Vector2 bottomStart = new Vector2(0, 512 - 32);
    Vector2 bottomEnd = new Vector2(512, 512 - 32);
    Bounds2 bottomBounds;

    Vector2 leftStart = new Vector2(32, 0);
    Vector2 leftEnd = new Vector2(32, 512 - 32);

    Vector2 rightStart = new Vector2(512 - 32, 0);
    Vector2 rightEnd = new Vector2(512 - 32, 512 - 32);

    // Tetronimo
    Vector2 initialPosition = new Vector2((512) / 2, 16);
    Tetronimo t;
    List<Tetronimo> tetronimos;

    float velocity = 1;

    // Playfield size is 10 x 40, with 20 rows hidden from the player
    // https://tetris.fandom.com/wiki/Playfield
    // https://hackaday.com/2014/01/28/a-deep-dive-into-nes-tetris/
    // need to create a grid

    int[,] grid = new int[16, 10];

    float count = 0;

    // Load the font
    Font font = Engine.LoadFont("arcade.ttf", 24);
    public TetrisGame()
    {
        bottomBounds = new Bounds2(bottomStart, bottomEnd);
        t = new Tetronimo(initialPosition);
        tetronimos = new List<Tetronimo>();

    }


    public void Update()
    {
        count += 1;

        if (count % 100 != 0)
        {
            return;
        }
        Engine.DrawString(count.ToString(), new Vector2(20, 20), Color.White, font);
        Engine.DrawLine(bottomStart, bottomEnd, Color.BlueViolet);
        Engine.DrawLine(leftStart, leftEnd, Color.BlueViolet);
        Engine.DrawLine(rightStart, rightEnd, Color.BlueViolet);

        foreach (var b in tetronimos)
        {
            Bounds2 bounds = new Bounds2(b.position * 16, b.size);
            Engine.DrawRectSolid(bounds, Color.OldLace);
        }

        if (!t.stopped)
        {
            // Draw the Tetronimo
            Engine.DrawRectSolid(t.bounds, t.color);
            Console.WriteLine(t.bounds.ToString());

            // Use the keyboard to control the block:
            Vector2 moveOffset = Vector2.Zero;
            if (Engine.GetKeyHeld(Key.Left))
            {
                moveOffset.X -= 1;
            }
            if (Engine.GetKeyHeld(Key.Right))
            {
                moveOffset.X += 1;
            }
            if (Engine.GetKeyHeld(Key.Down))
            {
                moveOffset.Y += 1;
            }
            if (Engine.GetKeyHeld(Key.Up))
            {
                moveOffset.Y -= 1;
            }
            //moveOffset.Y += 16 * velocity;

            //if (Engine.GetKeyHeld(Key.Down))
            //{
            //    velocity = 5;
            //}
            //else
            //{
            //    velocity = 1;
            //}


            t.position += moveOffset;
            t.bounds = new Bounds2(t.position, t.size);

            bool colliding = false;
            foreach (var b in tetronimos)
            {
                if (t.isColliding(b.bounds))
                {
                    colliding = true;
                    break;
                }
            }

            //if (t.position.Y >= bottomBounds.Position.Y - t.size.Y || colliding)
            //{
            //    // stop the current tetronimo
            //    t.stopped = true;
            //    tetronimos.Add(t);

            //    // start again with a new block
            //    t = new Tetronimo(initialPosition);
            //    // reset velocity
            //    velocity = 1;
            //}
        }

    }
}

