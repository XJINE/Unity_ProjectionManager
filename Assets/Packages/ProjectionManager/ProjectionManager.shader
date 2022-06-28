Shader "ProjectionManager"
{
    // NOTE:
    // This is needed to render a texture which has an alpha-channel.
    // Default material of Unity/UnlitTexture doesn't consider the alpha.

    Properties
    {
        [HideInInSpector] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.uv);
                return color;
            }

            ENDCG
        }
    }
}