Shader "Transparency_Basic" {
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
         
         
         //uniforms
         float4 u_Colour;
         
 
         float4 vert(float4 vertexPosition : POSITION) : SV_POSITION 
         {
            return mul(UNITY_MATRIX_MVP, vertexPosition);
         }
 
         float4 frag(void) : COLOR
         {
            return u_Colour;
               // the fourth component (alpha) is important: 
               // this is semitransparent green
         }
 
         ENDCG  
      }
   }
}