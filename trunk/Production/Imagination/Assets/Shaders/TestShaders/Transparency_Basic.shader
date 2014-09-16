Shader "Transparency/Basic" {
   Properties {
      u_Colour ("Colour and Alpha of the material", Color) = (0.5, 0.0, 0.0, 1.0)
   }
   SubShader {
      Tags { "Queue" = "Transparent" } 
         // draw after all opaque geometry has been drawn
      Pass {
         ZWrite Off // don't write to depth buffer 
            // in order not to occlude other objects
 
         Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
 
         CGPROGRAM
 
         #pragma vertex vert 
         #pragma fragment frag
         
         #include "UnityCG.cginc"
         
         struct vertexData
		{
			float4 pos:POSITION0;
			float4 col:COLOR0;
		};
         
         
         //uniforms
         float4 u_Colour;
         
 
         vertexData vert(vertexData vertexdata)
         {
         	vertexdata.pos = mul(UNITY_MATRIX_MVP, vertexdata.pos);
            return vertexdata;
         }
         
  		 float4 frag(vertexData vertexdata) : COLOR
         {
            return vertexdata.col;
               // the fourth component (alpha) is important: 
               // this is semitransparent green
         }
 
         ENDCG  
      }
   }
}