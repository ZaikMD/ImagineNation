// TO USE
//
// 1. Make sure you have set the ambient lighting. Edit -> Render Settings -> Ambient Lighting
// 2. Create a material
// 3. Place the material on the object
// 4. Drag a texture to the "_Texture" box
// 5. You are done.
//
// Created by Jason Hein

Shader "Production/Diffuse_Specular"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Gloss ("Gloss (0-1)", Float) = 1.0
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		//This is in CG
		CGPROGRAM
		#pragma surface surf Diffuse_Specular_Shader

		//Texture of our surface
		sampler2D _MainTex;
		
		//Glossyness of this surface
		float _Gloss;

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
  			output.Specular = _Gloss;
		}
		
		//Specular lighting
		half4 LightingDiffuse_Specular_Shader (SurfaceOutput output, half3 lightDirection, half3 viewDirection, half attenuation)
		{
			//Direction for detemined specular luminousity
	        half3 specularDirection = normalize (viewDirection + lightDirection);

			//Loss of intensity from diffuse lighting
	        half diffuseAttenuation = max (0, dot (output.Normal, lightDirection));
	        float specularLuminosity = max (0, dot (output.Normal, specularDirection));
			
			//Return the base fragment colo and a specular color
	        return half4 ((output.Albedo * _LightColor0.rgb * diffuseAttenuation + _LightColor0.rgb * specularLuminosity) * (attenuation * 2.0), output.Alpha);
	    }
	    
	    //Calls the internal pre_pass shader and then multiplies our texture color by the color returned
		float4 LightingDiffuse_Specular_Shader_PrePass(SurfaceOutput output, float4 light)
		{
    		return half4(	(output.Albedo + light.a) * light.rgb,
    						 output.Alpha + light.a * _SpecColor.a);
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}

/*Shader "Production/Diffuse_Specular"
{
	//Properties that can be set by designers
	Properties
    {
    	_MainTex ("Texture", 2D) = "white" {} 
    	_SpecColor ("Specular Light Color", Color) = (0.3,0.3,0.3,1.0) 
    	_Shininess ("Shininess", Float) = 10.0
    	_PointLightIllumination("Point Light Illumination", Float) = 10.0
    	_PointLightMaximumIllumination("Point Light Max Illumination", Float) = 0.35
    }
    
    //Shader
	SubShader
	{
		//Pass for directional and ambient lighting
		Pass 
		{
			Tags { "LightMode" = "ForwardBase" } 
			
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
         	
         	//Vertex lights
         	float _PointLightIllumination;
         	float _PointLightMaximumIllumination;
         	
         	//What the vertex shader will recieve
         	struct vertInput
         	{
            	float4 pos : POSITION0;
            	float3 normal : NORMAL;
            	half2 uv : TEXCOORD0;
       		};
       		
       		//What the fragment shader will recieve
         	struct vertOutput
         	{
            	float4 pos : POSITION0;
            	float4 posWorld : POSITION1;
            	float3 normalDir : POSITION2;
            	half2 uv : TEXCOORD0;
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
              	 	float distShading = 1.0 / pow(vertexToLightSource, 2) * _PointLightIllumination;
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
         	sampler2D _MainTex;
         	float4 _MainTex_ST;
         	float4 _SpecColor;
         	float _Shininess;
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
            	
            	//Specular reflection
            	float3 specularReflection;
            	
            	//If we are facing the light, so calculate specular lighting
            	if (_SpecColor.a > 0.0 && dot(normalDirection, lightDirection) > 0.0)
            	{
            		specularReflection = _LightColor0.xyz * _SpecColor.xyz * _SpecColor.a * distShading * textureColor.a *
            		pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
            	}
            	
            	//Otherwise their is no specular lighting
            	else
            	{
            		specularReflection = float3(0.0, 0.0, 0.0);
            	}

         		//Return the final colour of the fragment
         		return float4((diffuseLighting * textureColor.xyz) + specularReflection, 1.0);
         	}
         	
         	//End the cg shader
 			ENDCG
		}
		
		
		// Pass to render object as a shadow caster
		Pass 
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			
			Fog {Mode Off}
			ZWrite On ZTest LEqual Cull Off
			Offset 1, 1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f { 
				V2F_SHADOW_CASTER;
			};

			v2f vert( appdata_base v )
			{
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				return o;
			}

			float4 frag( v2f i ) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
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
}*/