//My version of an internal pre-pass shader shader
//
//
//Created by Jason Hein on Jan 27th, 2015

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
			
			//Vector for where the fragment's position is
			output.ray = mul (UNITY_MATRIX_MV, input.pos).xyz * float3(-1,-1,1);
			output.ray = lerp(output.ray, input.norm, _LightAsQuad);
			
			//Send this information to the fragment shader
			return output;
		}

		//Normal mapping
		sampler2D _CameraNormalsTexture;
		
		//Depth texture
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
		half getSpotlightShadow (float4 aShadowUV, float fadeDistance)
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
			
			//Get how much to shade the surface based on the shadow on the surface
			half shadow = dot( shadows, 0.25 );
			
			//Otherwise provide no shadow
			#else
			half shadow = 1.0;
			#endif
			
			//All our shadows are soft, but just in case we'll set a shadow for lower end pc's
			#else
			
			//Get shadows from a buffer
			#if defined (SHADOWS_NATIVE)
			half shadow = UNITY_SAMPLE_SHADOW_PROJ(_ShadowMapTexture, aShadowUV) * (1.0 - _LightShadowData.x) + _LightShadowData.x;
			
			//Otherwise provide no shadow
			#else
			half shadow = 1.0;
			#endif
			#endif
			
			//Calculate fading based on the distance from a shadow
			float fade = saturate(fadeDistance * _LightShadowData.z + _LightShadowData.w);
			
			//Return how much to shade the surface based on the shadow
			return saturate(shadow + fade);
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
			//Provide no shadow
			else
			{
				return 1.0;
			}	
			
			//If their are hard shadows
			#else
			
			//If the distance from the shadow is less then the fade distance, return a full shadow
			if (getDistanceFromShadow (toLight) < fadeDistance)
			{
				return _LightShadowData.r;
			}
			//Provide no shadow
			else
			{
				return 1.0;
			}
			#endif
		}
		#endif
		#endif
		
		
		//Directional light shadows
		#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
		#if defined(SHADOWS_SCREEN)
		
		//Directional light shadow texture for sampling
		sampler2D _ShadowMapTexture;
		
		//Returns a normalized shade value for a directional light shadow
		half getDirectionalLightShadow (float2 uv, float fadeDistance)
		{
			//Calculate fading based on the distance from a shadow
			float fade = saturate(fadeDistance * _LightShadowData.z + _LightShadowData.w);
		
			//Return a shadow shade
			return saturate(tex2D (_ShadowMapTexture, uv).x + fade);
		}
		#endif
		#endif
		ENDCG


		//One Pass for the lighting
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
			
			//Don't render any other pre-pass shader
			#pragma exclude_renderers noprepass
			
			//Don't automatically normalize normals
			#pragma glsl_no_auto_normalization
			
			//Compile for each type of light source
			#pragma multi_compile_lightpass

			//Fragment shader
			float4 fragShader (vertexOutput output) : SV_Target
			{
				//Bring ray into mvp space
				output.ray *= _ProjectionParams.z / output.ray.z;
				float2 uv = output.uv.xy / output.uv.w;
				
				//Get the normal of the fragment
				half4 textureNormal = tex2D (_CameraNormalsTexture, uv);
				
				//Get the normal of the fragment
				half3 normal = normalize(textureNormal.rgb * 2.0 - 1.0);
				
				//Get the depth of this fragment from the depth buffer
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
				depth = Linear01Depth (depth);
				
				//MVP position of the fragment
				float4 viewPosition = float4(output.ray * depth, 1.0);
				
				//World position of the fragment
				float3 worldPosition = mul (_CameraToWorld, viewPosition).xyz;
				
				//Get the distance to check if the shadow will fade
				float fadeDistance = lerp(viewPosition.z, distance(worldPosition, unity_ShadowFadeCenterAndType.xyz), unity_ShadowFadeCenterAndType.w);
				
				//If any type of light is defined
				#if defined (SPOT) ||  defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE) || defined (POINT) || defined (POINT_COOKIE)
				
				//If we are a spot light
				#if defined (SPOT)
				
				//Get the direction to the light
				float3 toLight = _LightPos.xyz - worldPosition;
				half3 lightDirection = normalize (toLight);
				
				//
				float4 lightUV = mul (_LightMatrix0, float4(worldPosition, 1.0));
				float attenuation = tex2Dproj (_LightTexture0, UNITY_PROJ_COORD(lightUV)).w;
				attenuation *= lightUV.w < 0;
				float att = dot(toLight, toLight) * _LightPos.w;
				attenuation *= tex2D (_LightTextureB0, att.rr).UNITY_ATTEN_CHANNEL;
				
				//If their are spot light shadows
				#if defined(SHADOWS_DEPTH)
				
				//Make a new shadow UV with a z
				float4 shadowUV = mul (unity_World2Shadow[0], float4(toLight, 1));
				
				//Apply a shade for a spot light shadow
				attenuation *= getSpotlightShadow (shadowUV, fadeDistance);
				#endif
				#endif
				
				
				//If we are a directional light
				#if defined (DIRECTIONAL) || defined (DIRECTIONAL_COOKIE)
				
				//Get the direction to the light
				half3 lightDirection = -_LightDir.xyz;
				
				//If their are directiona light shadows
				#if defined(SHADOWS_SCREEN)
				
				//Apply directional light shadows
				float attenuation =  getDirectionalLightShadow(uv, fadeDistance);
				
				//Otherwise their are no shadows
				#else
				float attenuation =  1.0;
				#endif
				#endif
				
				
				//If we are a point light
				#if defined (POINT) || defined (POINT_COOKIE)
				
				//Get the direction to the light
				float3 toLight = worldPosition - _LightPos.xyz;
				half3 lightDirection = -normalize (toLight);
				
				//Reduced intensity from being farther from the light
				float att = dot(toLight, toLight) * _LightPos.w * 1.0;
				
				//Intensity of the light on this surfface
				float attenuation = tex2D (_LightTextureB0, att.rr).UNITY_ATTEN_CHANNEL;
				
				//If their are point light shadows
				#if defined(SHADOWS_CUBE)
				
				//Apply point light shadows
				attenuation *= getPointLightShadow (toLight, (length(toLight) * _LightPositionRange.w) * 0.97);
				#endif
				
				//Reduce bright spots
				if (attenuation > 0.5)
				{
					float lessAtten = attenuation - 0.5;
					attenuation -= lessAtten;
					lessAtten *= 0.25;
					attenuation += lessAtten;
					if (attenuation > 0.5)
					{
						attenuation = 0.5;
					}
				}
				#endif
				
				
				//If no types of light are defined, provide values so we do not get an error
				#else
				half3 lightDirection = float3 (1.0, 1.0, 1.0);
				float attenuation = 1.0;
				#endif
				
				//Diffuse lighting
				half diffuseShade = max (0.0, dot (lightDirection, normal));
				
				//Specular lighting
				half3 specularDirection = normalize (lightDirection - normalize(worldPosition - _WorldSpaceCameraPos));
				float specularLuminance = pow (max (0.0, dot(specularDirection, normal)), textureNormal.a * 128.0);
				specularLuminance *= saturate(attenuation);
				
				//Make the final color of the fragment
				half4 finalColor;
				finalColor.xyz = _LightColor.rgb * diffuseShade * attenuation;
				finalColor.w = specularLuminance * Luminance (_LightColor.rgb);
				
				//Fade from how far away the light is
				float fade = fadeDistance * unity_LightmapFade.z + unity_LightmapFade.w;
				finalColor *= saturate(1.0 - fade);
				
				//Return the final color of the fragment
				return exp2(-finalColor);
			}

			ENDCG
		}
	}
	
	//No shader passes after this one
	Fallback Off
}