﻿Shader "Cg per-vertex diffuse lighting" {
   Properties {
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
   }
   SubShader {
      Pass {	
         Tags { "LightMode" = "ForwardBase" } 
            // make sure that all uniforms are correctly set
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
 
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         uniform float4 _Color; // define shader property for shaders
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            float3 normalDirection = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            float3 lightDirection;
            float attenuation;
            
            
 			if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               lightDirection = normalize(float3(_WorldSpaceLightPos0.xyz));
               attenuation = 1.0; // no attenuation
            } 
            else // point or spot light
            {
               lightDirection = normalize(_WorldSpaceLightPos0 - mul(modelMatrix, input.vertex).xyz);
               attenuation = 1.0 / length(lightDirection * lightDirection);
            }
 
 
            float3 diffuseReflection = (_LightColor0.xyz * attenuation) * _Color.xyz * max(0.0, dot(normalDirection, lightDirection));
 
            output.col = float4(diffuseReflection, 1.0);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            return input.col;
         }
 
         ENDCG
      }
      Pass {	
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
 
         uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         uniform float4 _Color; // define shader property for shaders
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
            float4x4 modelMatrix = _Object2World;
            float4x4 modelMatrixInverse = _World2Object; 
               // multiplication with unity_Scale.w is unnecessary 
               // because we normalize transformed vectors
 
            float3 normalDirection = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
            float3 lightDirection;
            float attenuation;
 
            if (0.0 == _WorldSpaceLightPos0.w) // directional light?
            {
               lightDirection = normalize(float3(_WorldSpaceLightPos0.xyz));
               attenuation = 1.0; // no attenuation
            } 
            else // point or spot light
            {
               lightDirection = normalize(_WorldSpaceLightPos0 - mul(modelMatrix, input.vertex).xyz);
               attenuation = 1.0 / length(lightDirection * lightDirection);
            }
 
            float3 diffuseReflection = (_LightColor0.xyz * attenuation) * _Color.xyz * max(0.0, dot(normalDirection, lightDirection));
 
            output.col = float4(diffuseReflection, 1.0);
            output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            return input.col;
         }
 
         ENDCG
      }
   }
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Diffuse"
}