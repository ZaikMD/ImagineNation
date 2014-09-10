Shader "NoDraw" {
   SubShader {
      Pass {
 
         CGPROGRAM
 
         #pragma vertex vert 
         #pragma fragment frag
 
         float4 vert(void) : SV_POSITION 
         {
            return float4(0,0,0,0);
         }
 
         float4 frag(void) : COLOR
         {
         	discard;
            return float4(0,0,0,0);
         }
 
         ENDCG  
      }
   }
}