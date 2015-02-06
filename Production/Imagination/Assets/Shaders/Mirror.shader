// TO USE
//
// 1. Create a cubemap and fill it with six textures.
// 2. Add the Cubemap to _CubeMap.
// 3. You are done, and this object should have a sort of distortion effect.
//
// Created by Jason Hein on Feb 6th, 2015.


Shader "Production/Mirror"
{
   Properties
   {
      _CubeMap("Reflection Map", Cube) = "" {}
   }
   SubShader
   {
      Pass
      {   
         CGPROGRAM
 
         #pragma vertex vertexShader 
         #pragma fragment fragShader
 
         #include "UnityCG.cginc"
 
         //Cubemap for the mirror
         samplerCUBE _CubeMap;
         
         //What the vertex shader recieves
         struct vertexInput
         {
            float4 pos : POSITION;
            float3 norm : NORMAL;
         };
         
         //What the vertex shader outputs and the fragment shader receives
         struct vertexOutput
         {
            float4 pos : SV_POSITION;
            float3 norm : TEXCOORD0;
            float3 viewDirection : TEXCOORD1;
         };
         
         //The vertex shader
         vertexOutput vertexShader (vertexInput input) 
         {
         	//Declare a vertex output variable
            vertexOutput output;
               
            //Provide vertex output our position on the camera
            output.pos = mul(UNITY_MATRIX_MVP, input.pos);
			
			//Provide the fragment shader the direction our vertecy was viewed, for the mirror's reflect calculation
            output.viewDirection = mul(_Object2World, input.pos).xyz - _WorldSpaceCameraPos;
			
            //Provide the fragment shader our vertecies normal, for the mirror's reflect calculation
            output.norm = normalize(mul(float4(input.norm, 0.0), _World2Object).xyz);
			
            //Send the fragment shader our vertexOutput
            return output;
         }
         
         //The fragment shader
         float4 fragShader (vertexOutput output) : COLOR
         {
            //Returns a texture color
            return texCUBE(_CubeMap, reflect(output.viewDirection, normalize(output.norm)));
         }
 
         ENDCG
      }
   }
}