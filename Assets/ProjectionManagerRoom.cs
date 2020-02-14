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

    public Vector2[] warpingQuadFront       = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadLeft        = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadRight       = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadBack        = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadBottom      = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadBottomLeft  = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadBottomRight = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };

    protected static readonly Vector2[] UvCoordsBottomLeft = new[]
    {
        new Vector2(0.5f, 0),
        new Vector2(   0, 0),
        new Vector2(   0, 1),
        new Vector2(0.5f, 1)
    };

    protected static readonly Vector2[] UvCoordsBottomRight = new[]
    {
        new Vector2(   1, 0),
        new Vector2(0.5f, 0),
        new Vector2(0.5f, 1),
        new Vector2(   1, 1)
    };

    #endregion Field

    #region Method

    protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        switch (this.projectionMode)
        {
            case ProjectionMode.All:
            {
                // NOTE:
                // Output image.
                // ―――――――――――――――――――――
                // | ↑ Front  | ↑ Left  | Bottom | Bottom |
                // ――――――――――― | Left   | Right  |
                // | ↑ Back   | ↑ Right | →     | →     |
                // ―――――――――――――――――――――

                int quarterWidth = Screen.width  / 4;
                int halfHeight   = Screen.height / 2;

                Rect viewportRectFront       = new Rect(0,                halfHeight, quarterWidth, halfHeight);
                Rect viewportRectLeft        = new Rect(quarterWidth,     halfHeight, quarterWidth, halfHeight);
                Rect viewportRectRight       = new Rect(quarterWidth,     0,          quarterWidth, halfHeight);
                Rect viewportRectBack        = new Rect(0,                0,          quarterWidth, halfHeight);
                Rect viewportRectBotomLeft   = new Rect(quarterWidth * 2, 0,          quarterWidth, Screen.height);
                Rect viewportRectBottomRight = new Rect(quarterWidth * 3, 0,          quarterWidth, Screen.height);

                base.Projection(destination,
                (viewportRectFront,       this.textureFront,  ProjectionManager.UvCoordsDefault,         this.warpingQuadFront),
                (viewportRectLeft,        this.textureLeft,   ProjectionManager.UvCoordsDefault,         this.warpingQuadLeft),
                (viewportRectRight,       this.textureRight,  ProjectionManager.UvCoordsDefault,         this.warpingQuadRight),
                (viewportRectBack,        this.textureBack,   ProjectionManager.UvCoordsDefault,         this.warpingQuadBack),
                (viewportRectBotomLeft,   this.textureBottom, ProjectionManagerRoom.UvCoordsBottomLeft,  this.warpingQuadBottomLeft),
                (viewportRectBottomRight, this.textureBottom, ProjectionManagerRoom.UvCoordsBottomRight, this.warpingQuadBottomRight));

                break;
            }

            case ProjectionMode.Front:  base.Projection(this.textureFront,  destination); break;
            case ProjectionMode.Left:   base.Projection(this.textureLeft,   destination); break;
            case ProjectionMode.Right:  base.Projection(this.textureRight,  destination); break;
            case ProjectionMode.Back:   base.Projection(this.textureBack,   destination); break;
            case ProjectionMode.Bottom: base.Projection(this.textureBottom, destination); break;
        }
    }

    #endregion Method
}