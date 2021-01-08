#ifndef VERTEX_INCLUDED
#define VERTEX_INCLUDED

#include "UnityCG.cginc"

void CalculerVertex(in float4 localPosition, in float3 localNormal, out float4 clipPosition, out float3 worldPosition, out float3 worldNormal, out float3 viewDir)
{
	clipPosition = UnityObjectToClipPos(localPosition);
	worldPosition = mul(unity_ObjectToWorld, localPosition).xyz;
	worldNormal = normalize(mul(localNormal, (float3x3)unity_WorldToObject));
	viewDir = WorldSpaceViewDir(localPosition);
}
#define CALCULER_VERTEX CalculerVertex
#endif
