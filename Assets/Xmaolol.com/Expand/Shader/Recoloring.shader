Shader "Custom/TeamMaskFinal" {
       Properties {
              _MainTex ("Base (RGB)", 2D) = "white" {}
              _From("From Color",Color) = (1,1,1,1)
              _To("To Color",Color) = (1,1,1,1)
              _HError("Hue Error",range(0,1)) = 0 // 允许的色相误差
              _SError("Saturation Error",range(0,1)) = 0 // 允许的饱和度误差
              _VError("Brightness Error",range(0,1)) = 0 // 允许的亮度误差
       }
       SubShader {
              Tags { "RenderType"="Opaque" }
              LOD 150
 
              CGPROGRAM
              #pragma surface surf Lambert noforwardadd
 
              sampler2D _MainTex;
              fixed4 _From;
              fixed4 _To;
              fixed _HError;
              fixed _SError;
              fixed _VError;
 
              struct Input {
                     float2 uv_MainTex;
              } ;
 
              fixed3 RGBtoHSV(fixed3 c)
              {
                  fixed4 K = fixed4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                  fixed4 p = lerp(fixed4(c.bg, K.wz), fixed4(c.gb, K.xy), step(c.b, c.g));
                  fixed4 q = lerp(fixed4(p.xyw, c.r), fixed4(c.r, p.yzx), step(p.x, c.r));
 
                  float d = q.x - min(q.w, q.y);
                  float e = 1.0e-10;
                  return fixed3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
              }
 
              fixed3 HSVtoRGB(fixed3 c)
              {
                  fixed4 K = fixed4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                  fixed3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                  return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
              }
 
              void surf (Input IN, inout SurfaceOutput o) {
                     fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                     fixed3 cHSV = RGBtoHSV(c);
                     fixed3 FromHSV= RGBtoHSV(_From);
                     fixed3 ToHSV= RGBtoHSV(_To);
 
                     fixed diffHue = abs(cHSV.x-FromHSV.x);
                     diffHue = lerp(diffHue,abs(diffHue-1),step(0.5,diffHue));
 
                     fixed con = step(diffHue,_HError);
                     con = con + step(abs(cHSV.y-FromHSV.y),_SError);
                     con = con + step(abs(cHSV.z-FromHSV.z),_VError);
                     con = step(2.5,con);
                     fixed3 ret = cHSV + ToHSV - FromHSV;
//                   ret.x = lerp(ret.x,ret.x-1,step(1,ret.x));
//                   ret.x = lerp(ret.x,ret.x+1,step(ret.x,0));
 
                     o.Albedo = lerp(c.rgb,HSVtoRGB(ret),fixed3(con,con,con));
 
                     o.Alpha = c.a;
              }
              ENDCG
       }
 
       Fallback "Mobile/VertexLit"
}