Shader "Custom/BetterCutout" {
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader
		{
			Tags
			{ 
				"Queue" = "Transparent" 
				"IgnoreProjector" = "True" 
				"RenderType" = "Transparent" 
			}
			LOD 200

			CGPROGRAM
			#pragma surface surf Lambert alpha

			sampler2D _MainTex;
			sampler2D _CutTex;
			fixed4 _Color;
			float _Cutoff;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				c = _Color;
				float ca = tex2D(_MainTex, IN.uv_MainTex).a;
				o.Albedo = c.rgb;

				if (ca > _Cutoff)
					o.Alpha = c.a;
				else
					o.Alpha = 0;
			}
			ENDCG
	}

	Fallback "Transparent/VertexLit"
}
