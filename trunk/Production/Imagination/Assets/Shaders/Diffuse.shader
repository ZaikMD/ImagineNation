﻿// TO USE
//
// 1. Make sure you have set the ambient lighting. Edit -> Render Settings -> Ambient Lighting
// 2. Create a material
// 3. Place the material on the object
// 4. Drag a texture to the "_Texture" box
// 5. You are done.
//
// Created by Jason Hein

Shader "Production/Diffuse"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		//This is in CG
		CGPROGRAM
		#pragma surface surf Diffuse_Shader

		//Texture of our surface
		sampler2D _MainTex;

		//What our vertex shader recieves
		struct Input
		{
			float2 uv_MainTex;
		};
		
		//Sets values for the internal pre_pass shader
		void surf (Input IN, inout SurfaceOutput output)
		{
			//Get a texture color at our UV
			output.Albedo = tex2D (_MainTex, IN.uv_MainTex);
		}
		
		//Calls the internal pre_pass shader and then multiplies our texture color by the color returned
		float4 LightingDiffuse_Shader_PrePass(SurfaceOutput output, float4 light)
		{
			return float4(output.Albedo * light.rgb, 1.0);
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}

/*Shader "Production/Diffuse"
{
	//Properties that can be set by designers
	Properties
    {
    	_MainTex ("Texture", 2D) = "white" {} 
    	_PointLightIllumination("Point Light Illumination", Float) = 10.0
    	_PointLightMaximumIllumination("Point Light Max Illumination", Float) = 0.35
    }
    
    //Shader
	SubShader
	{
		Tags {"RenderType" = "Opaque"}
	
		//Pass for directional and ambient lighting
		Pass 
		{
			//Tags { "LightMode" = "ForwardBase" } 
			
			//This is a CG shader
			CGPROGRAM
			
			//Allows us to get ambient lighting
			#include "UnityCG.cginc"
			
			//Shadows
			#include "AutoLight.cginc"
			#pragma multi_compile_fwdbase
 			
 			//Define the shaders
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//World Light Colour (from "UnityCG.cginc")
         	float4 _LightColor0;
         	
         	//Public Uniforms
         	sampler2D _MainTex;
         	float4 _MainTex_ST;
         	
         	//For vertex point lights
         	float _PointLightIllumination;
         	float _PointLightMaximumIllumination;
         	
         	//What the vertex shader will recieve
         	struct vertInput
         	{
            	float4 pos : POSITION0;
            	float3 normal : NORMAL;
            	float2 uv : TEXCOORD0;
       		};
       		
       		//What the fragment shader will recieve
         	struct vertOutput
         	{
            	float4 pos : POSITION0;
            	float4 posWorld : POSITION1;
            	float3 normalDir : POSITION2;
            	float2 uv : TEXCOORD0;
            	float3 vertexLighting : TEXCOORD1;
            	LIGHTING_COORDS(2,3)
        	};
         	
         	//Vertex Shader
         	vertOutput vertShader(vertInput input)
         	{
         		//A container for the vertexOutput
         		vertOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Calculate the real world position of the vertex, for later calculations
         		output.posWorld = mul(_Object2World, input.pos);
         		
         		//Calculate the direction of our surface normal
         		output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Give output the texture colour
         		output.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
         		
         		//Additional Lighting (vertex lights)
         		output.vertexLighting = float3 (0.0, 0.0, 0.0);
         		#ifdef VERTEXLIGHT_ON
            	for (int index = 0; index < 3; index++)
            	{    
            		//Get the vertex light position from unity
               		float3 lightPosition = float3(unity_4LightPosX0[index],  unity_4LightPosY0[index], unity_4LightPosZ0[index]);
               		
               		//Shading calculations
               		float3 vertexToLightSource = lightPosition - output.posWorld.xyz; 
              	 	float distShading = 1.0 / pow(length(vertexToLightSource), 2) * _PointLightIllumination;
              	 	if (distShading > _PointLightMaximumIllumination)
            		{
            			distShading = _PointLightMaximumIllumination;
            		}
              	 	
              	 	//Add this light to our vertex light
               		output.vertexLighting += distShading * unity_LightColor[index].rgb * max(0.0, dot(output.normalDir, normalize(vertexToLightSource))) / 2.0;         
            	}
           	 	#endif
           	 	
           	 	//Transfer the shadow to the fragment shadow
         		TRANSFER_VERTEX_TO_FRAGMENT(output);
         		
         		//Return our output
         		return output;
         	}
         	
         	//Fragment Shader
         	float4 fragShader(vertOutput output) : COLOR
         	{
         		//All our values are interpolated, so now we can do per-pixel calculations
         		
         		//Re-normalize direction values so they are correct
         		float3 normalDirection = normalize(output.normalDir);
            	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - output.posWorld.xyz);
            	
            	//Direction of our light, for dot product calculations
            	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
 				
 				//Base colour of this fragment
            	float4 textureColor = tex2D(_MainTex, output.uv);
            	
            	//Calculate the colour of the fragments diffuse lighting
            	float3 diffuseLighting = _LightColor0.xyz * LIGHT_ATTENUATION(output) * max(0.0, dot(normalDirection, lightDirection));

         		//Return the final colour of the fragment
         		return float4((UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseLighting + output.vertexLighting) * textureColor.xyz, 1.0);
         	}
         	
 			//End the cg shader
 			ENDCG
		}
		
		//Pass for additional lighting
		Pass
		{
			//Tags { "LightMode" = "ForwardAdd" }
			
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
         	sampler2D _MainTex;
         	float4 _MainTex_ST;
         	float _PointLightIllumination;
         	float _PointLightMaximumIllumination;
         	
         	//What the vertex shader will recieve
         	struct vertInput
         	{
            	float4 pos : POSITION0;
            	float3 normal : NORMAL;
            	half2 uv : TEXCOORD0;
       		};
       		
       		//What the fragment shader willl recieve
         	struct vertOutput
         	{
            	float4 pos : POSITION0;
            	float4 posWorld : POSITION1;
            	float3 normalDir : TEXCOORD0;
            	half2 uv : TEXCOORD1;
        	};
        	
        	//Vertex Shader
         	vertOutput vertShader(vertInput input)
         	{
         		//A container for the vertexOutput
         		vertOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Calculate the real world position of the vertex, for later calculations
         		output.posWorld = mul(_Object2World, input.pos);
         		
         		//Calculate the direction of our surface normal
         		output.normalDir = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Give output the texture colour
         		output.uv = input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
         		
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
            	float4 textureColor = tex2D(_MainTex, output.uv);
            	
            	//Calculate shading based off our distance from the lights
            	float distShading = 1.0 / pow(length(_WorldSpaceLightPos0.xyz - output.posWorld.xyz), 2.0) * _PointLightIllumination;
            	if (distShading > _PointLightMaximumIllumination)
            	{
            		distShading = _PointLightMaximumIllumination;
            	}
            	
            	//Calculate the colour of the fragments diffuse lighting
            	float3 diffuseLighting = _LightColor0.xyz * distShading * max(0.0, dot(normalDirection, lightDirection));

         		//Return the final colour of the fragment
         		return float4(diffuseLighting * textureColor.xyz, 1.0);
         	}
         	
         	//End the cg shader
 			ENDCG
		}
		
		// Pass to render object as a shadow collector
		// note: editor needs this pass as it has a collector pass.
		Pass
		{
			Name "ShadowCollector"
			Tags { "LightMode" = "ShadowCollector" }
			
			Fog {Mode Off}
			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcollector

			#define SHADOW_COLLECTOR_PASS
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};

			struct v2f {
				V2F_SHADOW_COLLECTOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				TRANSFER_SHADOW_COLLECTOR(o)
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				SHADOW_COLLECTOR_FRAGMENT(i)
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}*/