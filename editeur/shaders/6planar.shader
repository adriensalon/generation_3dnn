Shader "test6plan"
{
    Properties
    {
        [NoScaleOffset] _xMoins ("X Moins", 2D) = "white" {}
        [NoScaleOffset] _xPlus ("X Plus", 2D) = "white" {}
        [NoScaleOffset] _yMoins ("Y Moins", 2D) = "white" {}
        [NoScaleOffset] _yPlus ("Y Plus", 2D) = "white" {}
        [NoScaleOffset] _zMoins ("Z Moins", 2D) = "white" {}
        [NoScaleOffset] _zPlus ("Z Plus", 2D) = "white" {}

		
		_CentreX ("Centre X", Float) = 1
		_CentreY ("Centre Y", Float) = 1
		_CentreZ ("Centre Z", Float) = 1

		
		_TailleX ("Taille X", Float) = 1
		_TailleY ("Taille Y", Float) = 1
		_TailleZ ("Taille Z", Float) = 1





		_Taille ("Taille", Float) = 1
		_MelangeDecalage ("Decalage", Range(0, 0.5)) = 0.25
		_MelangeExposant ("Exposant", Range(1, 8)) = 2
		
		_Fondu("Step", Float) = 0.1
		[HDR] _OmbreCouleur("Ombre", Color) = (0.9,0.9,0.9,1)
		[HDR] _ReflectionCouleur("Reflection", Color) = (0.9,0.9,0.9,1)
    }
    SubShader
    {
		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			Cull back


			
            

			CGPROGRAM    
				#include "include_vertex.cginc"
				#include "include_lumiere.cginc"

				#pragma vertex vert
				#pragma fragment frag             
				#pragma multi_compile_fwdbase
				#pragma target 3.0

				uniform sampler2D _xMoins;
				uniform sampler2D _xPlus;
				uniform sampler2D _yMoins;
				uniform sampler2D _yPlus;
				uniform sampler2D _zMoins;
				uniform sampler2D _zPlus;

				uniform float _CentreX;
				uniform float _CentreY;
				uniform float _CentreZ;
				uniform float _TailleX;
				uniform float _TailleY;
				uniform float _TailleZ;
				uniform float _MelangeDecalage;
				uniform float _MelangeExposant;




				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float3 worldNormal : TEXCOORD0;
					float3 worldPosition : TEXCOORD1;
					float3 viewDir : TEXCOORD2;
					LIGHTING_COORDS(5,8)
				};

				v2f vert(appdata v)
				{
					v2f o;
					CALCULER_VERTEX(v.vertex, v.normal, o.pos, o.worldPosition, o.worldNormal, o.viewDir);	
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					return o;
				}

				float4 frag(v2f i) : SV_TARGET
				{

					// uvs
	float3 p = i.worldPosition;// * _Taille;
	p.x = (p.x - _CentreX) / _TailleX;
	p.y = (p.y - _CentreY) / _TailleY;
	p.z = (p.z - _CentreZ) / _TailleZ;


	float2 uvX = p.zy;
	float2 uvY = p.xz;
	float2 uvZ = p.xy;
	bool3 bPlusOuMoins = bool3(true, true, true);
	if (i.worldNormal.x < 0)
	{
		uvX.x = -uvX.x;
		bPlusOuMoins.x = false;
	}
	if (i.worldNormal.y < 0)
	{
		uvY.x = -uvY.x;
		bPlusOuMoins.y = false;
	}
	if (i.worldNormal.z >= 0)
	{
		uvZ.x = -uvZ.x;
		bPlusOuMoins.z = false;
	}




	// melange (hauteurs et normales)
	float3 triW = abs(i.worldNormal);
	triW = saturate(triW - _MelangeDecalage);
	triW = pow(triW, _MelangeExposant);
	triW = triW / (triW.x + triW.y + triW.z);

	// albedo
	float3 albedoX = bPlusOuMoins.x ? tex2D(_xPlus, uvX).rgb : tex2D(_xMoins, uvX).rgb;
	float3 albedoY = bPlusOuMoins.y ? tex2D(_yPlus, uvY).rgb : tex2D(_yMoins, uvY).rgb;
	float3 albedoZ = bPlusOuMoins.z ? tex2D(_zPlus, uvZ).rgb : tex2D(_zMoins, uvZ).rgb;

	// output
	float3 albedo = albedoX * triW.x + albedoY * triW.y + albedoZ * triW.z;

					float3 lumiere;
					fixed atten = LIGHT_ATTENUATION(i);
					
					float4 col;
					col.rgb = albedo * 1.7;// * atten;
					col.a = 1;
					return col;
				}
			ENDCG
        }
	}
	//Fallback "Diffuse"
}
