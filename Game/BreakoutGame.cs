using System;
using System.Collections.Generic;

class BreakoutGame : IGame
{
    public static readonly string Title = "Breakout";
    public static readonly Vector2 Resolution = new Vector2(512, 512);

    // Define some constants controlling animation speed:
    static readonly float Framerate = 10;
    static readonly float PaddleSpeed = 350;
    static readonly float BallSpeed = 120;

    // Load some textures when the game starts:
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
            {
                hit = true;
                return true;
            }
            return false;
        }
    }

    public BreakoutGame()
    {

        class Game
    {
        // bricks
        List<Brick> bricks;
        float BrickWidth = 40;
        float BrickHeight = 15;
        int BrickSpacer = 10;

        // This is your constructor method, which will be called to initialize your game
        // You can set up your bricks and playfield here initially 
        public Game()
        {
            // initialize a new list for your bricks
            bricks = new List<Brick>();

            // Let's do 5 rows of bricks, 10 each row
            int rows = 5;
            int cols = 10;

            // Calculate starting X value
            // 10 bricks, 9 spaces between bricks
            float startX = (Resolution.X - (cols * BrickWidth + (cols - 1) * BrickSpacer)) / 2;
            float startY = BrickHeight;

            for (int row = 0; row < rows; row++)
            {
                // Offset from the top of the window to start the row
                float offsetY = row * (BrickHeight + BrickSpacer) + startY;
                for (int col = 0; col < cols; col++)
                {
                    // Offset from left for each brick
                    float offsetX = col * (BrickWidth + BrickSpacer) + startX;

                    // Create the position vector using the offsetX, offsetY
                    Vector2 pos = new Vector2(offsetX, offsetY);

                    // Create the brick
                    Brick brick = new Brick(pos);

                    // Add to the Game's bricks list
                    bricks.Add(brick);

                }

            }

            //generateBricks();
        }

        public void Update()
        {
            // Draw the background:
            Engine.DrawRectSolid(new Bounds2(0, 0, Resolution.X, Resolution.Y), Color.Tan);
            Engine.DrawTexture(texBackground, Vector2.Zero);

            // Draw the title
            Engine.DrawString("to play breakout, press SPACE", new Vector2(Resolution.X / 2, Resolution.Y / 4),
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
            int rows = 5;
            int cols = 10;

            // Calculate starting X value
            // 10 bricks, 9 spaces between bricks
            float startX = (Resolution.X - (cols * BrickWidth + (cols - 1) * BrickSpacer)) / 2;
            float startY = BrickHeight;

            for (int row = 0; row < rows; row++)
            {
                // Offset from the top of the window to start the row
                float offsetY = row * (BrickHeight + BrickSpacer) + startY;
                for (int col = 0; col < cols; col++)
                {
                    // Offset from left for each brick
                    float offsetX = col * (BrickWidth + BrickSpacer) + startX;

                    // Create the position vector using the offsetX, offsetY
                    Vector2 pos = new Vector2(offsetX, offsetY);

                    // Create the brick
                    Brick brick = new Brick(pos);

                    // Add to the Game's bricks list
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
