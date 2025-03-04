Shader "UI/Unlit/MyUIMask" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_MaskTex("MaskTex", 2D) = "white" {}
		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5

			//MASK SUPPORT ADD
			_StencilComp("Stencil Comparison", Float) = 8
			_Stencil("Stencil ID", Float) = 0
			_StencilOp("Stencil Operation", Float) = 0
			_StencilWriteMask("Stencil Write Mask", Float) = 255
			_StencilReadMask("Stencil Read Mask", Float) = 255
			_ColorMask("Color Mask", Float) = 15
			//MASK SUPPORT END
	}
		SubShader{
			Tags {
				"IgnoreProjector" = "True"
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}
			//MASK SUPPORT ADD
			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}
			ColorMask[_ColorMask]
			//MASK SUPPORT END
			Pass {
				Name "FORWARD"
				Tags {
					"LightMode" = "ForwardBase"
				}
				Blend SrcAlpha OneMinusSrcAlpha
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#define UNITY_PASS_FORWARDBASE
				#include "UnityCG.cginc"
				#pragma multi_compile_fwdbase
				#pragma only_renderers d3d9 d3d11 glcore gles
				#pragma target 3.0
				uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
				uniform sampler2D _MaskTex; uniform float4 _MaskTex_ST;
				struct VertexInput
				{
					float4 vertex : POSITION;
					float2 texcoord0 : TEXCOORD0;
				};
				struct VertexOutput
				{
					float4 pos : SV_POSITION;
					float2 uv0 : TEXCOORD0;
				};

				VertexOutput vert(VertexInput v)
				{
					VertexOutput o = (VertexOutput)0;
					o.uv0 = v.texcoord0;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}

				float4 frag(VertexOutput i) : COLOR
				{
					float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
					float3 finalColor = _MainTex_var.rgb;
					float4 _MaskTex_var = tex2D(_MaskTex,TRANSFORM_TEX(i.uv0, _MaskTex));
					return fixed4(finalColor,_MaskTex_var.a);
				}
				ENDCG
			}
		}
		FallBack "Diffuse"
}