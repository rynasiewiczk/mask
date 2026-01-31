Shader "Sprites/Gradient"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ColorBottom ("Bottom Color", Color) = (1,1,1,1)
        _ColorTop    ("Top Color", Color)    = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _ColorBottom;
            fixed4 _ColorTop;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                fixed4 color  : COLOR;   // SpriteRenderer tint
            };

            struct v2f
            {
                float4 pos   : SV_POSITION;
                float2 uv    : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample only to get sprite alpha/shape
                fixed4 tex = tex2D(_MainTex, i.uv);

                // vertical gradient based on sprite UV.y (0 = bottom, 1 = top)
                fixed4 grad = lerp(_ColorBottom, _ColorTop, saturate(i.uv.y));

                // apply SpriteRenderer tint
                grad *= i.color;

                // keep sprite transparency
                grad.a *= tex.a;

                // premultiply for clean edges with Blend One OneMinusSrcAlpha
                grad.rgb *= grad.a;

                return grad;
            }
            ENDCG
        }
    }
}
