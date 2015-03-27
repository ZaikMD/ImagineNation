// TO USE
//
// 1. Create a light bulb or lamp material.
// 2. Apply the material to the object
// 3. Choose the color and opacity of the object, and choose a nice opacity growing value.
// 4. Choose how affected this object is affected by normal diffuse lighting (for less or more lit glass)
// 5. You are done
//
// Created by Jason Hein


Shader "Production/Light_Bulb"
{
	//Properties that can be set by designers
	Properties
	{
		_GlassColor ("Color", Color) = (0.3, 0.3, 0.3, 0.1)
		_OpacityGrow("Opacity Grow", Float) = 0.5
		_MinimumOpacity ("Minimum Opacity", Float) = 0.1
		_MaximumOpacity ("Maximum Opacity", Float) = 1.0
		_Gloss ("Gloss", Float) = 10.0
		_DiffuseLightingStrength ("Diffuse Lighting Strength", Float) = 0.5
	}
	
	//Shader
	Subshader
	{
		Tags { "Queue" = "Transparent" } 
		
		//Pass for shading the background enviroment
		Pass
		{
			//Do not remove the colours behind the object
			Cull back
			ZWrite Off
			
			//Our blend equation is multiplicative
         	Blend SrcAlpha OneMinusSrcAlpha
         	
         	CGPROGRAM
         	
         	//Allows us to get offset and tiling
			#include "UnityCG.cginc"
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//Public Uniforms;
         	float4 _GlassColor;
         	float _OpacityGrow;
         	float _MinimumOpacity;
         	float _MaximumOpacity;
         	
         	//What the vertex shader will recieve
         	struct vertexInput
         	{
         		float4 pos : POSITION;
         		float3 normal : NORMAL;
         	};
         	
         	//What the fragment shader will recieve
         	struct vertexOutput
         	{
         		float4 pos : SV_POSITION;
            	float3 normal : TEXCOORD0;
            	float3 viewDir : TEXCOORD1;
         	};
         	
         	//Vertex Shader
         	vertexOutput vertShader(vertexInput input)
         	{
         		//A container for the vertexOutput
         		vertexOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Calculate the normal of the surface in object coordinates
         		output.normal = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Calculate view direction, for dot calculations in the fragment shader
         		output.viewDir = normalize(_WorldSpaceCameraPos - mul(_Object2World, input.pos).xyz);
         		
         		//Return our output
         		return output;
         	}
         	
         	//Fragment Shader
         	float4 fragShader (vertexOutput output) : COLOR
         	{
         		//Re-normalize some interpolated vertex output
         		float3 normalDirection = normalize(output.normal);
            	float3 viewDirection = normalize(output.viewDir);
 
 				//Calculate a new opacity for faces that are facing away from the camera
            	float newOpacity = min(_MaximumOpacity, max(_MinimumOpacity, _GlassColor.a / max(0.01, (pow(dot(viewDirection, normalDirection), _OpacityGrow)))));

            	//Return the colour of the first pass's fragment
            	return float4 (_GlassColor.xyz, newOpacity);
         	}
         	
         	ENDCG
		}
		
		//Pass for specular highlights
		Pass
		{
			//Point lights
			Tags { "LightMode" = "ForwardAdd" }
		
			//Do not remove the colours behind the object
			Cull back
			ZWrite Off
			
			//Our blend equation is additive
         	Blend One One
         	
         	CGPROGRAM
         	
         	//Allows us to get offset and tiling
			#include "UnityCG.cginc"
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//Public Uniforms;
         	float4 _GlassColor;
         	float _OpacityGrow;
         	float _MinimumOpacity;
         	float _MaximumOpacity;
         	float _Gloss;
         	float _DiffuseLightingStrength;
         	float4 _LightColor0;
         	
         	//What the vertex shader will recieve
         	struct vertexInput
         	{
         		float4 pos : POSITION;
         		float3 normal : NORMAL;
         	};
         	
         	//What the fragment shader will recieve
         	struct vertexOutput
         	{
         		float4 pos : SV_POSITION;
            	float3 normal : TEXCOORD0;
            	float3 viewDir : TEXCOORD1;
            	float4 worldPos : TEXCOORD2;
         	};
         	
         	//Vertex Shader
         	vertexOutput vertShader(vertexInput input)
         	{
         		//A container for the vertexOutput
         		vertexOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Provide the fragment shader our world position
         		output.worldPos = mul (_Object2World, input.pos);
         		
         		//Calculate the normal of the surface in object coordinates
         		output.normal = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Calculate view direction, for dot calculations in the fragment shader
         		output.viewDir = normalize(_WorldSpaceCameraPos - mul(_Object2World, input.pos).xyz);
         		
         		//Return our output
         		return output;
         	}
         	
         	//Fragment Shader
         	float4 fragShader (vertexOutput output) : COLOR
         	{
         		//Re-normalize some interpolated vertex output
         		float3 normalDirection = normalize(output.normal);
            	float3 viewDirection = normalize(output.viewDir);
 
 				//Calculate a new opacity for faces that are facing away from the camera
            	float newOpacity = min(_MaximumOpacity, max(_MinimumOpacity, _GlassColor.a / max(0.01, (pow(dot(viewDirection, normalDirection), _OpacityGrow)))));
            	
            	//Direction of our light, for dot product calculations
            	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz - output.worldPos.xyz);
            	
            	//Shading for being far from the point light
            	float distShading = 1.0 / pow(length(_WorldSpaceLightPos0.xyz - output.worldPos.xyz), 2.0);
            	
            	//Shading
            	float diffuseShading = max(0.0, dot(normalize(normalDirection), -lightDirection));
            	float specularShading = max(0.0, pow(dot(normalize(normalDirection), viewDirection), _Gloss));
            	float attenuation = distShading * newOpacity * (diffuseShading * _DiffuseLightingStrength + specularShading);
            	
            	//Return the colour of illumination to add to this fragments current color
            	return float4((_LightColor0.xyz * _GlassColor.xyz) * attenuation, 1.0);
         	}
         	
         	ENDCG
		}
	}
}