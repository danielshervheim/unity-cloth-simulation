// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BurningCloth"
{
	Properties
	{
		_Diffuse("Diffuse", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_BurnAmount("Burn Amount", Range( 0 , 1)) = 0
		_BurnAlpha("Burn Alpha", 2D) = "white" {}
		_SmokeOffset("Smoke Offset", Range( 0 , 1)) = 0
		[HDR]_Emission("Emission", Color) = (0,0,0,0)
		_Flowmap("Flowmap", 2D) = "white" {}
		_tscale("tscale", Float) = 1
		_tamp("tamp", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			half2 uv_texcoord;
		};

		uniform sampler2D _Diffuse;
		uniform float4 _Diffuse_ST;
		uniform sampler2D _BurnAlpha;
		uniform float4 _BurnAlpha_ST;
		uniform sampler2D _Flowmap;
		uniform float4 _Flowmap_ST;
		uniform half _tscale;
		uniform half _tamp;
		uniform half _BurnAmount;
		uniform half _SmokeOffset;
		uniform half4 _Emission;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Diffuse = i.uv_texcoord * _Diffuse_ST.xy + _Diffuse_ST.zw;
			float2 uv_BurnAlpha = i.uv_texcoord * _BurnAlpha_ST.xy + _BurnAlpha_ST.zw;
			float2 uv_Flowmap = i.uv_texcoord * _Flowmap_ST.xy + _Flowmap_ST.zw;
			float mulTime61 = _Time.y * _tscale;
			float2 break47 = ( uv_BurnAlpha + ( (tex2D( _Flowmap, uv_Flowmap )).rg * ( sin( mulTime61 ) * _tamp ) ) );
			float temp_output_29_0 = ( break47.y + (-0.4 + (_BurnAmount - 0.0) * (0.9 - -0.4) / (1.0 - 0.0)) );
			float2 appendResult34 = (half2(break47.x , ( temp_output_29_0 + _SmokeOffset )));
			o.Albedo = ( tex2D( _Diffuse, uv_Diffuse ) * tex2D( _BurnAlpha, appendResult34 ).a ).rgb;
			float2 appendResult28 = (half2(break47.x , temp_output_29_0));
			half4 tex2DNode24 = tex2D( _BurnAlpha, appendResult28 );
			o.Emission = ( ( 1.0 - tex2DNode24.a ) * _Emission ).rgb;
			o.Alpha = 1;
			clip( tex2DNode24.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16211
205;92;1436;650;2409.904;-355.0359;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;62;-1533.705,1005.036;Float;False;Property;_tscale;tscale;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;61;-1359.505,1008.936;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1211.304,1098.636;Float;False;Property;_tamp;tamp;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;63;-1146.305,1002.436;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;49;-1987.404,722.9357;Float;True;Property;_Flowmap;Flowmap;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1013.704,1042.736;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;50;-1697.504,722.9357;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-2261.147,575.8281;Float;False;0;30;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1458.305,778.8358;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-1499.904,581.2357;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1333.337,662.8354;Float;False;Property;_BurnAmount;Burn Amount;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;47;-1372.101,520.045;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RelayNode;45;-1073.11,547.4239;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;27;-1047.147,668.0283;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.4;False;4;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-846.1465,644.0283;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RelayNode;44;-1072.11,472.4239;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1177.174,299.2672;Float;False;Property;_SmokeOffset;Smoke Offset;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-864.7289,280.5945;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;28;-704.1466,573.0283;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;30;-1424.637,229.4301;Float;True;Property;_BurnAlpha;Burn Alpha;3;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;24;-432.0957,546.5762;Float;True;Property;_Sample1;Sample1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;34;-660.2213,254.986;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;43;-85.99573,871.1736;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-490.2844,13.46139;Float;True;Property;_Diffuse;Diffuse;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;-499.2917,224.8517;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-365.7587,1048.385;Float;False;Property;_Emission;Emission;5;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-77.06882,958.1693;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-110.6914,176.1926;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;203.6293,263.4086;Half;False;True;2;Half;ASEMaterialInspector;0;0;Standard;BurningCloth;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;61;0;62;0
WireConnection;63;0;61;0
WireConnection;64;0;63;0
WireConnection;64;1;65;0
WireConnection;50;0;49;0
WireConnection;60;0;50;0
WireConnection;60;1;64;0
WireConnection;48;0;26;0
WireConnection;48;1;60;0
WireConnection;47;0;48;0
WireConnection;45;0;47;1
WireConnection;27;0;25;0
WireConnection;29;0;45;0
WireConnection;29;1;27;0
WireConnection;44;0;47;0
WireConnection;32;0;29;0
WireConnection;32;1;33;0
WireConnection;28;0;44;0
WireConnection;28;1;29;0
WireConnection;24;0;30;0
WireConnection;24;1;28;0
WireConnection;34;0;44;0
WireConnection;34;1;32;0
WireConnection;43;0;24;4
WireConnection;31;0;30;0
WireConnection;31;1;34;0
WireConnection;41;0;43;0
WireConnection;41;1;42;0
WireConnection;8;0;9;0
WireConnection;8;1;31;4
WireConnection;0;0;8;0
WireConnection;0;2;41;0
WireConnection;0;10;24;4
ASEEND*/
//CHKSM=F6C6B005E3783533D9B946C99ACA3971E0C7232B