// TO USE
//
// 1. Create a glow material (unless one has been made)
// 2. Go to the objects mesh renderer component
// 3. Open Materials
// 4. Increase the size of the materials array by 1
// 5. You can now attach a second material to the object
// 6. Put the material on the object
// 7. Set the opacity of the "Glow Tint" to the maximum opacity of the glow
// 8. You are done
//
// Created by Jason Hein



Shader "Production/GlowShader"
{
	//Properties that can be set by designers
	Properties
	{
		_GlowTint("Glow Tint", Color) = (0.1, 1.0, 0.0, 1.0)
		_GlowSize("Glow Size", Float) = 1.1
		_GlowShowsDistance("Distance that Glow Begins to Show", Float) = 15.0
	}
	Subshader
	{
		//Pass for ignoring the original object's model
		Pass
		{
			Tags { "Queue" = "Transparent" } 
			Stencil {
                Ref 1
                Comp always
                Pass replace
                ZFail Keep
            }
			
			//Do not remove the colours behind the object
			Cull back
			ZWrite Off
			
			//Our blend equation is multiplicative
         	Blend One One
         	
         	CGPROGRAM
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//Vertex Shader
         	float4 vertShader(float4 pos : POSITION) : POSITION
         	{
         		//Return our output
         		return mul(UNITY_MATRIX_MVP, pos);
         	}
         	
         	//Fragment Shader
         	float4 fragShader () : COLOR
         	{
            	//Return the colour of the first pass's fragment
            	return float4(0.0,0.0,0.0,0.0);
         	}
         	
         	
         	ENDCG
		}
		
		//Pass for drawing the glow
		Pass
		{
			Tags { "Queue" = "Transparent" }
			Stencil {
                Ref 1
                Comp NotEqual
                Pass keep
                ZFail Keep
            }
			
			//Do not remove the colours behind the object
			Cull back
			ZWrite Off
			
			//Our blend equation is multiplicative
         	Blend SrcAlpha One
         	
         	CGPROGRAM
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	
         	//Public Uniforms
         	float4 _GlowTint;
         	float _GlowSize;
         	float _GlowShowsDistance;
         	
         	
         	//What the vertex shader will recieve
         	struct vertexInput
         	{
         		float4 vertex : POSITION;
         		float3 normal : NORMAL;
         	};
         	
         	//What the fragment shader willl recieve
         	struct vertexOutput
         	{
         		float4 pos : POSITION;
         		float4 worldPos : POSITION1;
            	float3 normal : TEXCOORD0;
            	float3 viewDir : TEXCOORD1;
         	};
         	
         	//Vertex Shader
         	vertexOutput vertShader(vertexInput input)
         	{
         		//A container for the vertexOutput
         		vertexOutput output;
         		
         		//Enlarge the glow
         		input.vertex.xz *= pow(_GlowSize, 2);
         		input.vertex.y *= _GlowSize;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
         		
         		//Calculate the vertex's position in world space
         		output.worldPos = mul(_Object2World, input.vertex);
         		
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
            	float newOpacity = min(_GlowTint.a, pow(dot(viewDirection, normalDirection), 2.0) * _GlowTint.a * _GlowShowsDistance / distance(output.worldPos.xyz, _WorldSpaceCameraPos));
            	
            	if (newOpacity < 0.03)
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
