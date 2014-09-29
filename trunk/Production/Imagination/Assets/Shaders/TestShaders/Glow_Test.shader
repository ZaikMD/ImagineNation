// TO USE
//
// 1. Create a glow material (unless one has been made)
// 2. Go to the objects mesh renderer component
// 3. Open Materials
// 4. Increase the size of the materials array by 1
// 5. You can now attach a second material to the object
// 6. Put the material on the object
// 7. Drag a texture to the "_Texture" box
// 8. You are done
//
// Created by Jason Hein



Shader "Production/GlowShader"
{
	//Properties that can be set by designers
	Properties
	{
		_GlowTint("Glow Tint", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	Subshader
	{
		//Pass for drawing the glow
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
         	float4 _GlowTint;
         	
         	
         	//What the vertex shader will recieve
         	struct vertexInput
         	{
         		float4 vertex : POSITION;
         		float3 normal : NORMAL;
         	};
         	
         	//What the fragment shader willl recieve
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
         		
         		//Calculate the objects distance from the camera
         		float distanceModifier = 1.5 / pow(length(mul(UNITY_MATRIX_MVP, input.vertex).xyz), 0.2);
         		
         		//Enlarge the glow
         		input.vertex.xz *= 1.2f * distanceModifier;
         		input.vertex.y *= 1.1f * distanceModifier;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
         		
         		//Calculate the normal of the surface in object coordinates
         		output.normal = normalize(mul(float4(input.normal, 0.0), _World2Object).xyz);
         		
         		//Calculate view direction, for dot calculations in the fragment shader
         		output.viewDir = normalize(_WorldSpaceCameraPos - mul(_Object2World, input.vertex).xyz);
         		
         		
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
            	float newOpacity = pow(min(1.0, (dot(viewDirection, normalDirection)) * _GlowTint.a), 2.0);
            	
            	if (newOpacity < 0.05)
            	{
            		discard;
            	}
            	
            	//Calculate the colour of this fragment
            	float4 fragmentColour = float4 (_GlowTint.xyz, newOpacity);
            	
            	
            	//Return the colour of the first pass's fragment
            	return fragmentColour;
         	}
         	
         	
         	ENDCG
         }
	}
}
