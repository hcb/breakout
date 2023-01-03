using System;
using System.Collections.Generic;
using System.Linq;

class TetrisGame : IGame
{
    private static readonly Index2 GridSize = new Index2(9, 20);
    private static readonly float BlockSize = 24;
    private static readonly float FastDropDelay = 0.1f;
    private static readonly float SlowDropDelay = 1.0f;
    private static readonly Random Random = new Random();
    private static readonly Color[] Colors = new Color[] { Color.Red, Color.Cyan, Color.Yellow, Color.Green };

    public static readonly string Title = "Simple Tetris";
    public static readonly Vector2 Resolution = GridSize.ToVector2() * BlockSize;

    private Block FallingBlock;
    private List<Block> FallenBlocks = new List<Block>();
    private float DropTimer = 0;
    private float MoveTimer = 0;

    public TetrisGame()
    {
        CreateNewBlock();
    }

    private void CreateNewBlock()
    {
        FallingBlock = new Block();
        FallingBlock.Position = new Index2(GridSize.X / 2, 0);
        FallingBlock.Color = Colors[Random.Next(Colors.Length)];
    }

    public void Update()
    {
        if (FallingBlock.Position.Y == GridSize.Y - 1 || FallenBlocks.Any(block => block.Position == FallingBlock.Position + new Index2(0, 1)))
        {
            FallenBlocks.Add(FallingBlock);
            CreateNewBlock();
        }

        DropTimer += Engine.TimeDelta;
        if (DropTimer >= (Engine.GetKeyHeld(Key.Down) ? FastDropDelay : SlowDropDelay))
        {
            DropTimer = 0;
            FallingBlock.Position.Y += 1;
        }

        MoveTimer -= Engine.TimeDelta;
        if (MoveTimer <= 0)
        {
            if (Engine.GetKeyHeld(Key.Left) && FallingBlock.Position.X > 0 && !FallenBlocks.Any(block => block.Position == FallingBlock.Position + new Index2(-1, 0)))
            {
                MoveTimer = FastDropDelay;
                FallingBlock.Position.X -= 1;
            }
            else if (Engine.GetKeyHeld(Key.Right) && FallingBlock.Position.X < GridSize.X - 1 && !FallenBlocks.Any(block => block.Position == FallingBlock.Position + new Index2(1, 0)))
            {
                MoveTimer = FastDropDelay;
                FallingBlock.Position.X += 1;
            }
        }

        DrawBlock(FallingBlock);
        FallenBlocks.ForEach(block => DrawBlock(block));
    }

    private void DrawBlock(Block block)
    {
        Bounds2 bounds = new Bounds2(block.Position.ToVector2() * BlockSize, new Vector2(BlockSize, BlockSize));
        Engine.DrawRectSolid(bounds, block.Color);
    }

    private class Block
    {
        public Index2 Position;
        public Color Color;
    }
}

