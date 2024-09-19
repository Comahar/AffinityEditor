/*
* BEWARE: Prepare for bad shader code.
* This is written from scratch because I could not extract the shader code from the game.
* This shader allows us to have rounded corners in editor, and not needed in the game runtime.
*/
Shader "Unlit/RoundRectAuto" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Margin ("Margin", Float) = 8
        _Radius ("Radius", Float) = 1
        _Stroke ("Stroke", Float) = 1
        _OutlineColor ("OutlineColor", Vector) = (1,1,1,1)
        _FillColor ("FillColor", Vector) = (0,0,0,0.5)
        _FracStart ("FracStart", Float) = 0
        _FracEnd ("FracEnd", Float) = 1
        _BoilAmp ("BoilAmp", Float) = 0
        _BoilFreq ("BoilFreq", Float) = 0
        _BoilTime ("BoilTime", Float) = 0
        _SizeW ("Size W", Float) = 0
        _SizeH ("Size H", Float) = 0
        _Scale ("Scale", Float) = 1
        _ScreenScale ("ScreenScale", Float) = 1

        // --- Mask support ---
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        // --- Mask support ---
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        Lighting Off
        ZTest [unity_GUIZTestMode]
        ColorMask [_ColorMask]
        // ---
        
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZWrite Off

        LOD 100

        Pass {
            CGPROGRAM

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"          

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Radius;
            float _Stroke;
            float4 _OutlineColor;
            float4 _FillColor;
            float _SizeW;
            float _SizeH;

            v2f vert (appdata v) {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 color = tex2D(_MainTex, i.uv);
                float2 size = float2(_SizeW, _SizeH);

                float2 screenUV = i.uv * size;
                
                float2 bottomLeftDist = screenUV;
                float2 topRightDist = size - screenUV;
                float2 origin;
                if (bottomLeftDist.x < _Radius && bottomLeftDist.y < _Radius) {
                    // bottom left
                    origin = float2(_Radius, _Radius);
                } else if (topRightDist.x < _Radius && bottomLeftDist.y < _Radius) {
                    // bottom right
                    origin = float2(size.x - _Radius, _Radius);
                } else if (bottomLeftDist.x < _Radius && topRightDist.y < _Radius) {
                    // top left
                    origin = float2(_Radius, size.y - _Radius);
                } else if (topRightDist.x < _Radius && topRightDist.y < _Radius) {
                    // top right
                    origin = float2(size.x - _Radius, size.y - _Radius);
                } else {
                    // inside
                    if (
                        min(
                            min(screenUV.x, screenUV.y),
                            min(size.x - screenUV.x, size.y - screenUV.y)
                        ) < _Stroke
                    ) {
                        return _OutlineColor;
                    } else {
                        
                        return _FillColor * color * i.color;
                    }
                }

                float distanceToOrigin = distance(screenUV, origin);
                
                // debugging origins
                /*if (distanceToOrigin < 5) {
                    return float4(1,1,1,1);
                }*/

                if (distanceToOrigin < _Radius) {
                    if (distanceToOrigin + _Stroke > _Radius) {
                        // outline
                        return _OutlineColor;
                    } else {
                        // fill
                        return _FillColor * color * i.color;
                    }
                } else {
                    #ifdef UNITY_UI_ALPHACLIP
                    clip(-1);
                    #endif
                    return float4(0,0,0,0);
                }
            }
            ENDCG
        }
    }
}