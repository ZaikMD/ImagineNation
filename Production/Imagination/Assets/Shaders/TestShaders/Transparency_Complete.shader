Shader "Messing with Sillouettes" {
   Properties {
      _Color ("Color", Color) = (1, 1, 1, 0.5) 
         // user-specified RGBA color including opacity
   }
   SubShader {
      Tags { "Queue" = "Transparent" } 
         // draw after all opaque geometry has been drawn
      Pass { 
      	 Cull off
      
         ZWrite Off // don't occlude other objects
         //Blend SrcAlpha OneMinusSrcAlpha // standard alpha blending
         Blend Zero OneMinusSrcAlpha // multiplicative blending 
 
         CGPROGRAM 
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
 
         uniform float4 _Color; // define shader property for shaders
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 normal : TEXCOORD;
            float3 viewDir : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.normal = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.viewDir = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz);
 
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normal);
            float3 viewDirection = normalize(input.viewDir);
 
 			//Faces that are away from the camera will be more transparent
            float newOpacity = min(1.0, (abs(dot(viewDirection, normalDirection) * 0.75f)) / _Color.a);
            return float4(_Color.xyz, newOpacity);
         }
 
         ENDCG
      }
      pass
      {
      	 Cull off
      
         ZWrite Off // don't occlude other objects
         //Blend SrcAlpha OneMinusSrcAlpha // standard alpha blending
         //Blend Zero OneMinusSrcAlpha // multiplicative blending 
         Blend SrcAlpha One
 
         CGPROGRAM 
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
 
         uniform float4 _Color; // define shader property for shaders
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float3 normal : TEXCOORD;
            float3 viewDir : TEXCOORD1;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            output.normal = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            output.viewDir = normalize(_WorldSpaceCameraPos - mul(modelMatrix, input.vertex).xyz);
 
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            float3 normalDirection = normalize(input.normal);
            float3 viewDirection = normalize(input.viewDir);
 
 			//Faces that are away from the camera will be more transparent
            float newOpacity = min(1.0, (abs(dot(viewDirection, normalDirection) * 0.75f)) / _Color.a);
            return float4(_Color.xyz, newOpacity);
         }
 
         ENDCG
      }
   }
}