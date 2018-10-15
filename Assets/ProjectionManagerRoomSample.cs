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
        Assert.IsNotNull<Camera>(this.cameraFront);
        Assert.IsNotNull<Camera>(this.cameraLeft);
        Assert.IsNotNull<Camera>(this.cameraRight);
        Assert.IsNotNull<Camera>(this.cameraBack);
        Assert.IsNotNull<Camera>(this.cameraBottom);

        this.renderTextureFront  = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        this.renderTextureLeft   = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        this.renderTextureRight  = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        this.renderTextureBack   = new RenderTexture(resolutionX, resolutionY, resolutionZ);
        this.renderTextureBottom = new RenderTexture(resolutionX, resolutionY, resolutionZ);

        this.cameraFront.targetTexture  = this.renderTextureFront;
        this.cameraLeft.targetTexture   = this.renderTextureLeft;
        this.cameraRight.targetTexture  = this.renderTextureRight;
        this.cameraBack.targetTexture   = this.renderTextureBack;
        this.cameraBottom.targetTexture = this.renderTextureBottom;

        this.projectionManager.textureFront  = this.renderTextureFront;
        this.projectionManager.textureLeft   = this.renderTextureLeft;
        this.projectionManager.textureRight  = this.renderTextureRight;
        this.projectionManager.textureBack   = this.renderTextureBack;
        this.projectionManager.textureBottom = this.renderTextureBottom;
    }

    protected virtual void OnDestroy()
    {
        // NOTE:
        // Camera shows null in sometimes. For example when scene will be closed.

        ClearRenderTarget(this.cameraFront);
        ClearRenderTarget(this.cameraLeft);
        ClearRenderTarget(this.cameraRight);
        ClearRenderTarget(this.cameraBack);
        ClearRenderTarget(this.cameraBottom);

        DestroyImmediate(this.renderTextureFront);
        DestroyImmediate(this.renderTextureLeft);
        DestroyImmediate(this.renderTextureRight);
        DestroyImmediate(this.renderTextureBack);
        DestroyImmediate(this.renderTextureBottom);
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