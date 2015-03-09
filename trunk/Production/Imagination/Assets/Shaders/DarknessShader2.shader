// TO USE
//
// 1. Create a darkness material (unless one has been made)
// 2. Apply the material to the object
// 3. Drag a texture to the "_Texture" box
// 3. You are done
//
// Created by Jason Hein


Shader "Production/DarknessShader2"
{
	//Properties that can be set by designers
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MistTint("Mist Tint", Color) = (1.0, 1.0, 0.0, 1.0)
		_FogTint("Fog Tint", Color) = (1.0, 1.0, 0.0, 1.0)
		_OffsetSpeed ("Fog Move Speed", float) = 1
		_TransparencyGrow("Transprency Growing", float) = 0.75
		
		_NoiseTex ("Texture", 2D) = "white" {}
		_NoiseTexSize("Noise Texture Siaze", int) = 1024
		_Noise("Noise", vector) = (0.05, 0.05, 0.05)
		_NoiseSpeed ("Noise Speed", vector) = (1, 1, 1)
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
         	
         	//Public Uniforms
         	float4 _FogTint;
         	float _OffsetSpeed;
         	float _TransparencyGrow;
         	float4 _MistTint;
         	
         	
         	uniform sampler2D _NoiseTex;
         	uniform uint _NoiseTexSize;
			uniform float3 _Noise;
			uniform float3 _NoiseSpeed;
         	
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
         		//float4 noisePos = float4(abs(input.pos.x), abs(input.pos.y), abs(input.pos.z), 0.0);
         		float4 noisePos = input.pos;
         		
         		noisePos.x += _Time * _NoiseSpeed.x;
         		if(noisePos.x > 0.0f)
         		{ 		
         			input.pos.x += (tex2Dlod(_NoiseTex, float4((noisePos.x - noisePos.x % _NoiseTexSize) / _NoiseTexSize, noisePos.x % _NoiseTexSize, 0, 0)) - 0.5f)  * _Noise.x;
         		}
         		else
         		{
         			input.pos.x -= (tex2Dlod(_NoiseTex, float4((noisePos.x - noisePos.x % _NoiseTexSize) / _NoiseTexSize, noisePos.x % _NoiseTexSize, 0, 0)) - 0.5f)  * _Noise.x;
         		}
         		
         		noisePos.y += _Time * _NoiseSpeed.y;
         		if(noisePos.x > 0.0f)
         		{         		
         			input.pos.y += (tex2Dlod(_NoiseTex, float4((noisePos.y - noisePos.y % _NoiseTexSize) / _NoiseTexSize, noisePos.y % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.y;
         		}
         		else
         		{
         			input.pos.y -= (tex2Dlod(_NoiseTex, float4((noisePos.y - noisePos.y % _NoiseTexSize) / _NoiseTexSize, noisePos.y % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.y;
         		}
         		
         		noisePos.z += _Time * _NoiseSpeed.z;   
         		if(noisePos.x > 0.0f)
         		{      		
         			input.pos.z += (tex2Dlod(_NoiseTex, float4((noisePos.z - noisePos.z % _NoiseTexSize) / _NoiseTexSize, noisePos.z % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.z;
         		}
         		else
         		{
         			input.pos.z -= (tex2Dlod(_NoiseTex, float4((noisePos.z - noisePos.z % _NoiseTexSize) / _NoiseTexSize, noisePos.z % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.z;
         		}
         		
         		//A container for the vertexOutput
         		vertexOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Calculate the normal of the surface in object coordinates
         		output.normal = normalize(mul(float4(input.normal, 0.0), _World2Object).yyz);
         		
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
            	float newOpacity = pow(min(1.0, (dot(viewDirection, normalDirection) * _MistTint.a *
            							(1.0 / _TransparencyGrow))), _TransparencyGrow);
            	if (newOpacity < 0.02)
            	{
            		discard;
            	}
            	
            	//Calculate the colour of this fragment
            	float4 fragmentColour = float4 (_MistTint.xyz, newOpacity);
            	
            	
            	//Return the colour of the first pass's fragment
            	return fragmentColour;
         	}
         	
         	ENDCG
		}
		
		//Pass for drawing the fog within the darkness
		Pass
		{
			//stencil to prevent multiple iterations of blending due to noise
			Stencil
			{
				Ref 1
				Comp Greater
				Pass replace
			}
			
			//Do not remove the colours behind the object
			Cull off
			ZWrite Off
			
			//Our blend equation is additive
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
         	float _TransparencyGrow;
         	
         	uniform sampler2D _NoiseTex;
         	uniform uint _NoiseTexSize;
			uniform float3 _Noise;
			uniform float3 _NoiseSpeed;
         	
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
         		
         		//float4 noisePos = float4(abs(input.pos.x), abs(input.pos.y), abs(input.pos.z), 0.0);
         		float4 noisePos = input.vertex;
         		
         		noisePos.x += _Time * _NoiseSpeed.x;
         		if(noisePos.x > 0.0f)
         		{ 		
         			input.vertex.x += (tex2Dlod(_NoiseTex, float4((noisePos.x - noisePos.x % _NoiseTexSize) / _NoiseTexSize, noisePos.x % _NoiseTexSize, 0, 0)) - 0.5f)  * _Noise.x;
         		}
         		else
         		{
         			input.vertex.x -= (tex2Dlod(_NoiseTex, float4((noisePos.x - noisePos.x % _NoiseTexSize) / _NoiseTexSize, noisePos.x % _NoiseTexSize, 0, 0)) - 0.5f)  * _Noise.x;
         		}
         		
         		noisePos.y += _Time * _NoiseSpeed.y;
         		if(noisePos.x > 0.0f)
         		{         		
         			input.vertex.y += (tex2Dlod(_NoiseTex, float4((noisePos.y - noisePos.y % _NoiseTexSize) / _NoiseTexSize, noisePos.y % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.y;
         		}
         		else
         		{
         			input.vertex.y -= (tex2Dlod(_NoiseTex, float4((noisePos.y - noisePos.y % _NoiseTexSize) / _NoiseTexSize, noisePos.y % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.y;
         		}
         		
         		noisePos.z += _Time * _NoiseSpeed.z;   
         		if(noisePos.x > 0.0f)
         		{      		
         			input.vertex.z += (tex2Dlod(_NoiseTex, float4((noisePos.z - noisePos.z % _NoiseTexSize) / _NoiseTexSize, noisePos.z % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.z;
         		}
         		else
         		{
         			input.vertex.z -= (tex2Dlod(_NoiseTex, float4((noisePos.z - noisePos.z % _NoiseTexSize) / _NoiseTexSize, noisePos.z % _NoiseTexSize, 0, 0)) - 0.5f) * _Noise.z;
         		}
         		        		
         		
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
            	float newOpacity = pow(min(1.0, (abs(dot(viewDirection, normalDirection)) * _FogTint.a)), _TransparencyGrow);
            	
            	if (newOpacity < 0.01)
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
