using UnityEngine;
using UnityEngine.Assertions;

public class ProjectionManagerRoomSample : MonoBehaviour
{
    #region Field

    public ProjectionManagerRoom projectionManager;

    public Camera cameraFront;
    public Camera cameraLeft;
    public Camera cameraRight;
    public Camera cameraBack;
    public Camera cameraBottom;

    public int resolutionX = 1920;
    public int resolutionY = 1080;
    public int resolutionZ = 0;

    protected RenderTexture renderTextureFront;
    protected RenderTexture renderTextureLeft;
    protected RenderTexture renderTextureRight;
    protected RenderTexture renderTextureBack;
    protected RenderTexture renderTextureBottom;

    #endregion Field

    #region Method

    protected virtual void Awake()
    {
        Assert.IsNotNull(cameraFront);
        Assert.IsNotNull(cameraLeft);
        Assert.IsNotNull(cameraRight);
        Assert.IsNotNull(cameraBack);
        Assert.IsNotNull(cameraBottom);

        renderTextureFront  = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        renderTextureLeft   = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        renderTextureRight  = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        renderTextureBack   = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        renderTextureBottom = new RenderTexture(resolutionX, resolutionY, resolutionZ);

        cameraFront .targetTexture = renderTextureFront;
        cameraLeft  .targetTexture = renderTextureLeft;
        cameraRight .targetTexture = renderTextureRight;
        cameraBack  .targetTexture = renderTextureBack;
        cameraBottom.targetTexture = renderTextureBottom;

        projectionManager.textureFront  = renderTextureFront;
        projectionManager.textureLeft   = renderTextureLeft;
        projectionManager.textureRight  = renderTextureRight;
        projectionManager.textureBack   = renderTextureBack;
        projectionManager.textureBottom = renderTextureBottom;
    }

    protected virtual void OnDestroy()
    {
        // NOTE:
        // Camera shows null in sometimes. For example when scene will be closed.

        ClearRenderTarget(cameraFront);
        ClearRenderTarget(cameraLeft);
        ClearRenderTarget(cameraRight);
        ClearRenderTarget(cameraBack);
        ClearRenderTarget(cameraBottom);

        DestroyImmediate(renderTextureFront);
        DestroyImmediate(renderTextureLeft);
        DestroyImmediate(renderTextureRight);
        DestroyImmediate(renderTextureBack);
        DestroyImmediate(renderTextureBottom);
    }

    protected virtual void ClearRenderTarget(Camera camera) 
    {
        if (camera != null)
        {
            camera.targetTexture = null;
        }
    }

    #endregion Method
}