using UnityEngine;

[RequireComponent(typeof(Camera))]
public abstract class ProjectionManager : MonoBehaviour
{
    #region Field

    public Material material;

    public static readonly Vector2[] DefaultWarp  = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public static readonly Vector2[] DefaultUV    = { new (0, 0), new (0, 1), new (1, 1), new (1, 0) };
    public static readonly Vector2[] XFlipUV      = { new (1, 0), new (1, 1), new (0, 1), new (0, 0) };
    public static readonly Vector2[] RotateLeftUV = { new (0, 1), new (1, 1), new (1, 0), new (0, 0) };

    #endregion Field

    #region Method

    protected abstract void OnRenderImage(RenderTexture source, RenderTexture destination);

    protected void HorizontalProjection(RenderTexture destination, params RenderTexture[] sources)
    {
        var drawRects = new (Rect, RenderTexture, Vector2[], Vector2[])[sources.Length];
        var offset    = 0;

        for(var i = 0; i < sources.Length; i++)
        {
            var width  = sources[i].width;
            var height = sources[i].height;

            drawRects[i] = (new Rect(offset, 0, width, height), sources[i], DefaultUV, DefaultWarp);
            offset += width;
        }

        Projection(destination, drawRects);
    }

    protected virtual void Projection(RenderTexture source, RenderTexture destination)
    {
        var width    = destination ? destination.width  : Screen.width;
        var height   = destination ? destination.height : Screen.height;
        var viewport = new Rect(0, 0, width, height);

        Projection(destination, (viewport, source, DefaultUV, DefaultWarp));
    }

    protected virtual void Projection
    (RenderTexture destination, params (Rect viewport, RenderTexture tex, Vector2[] uv, Vector2[] warp)[] drawRects)
    {
        Graphics.SetRenderTarget(destination);

        GL.Clear(true, true, Color.clear);

        GL.PushMatrix();

        GL.LoadOrtho();

        foreach (var drawRect in drawRects)
        {
            DrawRect(drawRect.viewport, drawRect.tex, drawRect.uv, drawRect.warp);
        }

        GL.PopMatrix();
    }

    protected void DrawRect(Rect viewport, RenderTexture texture, Vector2[] uv, Vector2[] warp)
    {
        // NOTE:
        // Texture will be rendered with clockwise definition even it use GL~ function.
        // (Usually, DirectX use clockwise and GL use anti-clockwise.)
        // 
        // ――――――――――――――――
        // |(0,1)    (1,1)|
        // |              |
        // |(0,0)    (1,0)|
        // ――――――――――――――――

        material.mainTexture = texture;
        material.SetPass(0);

        GL.Viewport(viewport);

        GL.Begin(GL.QUADS);

        GL.Color(new Color(0, 0, 0, 0));

        GL.TexCoord(uv[0]);
        GL.Vertex(warp[0]);

        GL.TexCoord(uv[1]);
        GL.Vertex(warp[1]);

        GL.TexCoord(uv[2]);
        GL.Vertex(warp[2]);

        GL.TexCoord(uv[3]);
        GL.Vertex(warp[3]);

        GL.End();
    }

    #endregion Method
}