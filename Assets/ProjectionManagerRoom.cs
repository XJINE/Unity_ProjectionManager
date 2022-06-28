using UnityEngine;

public class ProjectionManagerRoom : ProjectionManager
{
    #region Enum

    public enum ProjectionMode
    {
        All,
        Front,
        Left,
        Right,
        Back,
        Bottom
    }

    #endregion Enum

    #region Field

    public ProjectionMode projectionMode;

    public RenderTexture textureFront;
    public RenderTexture textureLeft;
    public RenderTexture textureRight;
    public RenderTexture textureBack;
    public RenderTexture textureBottom;

    public Vector2[] warpFront       = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public Vector2[] warpLeft        = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public Vector2[] warpRight       = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public Vector2[] warpBack        = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public Vector2[] warpBottom      = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public Vector2[] warpBottomLeft  = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public Vector2[] warpBottomRight = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };

    protected static readonly Vector2[] BottomLeftUV = { new (0.5f, 0),
                                                         new (   0, 0),
                                                         new (   0, 1),
                                                         new (0.5f, 1) };

    protected static readonly Vector2[] BottomRightUV = { new (   1, 0),
                                                          new (0.5f, 0),
                                                          new (0.5f, 1),
                                                          new (   1, 1) };

    #endregion Field

    #region Method

    protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        switch (projectionMode)
        {
            case ProjectionMode.All:
            {
                // NOTE:
                // Output image.
                // ―――――――――――――――――――――――――――――――――――――――――
                // | ↑ Front  | ↑ Left   | Bottom | Bottom |
                // ――――――――――――――――――――― | Left   | Right  |
                // | ↑ Back   | ↑ Right  | →      | →      |
                // ―――――――――――――――――――――――――――――――――――――――――

                var quarterWidth = Screen.width  / 4;
                var halfHeight   = Screen.height / 2;

                var viewportRectFront       = new Rect(0,                halfHeight, quarterWidth, halfHeight);
                var viewportRectLeft        = new Rect(quarterWidth,     halfHeight, quarterWidth, halfHeight);
                var viewportRectRight       = new Rect(quarterWidth,     0,          quarterWidth, halfHeight);
                var viewportRectBack        = new Rect(0,                0,          quarterWidth, halfHeight);
                var viewportRectBottomLeft  = new Rect(quarterWidth * 2, 0,          quarterWidth, Screen.height);
                var viewportRectBottomRight = new Rect(quarterWidth * 3, 0,          quarterWidth, Screen.height);

                base.Projection(destination,
                (viewportRectFront,       textureFront,  DefaultUV,     warpFront),
                (viewportRectLeft,        textureLeft,   DefaultUV,     warpLeft),
                (viewportRectRight,       textureRight,  DefaultUV,     warpRight),
                (viewportRectBack,        textureBack,   DefaultUV,     warpBack),
                (viewportRectBottomLeft,  textureBottom, BottomLeftUV,  warpBottomLeft),
                (viewportRectBottomRight, textureBottom, BottomRightUV, warpBottomRight));

                break;
            }

            case ProjectionMode.Front:  base.Projection(textureFront,  destination); break;
            case ProjectionMode.Left:   base.Projection(textureLeft,   destination); break;
            case ProjectionMode.Right:  base.Projection(textureRight,  destination); break;
            case ProjectionMode.Back:   base.Projection(textureBack,   destination); break;
            case ProjectionMode.Bottom: base.Projection(textureBottom, destination); break;
        }
    }

    #endregion Method
}