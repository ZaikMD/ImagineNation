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


Shader "Production/Outline"
{
	//Properties that can be set by designers
	Properties
	{
		_GlowTint("Glow Tint", Color) = (0.1, 1.0, 0.0, 1.0)
		_GlowSize("Glow Size", Float) = 1.1
		_Offset("Offset of Origin", Vector) = (0.0, 0.0, 0.0)
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
				Pass IncrSat
				ZFail Keep
			}
			
			//Do not remove the colours behind the object
			Cull back
			ZWrite Off
					
			//Our blend equation is Additive
			Blend SrcAlpha SrcColor
					
			CGPROGRAM
					
			#pragma vertex vertShader
			#pragma fragment fragShader
					
			//Public Uniforms
			float4 _GlowTint;
			float _GlowSize;
			float3 _Offset;
			
			
			//What the vertex shader will recieve
			struct vertexInput
			{
				float4 pos : POSITION;
			};
			
			//What the fragment shader will recieve
			struct vertexOutput
			{
				float4 pos : POSITION;
			};
			
			//Vertex Shader
			vertexOutput vertShader(vertexInput input)
			{
				//A container for the vertexOutput
				vertexOutput output;
				
				//Enlarge the glow
				input.pos.xyz *= _GlowSize;
				input.pos.xyz += _Offset.xyz;
				
				//Calculate the vertex's position according to the camera
				output.pos = mul(UNITY_MATRIX_MVP, input.pos);
				
				//Return our output
				return output;
			}
			
			//Fragment Shader
			float4 fragShader (vertexOutput output) : COLOR
			{				
				//Return the colour of the first pass's fragment
				return _GlowTint;
			}
			
			ENDCG
		}
	}
}
