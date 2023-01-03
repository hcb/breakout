using System;

class Tetronimo
{
    public Vector2 size = new Vector2(16, 16);
    public Vector2 position;
    public Bounds2 bounds;
    public Color color;
    public bool stopped = false;

    public Tetronimo(Vector2 position)
    {
        this.position = position;
        this.color = Color.Pink;
        this.bounds = new Bounds2(position, new Vector2(16, 16));
    }

    public bool isColliding(Bounds2 otherBounds)
    {
        return this.bounds.Overlaps(otherBounds);
    }
}


