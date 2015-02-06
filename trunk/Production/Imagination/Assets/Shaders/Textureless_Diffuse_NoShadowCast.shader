// TO USE
//
// 1. Make sure you have set the ambient lighting. Edit -> Render Settings -> Ambient Lighting
// 2. Create a material
// 3. Place the material on the object
// 4. Drag a texture to the "_Texture" box
// 5. You are done.
//
// Created by Jason Hein

Shader "Production/Textureless_Diffuse_NoShadowCast"
{
	//Properties that can be set by designers
	Properties
    {
    	_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    
    //Shader
	SubShader
	{
		Tags {"RenderType" = "Opaque"}
	
		//Pass for directional and ambient lighting
		Pass 
		{
			Tags { "LightMode" = "ForwardBase" } 
			
			//This is a CG shader
			CGPROGRAM
			
			//Allows us to get ambient lighting
			#include "UnityCG.cginc"
 			
 			//Define the shaders
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//Public Uniforms
         	float4 _Color;
         	
         	//What the vertex shader will recieve
         	struct vertInput
         	{
            	float4 pos : POSITION0;
       		};
       		
       		//What the fragment shader will recieve
         	struct vertOutput
         	{
            	float4 pos : POSITION0;
        	};
         	
         	//Vertex Shader
         	vertOutput vertShader(vertInput input)
         	{
         		//A container for the vertexOutput
         		vertOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Return our output
         		return output;
         	}
         	
         	//Fragment Shader
         	float4 fragShader(vertOutput output) : COLOR
         	{
         		//Return the final colour of the fragment
         		return _Color;
         	}
         	
 			//End the cg shader
 			ENDCG
		}
	}
}