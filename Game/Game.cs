using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(512, 512);

    // Define some constants controlling animation speed:
    static readonly float Framerate = 10;
    static readonly float PaddleSpeed = 350;
    static readonly float BallSpeed = 120;

    // Load some textures when the game starts:
    Texture texKnight = Engine.LoadTexture("knight.png");
    Texture texBackground = Engine.LoadTexture("hillsbg.png");
    Texture texBall = Engine.LoadTexture("ball.png");

    // Load the font
    Font font = Engine.LoadFont("arcade.ttf", 24);

    // #BA8475
    Color colorPaddle = new Color(0xba, 0x84, 0x75);

    // Keep track of the paddle and ball position
    Vector2 paddlePosition = new Vector2(Resolution.X / 2, Resolution.Y - 60);
    Vector2 ballPosition = Resolution / 2;
    Vector2 direction = new Vector2(1, 1);

    // Ball
    bool ballLost = false;

    // bricks
    List<Brick> bricks;
    float BrickWidth = 40;
    float BrickHeight = 15;
    int BrickSpacer = 10;
    int hitCount = 0;
    class Brick
    {
        //float Width = 25;
        //float Height = 15;
        public Vector2 position;
        public bool hit;

        public Brick(Vector2 position)
        {
            this.position = position;
        }

        public bool isColliding(Vector2 ballPosition, Texture texture)
        {

            if (position.X < ballPosition.X + texture.Width &&
                position.X + 40 > ballPosition.X &&
                position.Y < ballPosition.Y + texture.Height &&
                position.Y + 15 > ballPosition.Y
                )
            //&& position.Y <= ballPosition.Y + texture.Height)
            {
                hit = true;
                return true;
            }
            return false;
        }
    }

    public Game()
    {
        generateBricks();
    }

    public void Update()
    {
        // Draw the background:
        Engine.DrawRectSolid(new Bounds2(0, 0, Resolution.X, Resolution.Y), Color.Tan);
        Engine.DrawTexture(texBackground, Vector2.Zero);

        // Draw the title
        Engine.DrawString("eclater", new Vector2(Resolution.X / 2, Resolution.Y / 4),
            Color.DarkSalmon, font, TextAlignment.Center);

        if (!ballLost)
        {
            // draw the score
            Engine.DrawString(hitCount.ToString(), new Vector2(25, Resolution.Y - 25),
                Color.White, font, TextAlignment.Right);
            // Draw the bricks
            foreach (var brick in bricks)
            {
                if (!brick.hit)
                {
                    Bounds2 bounds = new Bounds2(brick.position, new Vector2(BrickWidth, BrickHeight));
                    Engine.DrawRectSolid(bounds, Color.PaleVioletRed);
                }
            }

            // Use the keyboard to control the paddle:
            Vector2 moveOffset = Vector2.Zero;
            if (Engine.GetKeyHeld(Key.Left))
            {
                moveOffset.X -= 1;
            }
            if (Engine.GetKeyHeld(Key.Right))
            {
                moveOffset.X += 1;
            }
            paddlePosition += moveOffset * PaddleSpeed * Engine.TimeDelta;

            // Draw the paddle:
            Engine.DrawRectSolid(new Bounds2(paddlePosition.X, paddlePosition.Y, 75, 15), colorPaddle);

            // Handle ball direction
            if (ballPosition.Y > Resolution.Y)
            {
                ballLost = true;
            }
            else if (ballPosition.Y + 1 + texBall.Height >= paddlePosition.Y
                && ballPosition.Y + 1 <= paddlePosition.Y + 15
                && ballPosition.X + texBall.Width >= paddlePosition.X
                && ballPosition.X <= paddlePosition.X + 75)
            {
                direction.Y = -1;
            }
            else if (ballPosition.Y <= 0)
            {
                direction.Y = 1;
            }

            if (ballPosition.X + 1 + texBall.Width >= Resolution.X)
            {
                direction.X = -1;
            }
            else if (ballPosition.X + 1 <= 0)
            {
                direction.X = 1;
            }

            foreach (Brick brick in bricks)
            {
                if (!brick.hit && brick.isColliding(ballPosition, texBall))
                {
                    hitCount++;
                    direction.Y *= -1;
                };
            }
            ballPosition = updatePosition(direction, ballPosition, BallSpeed);

            Engine.DrawTexture(texBall, ballPosition);
        }
        else
        {
            Engine.DrawString("You lost the ball : (", Resolution / 2,
                Color.DarkSalmon, font, TextAlignment.Center);
            Engine.DrawString("Press enter to play again!",
                new Vector2(Resolution.X / 2, Resolution.Y / 2 + 16),
                Color.DarkSalmon, font, TextAlignment.Center);

            if (Engine.GetKeyHeld(Key.Return))
            {
                ballLost = false;
                ballPosition = Resolution / 2;
            }

        }
    }

    public void generateBricks()
    {
        bricks = new List<Brick>();
        // Let's do 5 rows of bricks, 10 each row
        float startX = ((Resolution.X - (10 * BrickWidth + 9 * BrickSpacer)) / 2);
        for (int i = 0; i < 5; i++)
        {
            float offsetY = i * BrickSpacer + startX;
            for (int j = 0; j < 10; j++)
            {
                float offsetX = j * BrickSpacer + startX;
                Vector2 pos = new Vector2(j * BrickWidth + offsetX, i * BrickHeight + offsetY);
                Brick brick = new Brick(pos);
                bricks.Add(brick);
            }
        }
    }

    public Vector2 updatePosition(Vector2 direction, Vector2 position, float speed)
    {
        position.X += direction.X * speed * Engine.TimeDelta;
        position.Y += direction.Y * speed * Engine.TimeDelta;
        return position;
    }

}
