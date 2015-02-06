// TO USE
//l
// 1. Set the properties of the material of the same name to set custom render settings.
// 2. You are done.
//
// Created by Jason Hein


Shader "Production/MenuBlur"
{
	//Properties that can be set by designers
	Properties
    {
    	_MainTex ("Texture", 2D) = "white" {}
    	_BlurAmount ("Blur Amount", Float) = 0.002
    }
    
    //Shader
	SubShader
	{
		//Pass for settings brightness
		Pass 
		{
			//This is a CG shader
			CGPROGRAM
 			
 			//Define the shaders
         	#pragma vertex vertShader
         	#pragma fragment fragShader
         	
         	//What the vertex shader will recieve
         	struct vertInput
         	{
            	float4 pos : POSITION0;
            	half2 uv : TEXCOORD0;
       		};
       		
       		//What the fragment shader will recieve
         	struct vertOutput
         	{
            	float4 pos : POSITION0;
            	half2 uv : TEXCOORD0;
        	};
        	
        	//Main texuture
        	sampler2D _MainTex;
        	
        	//Amount to blur
        	float _BlurAmount;
         	
         	//Vertex Shader
         	vertOutput vertShader(vertInput input)
         	{
         		//A container for the vertexOutput
         		vertOutput output;
         		
         		//Calculate the vertex's position according to the camera
         		output.pos = mul(UNITY_MATRIX_MVP, input.pos);
         		
         		//Give output the texture coordinates
         		output.uv = input.uv;
         		
         		//Return our output
         		return output;
         	}
         	
         	//Fragment Shader
         	float4 fragShader(vertOutput output) : COLOR
         	{
         		//All our values are interpolated, so now we can do per-pixel calculations
 				
 				//Base colour of this fragment
 				float3 finalColor = tex2D(_MainTex, output.uv).xyz;
 				finalColor += tex2D(_MainTex, output.uv + float2(_BlurAmount, 0.0)).xyz;
 				finalColor += tex2D(_MainTex, output.uv + float2(0.0, _BlurAmount)).xyz;
 				finalColor += tex2D(_MainTex, output.uv - float2(_BlurAmount, 0.0)).xyz;
 				finalColor += tex2D(_MainTex, output.uv - float2(0.0, _BlurAmount)).xyz;
 				finalColor /= 5.0;

         		//Return the final colour of the fragment
         		return float4(finalColor, 1.0);
         	}
         	
 			//End the cg shader
 			ENDCG
		}
	}
}
