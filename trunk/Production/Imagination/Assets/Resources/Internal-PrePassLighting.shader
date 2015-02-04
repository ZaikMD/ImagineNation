//My version of a world shader
//
//
//Created by Jason Hein on Feb 4th, 2015

Shader "Hidden/Internal-PrePassLighting"
{
	Properties
	{
		_LightTexture0 ("", any) = "" {}
		_LightTextureB0 ("", 2D) = "" {}
		_ShadowMapTexture ("", any) = "" {}
		
	}
	SubShader
	{
		//We need the CG library
		CGINCLUDE
		#include "UnityCG.cginc"

		//What the vertex shader recieves from unity
		struct vertexInput
		{
			float4 pos : POSITION0;
			float3 norm : NORMAL0;
		};

		//What the vertex shader returns and the fragment shader recieves
		struct vertexOutput
		{
			float4 pos : POSITION0;
			float4 uv : TEXCOORD0;
			float3 ray : TEXCOORD1;
		};

		//Light as a quad
		float _LightAsQuad;

		//Vertex shader
		vertexOutput vert (vertexInput input)
		{
			vertexOutput output;
			
			//Provide unity the objects position
			output.pos = mul(UNITY_MATRIX_MVP, input.pos);
			
			//Provide unity the objects texture coordinates
			output.uv = ComputeScreenPos (output.pos);
			
			//Direction for specular light calculations
			output.ray = mul (UNITY_MATRIX_MV, input.pos).xyz * float3(-1,-1,1);
			output.ray = lerp(output.ray, input.norm, _LightAsQuad);
			
			//Send this information to the fragment shader
			return output;
		}

		//Normal mapping uniforms
		sampler2D _CameraNormalsTexture;
		sampler2D_float _CameraDepthTexture;

		//Unity Light uniforms
		float4 _LightDir;
		float4 _LightPos;
		float4 _LightColor;
		float4 unity_LightmapFade;
		float4x4 _LightMatrix0;
		sampler2D _LightTextureB0;

		//Camera to world
		CBUFFER_START(UnityPerCamera2)
		float4x4 _CameraToWorld;
		CBUFFER_END
		
		//Declare a light texture
		#if defined (POINT_COOKIE)
		samplerCUBE _LightTexture0;
		#else
		sampler2D _LightTexture0;
		#endif



		//Spotlight shadows
		#if defined (SHADOWS_DEPTH)
		#if defined (SPOT)
		
		//Decalre a shadowmap for sampling
		UNITY_DECLARE_SHADOWMAP(_ShadowMapTexture);
		
		//Sampling offsets for softend shadows
		#if defined (SHADOWS_SOFT)
		float4 _ShadowOffsets[4];
		#endif
		
		//Get a shadow
		half getSpotlightShadow (float4 aShadowUV, float fadeAMount)
		{
			//For softend shadows, it may be rescaled during the calculation (so we rescale it)
			#if defined (SHADOWS_SOFT)
			float3 shadowUV = aShadowUV.xyz / aShadowUV.w;
			
			//Get shadows from a buffer provided by shadow casters
			#if defined (SHADOWS_NATIVE)
			half4 shadows;
			shadows.x = UNITY_SAMPLE_SHADOW(_ShadowMapTexture, shadowUV + _ShadowOffsets[0]);
			shadows.y = UNITY_SAMPLE_SHADOW(_ShadowMapTexture, shadowUV + _ShadowOffsets[1]);
			shadows.z = UNITY_SAMPLE_SHADOW(_ShadowMapTexture, shadowUV + _ShadowOffsets[2]);
			shadows.w = UNITY_SAMPLE_SHADOW(_ShadowMapTexture, shadowUV + _ShadowOffsets[3]);	
			shadows = _LightShadowData.xxxx + shadows * (1.0 - _LightShadowData.xxxx);
			#endif
			
			//Get how much to shade the surface based on the shadow on the surface
			half shadow = dot( shadows, 0.25 );
			
			//All our shadows are soft, but just in case we'll set a shadow to nothing if we aren't using a soft shadow for some scene
			#else
			half shadow = 1.0;
			#endif
			
			//Return how much to shade the surface based on the shadow
			return saturate(shadow + fadeAMount);
		}
		#endif
		#endif

		//Point light shadows
		#if defined (SHADOWS_CUBE)
		#if defined (POINT) || defined (POINT_COOKIE)
		
		//The shadow map provided by shadow casters
		samplerCUBE _ShadowMapTexture;
		
		//Returns the distance from a shadow
		float getDistanceFromShadow (float3 lightPosition)
		{
			//Sampled a shade from a shadow texture
			float4 shade = texCUBE (_ShadowMapTexture, lightPosition);
			
			//Returns the distance from a shadow
			return DecodeFloatRGBA( shade );
		}
		
		//Returns a shadow for a surface lit by a point light
		half getPointLightShadow (float3 toLight, float fadeDistance)
		{
			//For soft shadows
			#if defined (SHADOWS_SOFT)
			
			//Changes the size of shadows cast by objects
			//Changes how far an object must be to cast a shadow
			half samplingDistance = 1.0 / 128.0;
			
			//If the caster of the shadow is close enough to cast a shadhow, return a shade
			if (getDistanceFromShadow (toLight + float3( samplingDistance,  samplingDistance,  samplingDistance)) < fadeDistance &&
				getDistanceFromShadow (toLight + float3(-samplingDistance, -samplingDistance,  samplingDistance)) < fadeDistance &&
				getDistanceFromShadow (toLight + float3(-samplingDistance,  samplingDistance, -samplingDistance)) < fadeDistance &&
				getDistanceFromShadow (toLight + float3( samplingDistance, -samplingDistance, -samplingDistance)) < fadeDistance)
			{
				//Return how much to shade the surface based on the shadow on the surface
				return _LightShadowData.xxxx * 0.25;
			}
			//Provide a full shadow
			else
			{
				return 1.0;
			}	
			
			//We are using soft shadows, but just in case we should return a value for scenes that don't
			#else
			return 1.0;
			#endif
		}
		#endif
		#endif
		
		
		//Directional light shadows
		#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
		#if defined(SHADOWS_SCREEN)
		
		//Directional light shadow texture for sampling
		sampler2D _ShadowMapTexture;
		
		//Returns a normalized shade value for a directiona light shadow
		half getDirectionalLightShadow (float2 uv, float fadeAmount)
		{
			return saturate(tex2D (_ShadowMapTexture, uv).r + fadeAmount);
		}
		#endif
		#endif





		//Returns how much to shade a surface based on the shadows casted on it
		half getShadow(float3 toLight, float fadeDistance, float2 uv)
		{
			//Calculate how much to fade the shadow
			#if defined(SHADOWS_DEPTH) || defined(SHADOWS_SCREEN) || defined(SHADOWS_CUBE)
			float fade = saturate(fadeDistance * _LightShadowData.z + _LightShadowData.w);
			#endif
			
			//Shading for spot lights
			#if defined(SPOT)
			#if defined(SHADOWS_DEPTH)
			
			//Make a new shadow UV with a z
			float4 shadowUV = mul (unity_World2Shadow[0], float4(toLight, 1));
			
			//Returns shade for a spot light shadow
			return getSpotlightShadow (shadowUV, fade);
			#endif
			#endif
			
			//Shading for direction lights
			#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
			#if defined(SHADOWS_SCREEN)
			return getDirectionalLightShadow(uv, fade);
			#endif
			#endif
			
			//Shading for point lights
			#if defined (POINT) || defined (POINT_COOKIE)
			#if defined(SHADOWS_CUBE)
			return getPointLightShadow (toLight, (length(toLight) * _LightPositionRange.w) * 0.97);	
			#endif
			#endif
			
			//If no shadows are defined, return no shadows
			return 1.0;
		}

		//Returns a color based on the surfaces color and lighting
		half4 getLightingColor (vertexOutput output)
		{
			//				Camera far plane / ray z
			output.ray *= _ProjectionParams.z / output.ray.z;
			float2 uv = output.uv.xy / output.uv.w;
			
			//Get the normal of the fragment
			half4 specularNormal = tex2D (_CameraNormalsTexture, uv);
			
			//Get the normal of the fragment
			half3 normal = normalize(specularNormal.rgb * 2.0 - 1.0);
			
			//Get the depth of this fragment from the depth buffer
			float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
			depth = Linear01Depth (depth);
			
			//Positions
			float4 vpos = float4(output.ray * depth, 1.0);
			float3 wpos = mul (_CameraToWorld, vpos).xyz;
			
			//Get the distance to check if the shadow will fade
			float fadeDistance = lerp(vpos.z, distance(wpos, unity_ShadowFadeCenterAndType.xyz), unity_ShadowFadeCenterAndType.w);
			
			//If their are spot lights
			#if defined (SPOT)
			
			//Get the direction to the light
			float3 tolight = _LightPos.xyz - wpos;
			half3 lightDirection = normalize (tolight);
			
			//
			float4 uvCookie = mul (_LightMatrix0, float4(wpos, 1.0));
			float atten = tex2Dproj (_LightTexture0, UNITY_PROJ_COORD(uvCookie)).w;
			atten *= uvCookie.w < 0.0;
			float att = dot(tolight, tolight) * _LightPos.w;
			atten *= tex2D (_LightTextureB0, att.rr).UNITY_ATTEN_CHANNEL;
			
			//Apply spot light shadows
			atten *= getShadow (wpos, fadeDistance, uv);
			#endif
			
			//
			#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
			
			//Get the direction to the light
			half3 lightDirection = -_LightDir.xyz;
			
			//Get directional light shadows
			float atten = getShadow (wpos, fadeDistance, uv);
			
			//
			#if defined (DIRECTIONAL_COOKIE)
			atten *= tex2D (_LightTexture0, mul(_LightMatrix0, half4(wpos, 1.0)).xy).w;
			#endif
			#endif
			
			//
			#if defined (POINT) || defined (POINT_COOKIE)
			
			//Get the direction to the light
			float3 tolight = wpos - _LightPos.xyz;
			half3 lightDirection = -normalize (tolight);
			
			//
			float att = dot(tolight, tolight) * _LightPos.w;
			float atten = tex2D (_LightTextureB0, att.rr).UNITY_ATTEN_CHANNEL;
			
			//Apply point light shadows
			atten *= getShadow (tolight, fadeDistance, uv);
			
			//
			#if defined (POINT_COOKIE)
			atten *= texCUBE(_LightTexture0, mul(_LightMatrix0, half4(wpos, 1.0)).xyz).w;
			#endif
			#endif
			
			//Diffuse lighting
			half diffuseShade = max (0.0, dot (lightDirection, normal));
			half3 h = normalize (lightDirection - normalize(wpos-_WorldSpaceCameraPos));
			
			//Specular lighting
			float specularLuminance = pow (max (0.0, dot(h, normal)), specularNormal.a * 128.0);
			specularLuminance *= saturate(atten);
			
			//Make the final color of the fragment
			half4 finalColor;
			finalColor.xyz = _LightColor.rgb * (diffuseShade * atten);
			finalColor.w = specularLuminance * Luminance (_LightColor.rgb);
			
			//
			float fade = fadeDistance * unity_LightmapFade.z + unity_LightmapFade.w;
			finalColor *= saturate(1.0 - fade);
			
			//Return the final color of the fragment
			return finalColor;
		}
		ENDCG

		//Pass for base lighting
		Pass
		{
			//Don't write to the z buffer
			ZWrite Off
			
			//Turn fog off
			Fog { Mode Off }
			
			//This will be the only color in the fragment
			Blend DstColor Zero
			
			//This is CG
			CGPROGRAM
			
			//Target v3.0
			#pragma target 3.0
			
			//Define shaders
			#pragma vertex vert
			#pragma fragment fragShader
			
			//
			#pragma exclude_renderers noprepass
			#pragma glsl_no_auto_normalization
			#pragma multi_compile_lightpass

			//Fragment shader
			float4 fragShader (vertexOutput output) : SV_Target
			{
				return exp2(-getLightingColor(output));
			}

			ENDCG
		}

		//Base lighting pass
		Pass
		{
			//Don't write to the z buffer
			ZWrite Off
			
			//Turn fog off
			Fog { Mode Off }
			
			//Add to the current color
			Blend One One
			
			//This is CG
			CGPROGRAM
			
			//Target v3.0
			#pragma target 3.0
			
			//Define shaders
			#pragma vertex vert
			#pragma fragment fragShader
			
			//
			#pragma exclude_renderers noprepass
			#pragma glsl_no_auto_normalization
			#pragma multi_compile_lightpass

			//Fragment shader
			float4 fragShader (vertexOutput output) : SV_Target
			{
				return getLightingColor(output);
			}

			ENDCG
		}

		//Specular light Pass
		Pass
		{
			//Don't write to the z buffer
			ZWrite Off
			
			//Turn fog off
			Fog { Mode Off }
			
			//Add to the current color
			Blend One One
			
			//This is CG
			CGPROGRAM
			
			//Target v3.0
			#pragma target 3.0
			
			//Define shaders
			#pragma vertex vert
			#pragma fragment fragShader
			
			//
			#pragma exclude_renderers noprepass
			#pragma glsl_no_auto_normalization
			#pragma multi_compile_lightpass

			//Fragment shader
			float4 fragShader (vertexOutput output) : SV_Target
			{
				return getLightingColor(output).argb;
			}

			ENDCG
		}
	}
	
	//No shader passes after this one
	Fallback Off
}