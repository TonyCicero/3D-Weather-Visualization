Shader "Custom/GlobeElev" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _CentrePoint ("Centre", Vector) = (0, 0, 0, 0)
        _HeightMin ("Height Min", Float) = -1
	    _HeightMid ("Height Mid", Float) = 0
        _HeightMax ("Height Max", Float) = 1
        _ColorMin ("Tint Color At Min", Color) = (0,0,0,1)
	    _ColorMid ("Tint Color At Mid", Color) = (0,0,1,1)
        _ColorMax ("Tint Color At Max", Color) = (1,1,1,1)
     }
     SubShader {
        Tags { "RenderType"="Opaque" }
  
        CGPROGRAM
        #pragma surface surf Lambert
    
        sampler2D _MainTex;
        fixed4 _ColorMin;
        fixed4 _ColorMid;
        fixed4 _ColorMax;
        float _HeightMin;
        float _HeightMid;
        float _HeightMax;
        float4 _CentrePoint;
    
        struct Input
        {
        float2 uv_MainTex;
        float3 worldPos;
        };
  
 
         void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);

            float curDistance = distance(_CentrePoint.xyz, IN.worldPos);
             
            fixed4 tintColor;
            float h = (_HeightMax-curDistance) / (_HeightMax-_HeightMin);
             if (curDistance < _HeightMid){
                tintColor = lerp(_ColorMid.rgba, _ColorMin.rgba, h*20);
            }else{
                tintColor = lerp(_ColorMax.rgba, _ColorMid.rgba, h*20);
            }
             
            o.Albedo = c.rgb * tintColor.rgb;
            o.Alpha = c.a * tintColor.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }