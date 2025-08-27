//This shader from http://wiki.unity3d.com/index.php/TexturedFont
Shader "GUI/Textured Text Shader"
 {
 	Properties {
 		_MainTex ("Font Texture", 2D) = "white" {}
 		_Color ("Text Color", Color) = (1,1,1,1)
 	}
 	
 	SubShader {
 		Lighting Off
 		cull off
 		Zwrite off
 		Fog { Mode Off }
		Tags { "Queue"="Transparent" } 
 		Pass {
 			Blend SrcAlpha OneMinusSrcAlpha
 			SetTexture [_MainTex] {
 				constantColor [_Color]
 				Combine texture * constant, texture * constant
 			}
 		}
 	}
 }