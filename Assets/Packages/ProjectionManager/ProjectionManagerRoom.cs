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

    // NOTE:
    // RenderTexture generated as asset are also should be able to set from inspector.
    // And when developing, RenderTextures in inspector are useful to debug.

    public RenderTexture textureFront;
    public RenderTexture textureLeft;
    public RenderTexture textureRight;
    public RenderTexture textureBack;
    public RenderTexture textureBottom;

    public Vector2[] warpingQuadFront  = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadLeft   = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadRight  = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadBack   = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
    public Vector2[] warpingQuadBottom = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
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
                    ProjectionAll(destination);
                    break;
                }
            case ProjectionMode.Front:
                {
                    base.Projection(this.textureFront, destination, this.warpingQuadFront);
                    break;
                }
            case ProjectionMode.Left:
                {
                    base.Projection(this.textureLeft, destination, this.warpingQuadLeft);
                    break;
                }
            case ProjectionMode.Right:
                {
                    base.Projection(this.textureRight, destination, this.warpingQuadRight);
                    break;
                }
            case ProjectionMode.Back:
                {
                    base.Projection(this.textureBack, destination, this.warpingQuadBack);
                    break;
                }
            case ProjectionMode.Bottom:
                {
                    base.Projection(this.textureBottom, destination, this.warpingQuadBottom);
                    break;
                }
        }
    }

    protected override void ProjectionAll(RenderTexture destination)
    {
        // NOTE:
        // Output Image
        // ―――――――――――――――――――――
        // | ↑ Front  | ↑ Left  | Bottom | Bottom |
        // ――――――――――― | Left   | Right  |
        // | ↑ Back   | ↑ Right | →     | →     |
        // ―――――――――――――――――――――

        int screenWidth  = Screen.width;
        int screenHeight = Screen.height;

        int quarterWidth = screenWidth  / 4;
        int halfHeight   = screenHeight / 2;

        Rect viewportRectFront = new Rect(0,            halfHeight, quarterWidth, halfHeight);
        Rect viewportRectLeft  = new Rect(quarterWidth, halfHeight, quarterWidth, halfHeight);
        Rect viewportRectRight = new Rect(quarterWidth, 0,          quarterWidth, halfHeight);
        Rect viewportRectBack  = new Rect(0,            0,          quarterWidth, halfHeight);
        Rect viewportRectBotomLeft   = new Rect(quarterWidth * 2, 0, quarterWidth, screenHeight);
        Rect viewportRectBottomRight = new Rect(quarterWidth * 3, 0, quarterWidth, screenHeight);

        Graphics.SetRenderTarget(destination);

        GL.Clear(true, true, Color.clear);

        GL.PushMatrix();

        GL.LoadOrtho();

        base.DrawRect(viewportRectFront,       this.textureFront,  ProjectionManager.UvCoordsDefault,         this.warpingQuadFront);
        base.DrawRect(viewportRectLeft,        this.textureLeft,   ProjectionManager.UvCoordsDefault,         this.warpingQuadLeft);
        base.DrawRect(viewportRectRight,       this.textureRight,  ProjectionManager.UvCoordsDefault,         this.warpingQuadRight);
        base.DrawRect(viewportRectBack,        this.textureBack,   ProjectionManager.UvCoordsDefault,         this.warpingQuadBack);
        base.DrawRect(viewportRectBotomLeft,   this.textureBottom, ProjectionManagerRoom.UvCoordsBottomLeft,  this.warpingQuadBottomLeft);
        base.DrawRect(viewportRectBottomRight, this.textureBottom, ProjectionManagerRoom.UvCoordsBottomRight, this.warpingQuadBottomRight);

        GL.PopMatrix();
    }

    #endregion Method
}