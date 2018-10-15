using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class ProjectionManager : MonoBehaviour
{
    #region Field

    public Material drawRectMaterial;

    public static readonly Vector2[] WarpingQuadDefault = new Vector2[]
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),
    };

    protected static readonly Vector2[] UvCoordsDefault = new Vector2[]
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0)
    };

    protected static readonly Vector2[] UvCoordsXFlip = new Vector2[]
    {
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),
        new Vector2(0, 0),
    };

    #endregion Field

    #region Method


    protected abstract void OnRenderImage(RenderTexture source, RenderTexture destination);

    protected abstract void ProjectionAll(RenderTexture destination);

    protected virtual void Projection(RenderTexture source, RenderTexture destination, Vector2[] warpingQuad)
    {
        int width  = destination ? destination.width  : Screen.width;
        int height = destination ? destination.height : Screen.height;

        Rect viewportRect = new Rect(0, 0, width, height);

        Graphics.SetRenderTarget(destination);

        GL.Clear(true, true, Color.clear);

        GL.PushMatrix();

        GL.LoadOrtho();

        DrawRect(viewportRect, source, ProjectionManager.UvCoordsDefault, warpingQuad);

        GL.PopMatrix();
    }

    protected virtual void DrawRect(Rect viewPortRect, RenderTexture texture, Vector2[] uvCoords, Vector2[] warpingQuad)
    {
        // NOTE:
        // Texture will be rendered with clockwise definition even it use GL~ function.
        // (Usually, DirectX use clockwise and GL use anti-clockwise.)
        // 
        // ――――――――
        // |(0,1)    (1,1)|
        // |              |
        // |(0,0)    (1,0)|
        // ――――――――

        this.drawRectMaterial.mainTexture = texture;
        this.drawRectMaterial.SetPass(0);

        GL.Viewport(viewPortRect);

        GL.Begin(GL.QUADS);

        GL.Color(new Color(0, 0, 0, 0));

        GL.TexCoord(uvCoords[0]);
        GL.Vertex(warpingQuad[0]);

        GL.TexCoord(uvCoords[1]);
        GL.Vertex(warpingQuad[1]);

        GL.TexCoord(uvCoords[2]);
        GL.Vertex(warpingQuad[2]);

        GL.TexCoord(uvCoords[3]);
        GL.Vertex(warpingQuad[3]);

        GL.End();
    }

    #endregion Method
}