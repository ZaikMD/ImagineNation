﻿// TO USE
//
// 1. Create a darkness material (unless one has been made)
// 2. Drag a texture to the "_Texture" box
// 3. You are done
//
// Created by Jason Hein



Shader "Production/DarknessShader"
{
	//Properties that can be set by designers
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FogTint("Fog Tint", Color) = (1.0, 1.0, 0.0, 1.0)
		_OffsetSpeed ("Fog Move Speed", Float) = 0.1
	}
	
	//Shader
	Subshader
	{
		Tags { "Queue" = "Transparent" } 
		
		//Pass for background enviroment
		Pass
		{
			//Do not remove the colours behind the object
			Cull off
			ZWrite Off
			
			//Our blend equation is multiplicative
         	Blend Zero OneMinusSrcAlpha
         	
         	CGPROGRAM
         	
         	//Allows us to get offset and tiling
			#include "UnityCG.cginc"
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	
         	//Public Uniforms
         	sampler2D _MainTex;
         	float4 _FogTint;
         	float _OffsetSpeed;
         	float4 _MainTex_ST; 
         	
         	
         	//What the vertex shader will recieve
         	struct vertexInput
         	{
         		float4 vertex : POSITION;
         		float3 normal : NORMAL;
            	float4 texcoord : TEXCOORD0;
         	};
         	
         	//What the fragment shader willl recieve
         	struct vertexOutput
         	{
         		float4 pos : SV_POSITION;
            	float3 normal : TEXCOORD0;
            	float3 viewDir : TEXCOORD1;
            	half2 tex : TEXCOORD2;
         	};
         	
         	//Vertex Shader
         	vertexOutput vertShader(vertexInput input)
         	{
         		//A container for the vertexOutput
         		vertexOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
         		
         		//Calculate the normal of the surface in object coordinates
         		output.normal = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Calculate view direction, for dot calculations in the fragment shader
         		output.viewDir = normalize(_WorldSpaceCameraPos - mul(_Object2World, input.vertex).xyz);
         		
         		//Give output the texture's UV
         		output.tex = input.texcoord * _MainTex_ST.xy + _MainTex_ST.zw + _Time.x * _OffsetSpeed;
         		output.tex += _Time.x * _OffsetSpeed;
         		
         		
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
            	float newOpacity = min(1.0, (abs(dot(viewDirection, normalDirection) * 1.1)) * _FogTint.a);
            	
            	if (newOpacity < 0.05)
            	{
            		discard;
            	}
            	
            	//Base colour of this fragment
            	float4 textureColor = tex2D(_MainTex, output.tex);
            	
            	
            	//Calculate the colour of this fragment
            	float4 fragmentColour = float4 (textureColor.xyz * _FogTint.xyz, newOpacity);
            	
            	
            	//Return the colour of the first pass's fragment
            	return fragmentColour;
         	}
         	
         	
         	ENDCG
		}
		
		//Pass for drawing the darkness
		Pass
		{
			//Do not remove the colours behind the object
			Cull off
			ZWrite Off
			
			//Our blend equation is multiplicative
         	Blend SrcAlpha One
         	
         	CGPROGRAM
         	
         	//Allows us to get offset and tiling
			#include "UnityCG.cginc"
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	
         	//Public Uniforms
         	sampler2D _MainTex;
         	float4 _FogTint;
         	float _OffsetSpeed;
         	float4 _MainTex_ST;
         	
         	
         	//What the vertex shader will recieve
         	struct vertexInput
         	{
         		float4 vertex : POSITION;
         		float3 normal : NORMAL;
            	float4 texcoord : TEXCOORD0;
         	};
         	
         	//What the fragment shader willl recieve
         	struct vertexOutput
         	{
         		float4 pos : SV_POSITION;
            	float3 normal : TEXCOORD0;
            	float3 viewDir : TEXCOORD1;
            	half2 tex : TEXCOORD2;
         	};
         	
         	//Vertex Shader
         	vertexOutput vertShader(vertexInput input)
         	{
         		//A container for the vertexOutput
         		vertexOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
         		
         		//Calculate the normal of the surface in object coordinates
         		output.normal = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Calculate view direction, for dot calculations in the fragment shader
         		output.viewDir = normalize(_WorldSpaceCameraPos - mul(_Object2World, input.vertex).xyz);
         		
         		//Give output the texture's UV
         		output.tex = input.texcoord * _MainTex_ST.xy + _MainTex_ST.zw + _Time.x * _OffsetSpeed;
         		output.tex += _Time.x * _OffsetSpeed;
         		
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
            	float newOpacity = min(1.0, (abs(dot(viewDirection, normalDirection) * 1.1)) * _FogTint.a);
            	
            	if (newOpacity < 0.05)
            	{
            		discard;
            	}
            	
            	//Base colour of this fragment
            	float4 textureColor = tex2D(_MainTex,  output.tex);
            	
            	
            	//Calculate the colour of this fragment
            	float4 fragmentColour = float4 (textureColor.xyz * _FogTint.xyz, newOpacity);
            	
            	
            	//Return the colour of the first pass's fragment
            	return fragmentColour;
         	}
         	
         	
         	ENDCG
		}
	}
}
