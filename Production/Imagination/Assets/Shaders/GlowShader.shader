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
		_GlowSize("Glow Size", Float) = 1.2
		_GlowShowsDistance("Distance that Glow Begins to Show", Float) = 10.0
		_TransparencyGrow("Transparency Grow", Float) = 4.0
	}
	Subshader
	{
		//Pass for ignoring the original object's model
		Pass
		{
			Tags { "Queue" = "Transparent" } 
			Stencil
			{
                Ref 1
                Comp always
                Pass replace
                ZFail Keep
            }
			
			//Do not remove the colours behind the object
			Cull back
			ZWrite Off
			
			//Our blend equation is additive
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
			Stencil
			{
                Ref 1
                Comp NotEqual
                Pass keep
                ZFail Keep
            }
			
			//Do not remove the colours behind the object
			Cull back
			ZWrite Off
			
			//Our blend equation is Additive
         	Blend SrcAlpha One
         	
         	CGPROGRAM
 
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	
         	//Public Uniforms
         	float4 _GlowTint;
         	float _GlowSize;
         	float _GlowShowsDistance;
         	float _TransparencyGrow;
         	
         	
         	//What the vertex shader will recieve
         	struct vertexInput
         	{
         		float4 pos : POSITION;
         		float3 normal : NORMAL;
         	};
         	
         	//What the fragment shader will recieve
         	struct vertexOutput
         	{
         		float4 pos : POSITION;
         		half distanceFromCamera : POSITION1;
            	float3 normal : TEXCOORD0;
            	float3 viewDir : TEXCOORD1;
         	};
         	
         	//Vertex Shader
         	vertexOutput vertShader(vertexInput input)
         	{
         		//A container for the vertexOutput
         		vertexOutput output;
         		
         		//Enlarge the glow
         		input.pos.xyz *= _GlowSize;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Calc a float to modify the opacity based on the distance from the camera
         		output.distanceFromCamera = pow(_GlowShowsDistance / distance(mul(_Object2World, input.pos), _WorldSpaceCameraPos), 2.0);
         		if (output.distanceFromCamera > 1.0)
 				{
 					output.distanceFromCamera = 1.0;
 				}
         		
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
 				
 				//Calculate a dot product to make the opacity grow out from the object
 				float dotOfAngle = dot(viewDirection, normalDirection);
 				if (dotOfAngle < 0.01)
 				{
 					discard;
 				}
 				else if (dotOfAngle > 1.0)
 				{
 					dotOfAngle = 1.0;
 				}
 				
 				//Calculate a new opacity for faces that are facing tangent to the cameras view direction
            	float newOpacity = min(_GlowTint.a, pow(dotOfAngle, _TransparencyGrow) * _GlowTint.a * output.distanceFromCamera);
            	if (newOpacity < 0.01)
            	{
            		discard;
            	}
            	
            	//Return the colour of the first pass's fragment
            	return float4 (_GlowTint.xyz, newOpacity);
         	}
         	
         	
         	ENDCG
         }
	}
}
