// TO USE
//
// 1. Create a darkness material (unless one has been made)
// 2. Apply the material to the object
// 3. Drag a texture to the "_Texture" box
// 3. You are done
//
// Created by Jason Hein


Shader "Production/Light_Bulb"
{
	//Properties that can be set by designers
	Properties
	{
		_Color ("Color", Color) = (0.0, 0.0, 0.0, 0.1)
		_TransparencyGrow("Transprency Growing", Float) = 20.0
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
         	float _TransparencyGrow;
         	float4 _Color;
         	
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
 				float affectOfColorOpacity = _Color.a * 0.05 + 0.98;
            	float newOpacity = 	pow(min(1.0, affectOfColorOpacity - dot(viewDirection, normalDirection) * (1.0 / _TransparencyGrow)), _TransparencyGrow);
            	if (newOpacity < 0.01)
            	{
            		discard;
            	}
            	
            	//Calculate the colour of this fragment
            	float4 fragmentColour = float4 (_Color.xyz, newOpacity);
            	
            	//Return the colour of the first pass's fragment
            	return fragmentColour;
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
			
			//Our blend equation is multiplicative
         	Blend One One
         	
         	CGPROGRAM
         	
         	//Allows us to get offset and tiling
			#include "UnityCG.cginc"
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//Public Uniforms;
         	float _TransparencyGrow;
         	float4 _Color;
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
 				float affectOfColorOpacity = _Color.a * 0.05 + 0.98;
            	float newOpacity = 	pow(min(1.0, affectOfColorOpacity - dot(viewDirection, normalDirection) * (1.0 / _TransparencyGrow)), _TransparencyGrow);
            	if (newOpacity < 0.01)
            	{
            		discard;
            	}
            	
            	//Direction of our light, for dot product calculations
            	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz - output.worldPos.xyz);
            	
            	//Shading for being far from the point light
            	float distShading = 1.0 / pow(length(_WorldSpaceLightPos0.xyz - output.worldPos.xyz), 2.0);
            	
            	//Calculate lighting for lights inside this object
            	float4 illumination = float4(_LightColor0.xyz * distShading * _Color.a * newOpacity * max(0.0, dot(normalize(normalDirection + viewDirection), -lightDirection)), 1.0);
            	
            	//Return the colour of illumination to add to this fragments current color
            	return illumination;
         	}
         	
         	ENDCG
		}
	}
}
