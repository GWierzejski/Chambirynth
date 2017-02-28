Shader "Custom/AddColorPattern" {
	Properties {
		_MainColor("Main Color", Color)=(1,1,1,1)
		_Color("Pattern Color", Color)=(1,1,1,1)
		_Intensity("Pattern Intensity", range(1,5))=1
     [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
     [NoScaleOffset] _Detail ("Pattern", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200


		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		struct Input {float2 uv_MainTex; float2 uv_Detail;};

        sampler2D _MainTex;
     	sampler2D _Detail;
      	fixed4 _Color;
     	fixed4 _MainColor;
     	half _Intensity;

     	 void surf (Input IN, inout SurfaceOutputStandard o) {
     	 	fixed4 cm = tex2D (_MainTex, IN.uv_MainTex) * _MainColor;
     	 	fixed4 cma = tex2D(_MainTex, IN.uv_MainTex).a - tex2D (_Detail, IN.uv_Detail).a;
     	 	fixed4 cd = tex2D (_MainTex, IN.uv_MainTex)  * _Color * _Intensity;
     	 	fixed4 cda = tex2D (_Detail, IN.uv_Detail).a;

        	o.Albedo = cma * cm.rgb;
        	o.Albedo += cda * cd.rgb;
        }
		ENDCG
	}
	FallBack "Diffuse"
}
