// TO USE
//
// 1. Make sure you have set the ambient lighting. Edit -> Render Settings -> Ambient Lighting
// 2. Create a material
// 3. Drag a texture to the "_Texture" box.
// 4. You are done, but you can set how shiny the world is, and the color of shinyness that is applied.
//
// Created by Jason Hein


Shader "Production/WorldShader"
{
	Properties
   {
      _Texture ("Texture", 2D) = "white" {} 
      _SpecColor ("Specular Light Color", Color) = (1.0,1.0,1.0,1.0) 
      _Shininess ("Shininess", Float) = 10.0
   }
	SubShader
	{
		//Pass for ambience and directional light
		Pass 
		{
			Tags { "LightMode" = "ForwardBase" } 
			
			//This is a CG shader
			CGPROGRAM
			
			//Allows us to get ambient lighting
			#include "UnityCG.cginc"
 			
 			//Define the shaders
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//World Light Colour (from "UnityCG.cginc")
         	float4 _LightColor0;
         	
         	//Public Uniforms
         	sampler2D _Texture;
         	
         	
         	//What the vertex shader will recieve
         	struct vertInput
         	{
            	float4 vertex : POSITION;
            	float3 normal : NORMAL;
            	float4 texcoord : TEXCOORD0;
       		};
       		
       		//What the fragment shader willl recieve
         	struct vertOutput
         	{
            	float4 pos : SV_POSITION;
            	float4 posWorld : TEXCOORD0;
            	float3 normalDir : TEXCOORD1;
            	float4 tex : TEXCOORD2;
        	};
        	
         	
         	//Vertex Shader
         	vertOutput vertShader(vertInput input)
         	{
         		//A container for the vertexOutput
         		vertOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
         		
         		//Calculate the real world position of the vertex, for later calculations
         		output.posWorld = mul(_Object2World, input.vertex);
         		
         		//Calculate the direction of our surface normal
         		output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Give output the texture colour
         		output.tex = input.texcoord;
         		
         		//Return our output
         		return output;
         	}
         	
         	//Fragment Shader
         	float4 fragShader(vertOutput output) : COLOR
         	{
         		//All our values are interpolated, so now we can do per-pixel calculations
         		
         		//Re-normalize direction values so they are correct
         		float3 normalDirection = normalize(output.normalDir);
            	float3 viewDirection = normalize(_WorldSpaceCameraPos - float3(output.posWorld.xyz));
            	
            	//Direction of our light, for dot product calculations
            	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
 				
 				//Base colour of this fragment
            	float4 textureColor = tex2D(_Texture, float2(output.tex.xy));
            	
            	//Calculate ambient light
            	float3 ambientLight = textureColor.xyz * UNITY_LIGHTMODEL_AMBIENT.xyz;
            	
            	
            	//Calculate the base colour of the fragment with lighting
            	
            	//Texture colour x light colour x shade based off of a dot product between the surface normal and the light direction (at least being 0).
            	float3 fragmentColour = textureColor.xyz * _LightColor0.xyz * max(0.0, dot(normalDirection, lightDirection));

         		//Return the final colour of the fragment
         		return float4(fragmentColour + ambientLight, 1.0);
         	}
         	
         	
 			//End the cg shader
 			ENDCG
		}
		
		
		//Pass for additional lighting
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" }
			
			//Add the the colour we had already
			Blend One One
			
			//This is a CG shader
			CGPROGRAM
 			
 			//Define the shaders
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	
         	//World Light Colour (from "UnityCG.cginc")
         	float4 _LightColor0;
         	
         	//Public Uniforms
         	sampler2D _Texture;
         	float4 _SpecColor;
         	float _Shininess;
         	
         	
         	//What the vertex shader will recieve
         	struct vertInput
         	{
            	float4 vertex : POSITION;
            	float3 normal : NORMAL;
            	float4 texcoord : TEXCOORD0;
       		};
       		
       		//What the fragment shader willl recieve
         	struct vertOutput
         	{
            	float4 pos : SV_POSITION;
            	float4 posWorld : TEXCOORD0;
            	float3 normalDir : TEXCOORD1;
            	float4 tex : TEXCOORD2;
        	};
        	
        	
        	//Vertex Shader
         	vertOutput vertShader(vertInput input)
         	{
         		//A container for the vertexOutput
         		vertOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
         		
         		//Calculate the real world position of the vertex, for later calculations
         		output.posWorld = mul(_Object2World, input.vertex);
         		
         		//Calculate the direction of our surface normal
         		output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Give output the texture colour
         		output.tex = input.texcoord;
         		
         		//Return our output
         		return output;
         	}
         	
         	//Fragment Shader
         	float4 fragShader(vertOutput output) : COLOR
         	{
         		//All our values are interpolated, so now we can do per-pixel calculations
         		
         		//Re-normalize direction values so they are correct
         		float3 normalDirection = normalize(output.normalDir);
            	float3 viewDirection = normalize(_WorldSpaceCameraPos - output.posWorld.xyz);
            	
            	//Direction of our light, for dot product calculations
            	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz - output.posWorld.xyz);
 				
 				//Base colour of this fragment
            	float4 textureColor = tex2D(_Texture, float2(output.tex.xy));
            	
            	//Calculate shading based off our distance from the lights
            	float3 distShading = 1.0 / length(_WorldSpaceLightPos0.xyz - output.posWorld.xyz);
            	
            	
            	//Calculate the base colour of the fragment with lighting
            	
            	//Texture colour x light colour x shade based off distance from light x shade based off
            	//of a dot product between the surface normal and the light direction (at least being 0).
            	float3 fragmentColour = textureColor.xyz * _LightColor0.xyz * distShading * max(0.0, dot(normalDirection, lightDirection));
            	
            	
            	//Specular reflection
            	float3 specularReflection;
            	
            	//If we are facing the light, so calculate specular lighting
            	if (dot(normalDirection, lightDirection) > 0.0)
            	{
            		specularReflection = _LightColor0.xyz * _SpecColor.xyz * distShading * (1.0 - textureColor.a) *
            		pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
            	}
            	
            	//Otherwise their is no specular lighting
            	else
            	{
            		specularReflection = float3(0.0, 0.0, 0.0);
            	}
            	

         		//Return the final colour of the fragment
         		return float4(fragmentColour + specularReflection, 1.0);
         	}
         	
         	
         	//End the cg shader
 			ENDCG
		}
	}
}