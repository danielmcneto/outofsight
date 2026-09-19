Shader "oos/oos-map"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Green ("Color", Color) = (0, 1, 0, 1)
        _Brightness ("Brightness", Range(0, 3)) = 1
        _Contrast ("Contrast", Range(0, 3)) = 1.5
    }

    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Green;
            float _Brightness;
            float _Contrast;

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //grayscale the image
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                //contrast
                gray = (gray - 0.5) * _Contrast + 0.5;
                //brightness
                gray *= _Brightness;
                //clamp saturation
                gray = saturate(gray);
                //make black green
                return fixed4(_Green.rgb * gray, 1);
            }

            ENDCG
        }
    }
}