#ifndef LUMIERE_INCLUDED
#define LUMIERE_INCLUDED

#include "Lighting.cginc"
#include "AutoLight.cginc"
#include "include_fragment.cginc"

uniform float _Fondu;
uniform float3 _OmbreCouleur;
uniform float3 _ReflectionCouleur;

void CalculerLumiereRampe(in FragmentDonnees fragment, in float3 viewDir, in float shadowAttenuation, out float3 lumiere)
{
	float NdotL = dot(fragment.normal, _WorldSpaceLightPos0.xyz);
	float diffuse = smoothstep(0, _Fondu, NdotL);

	float3 halfVector = normalize(_WorldSpaceLightPos0.xyz + normalize(viewDir));
	float NdotH = dot(fragment.normal, halfVector);
	float specularIntensity = pow(NdotH * diffuse, fragment.specular * fragment.specular);
	float specularIntensitySmooth = smoothstep(0.005, _Fondu, specularIntensity);
	float3 specular = specularIntensitySmooth * _ReflectionCouleur;


	lumiere = shadowAttenuation * _LightColor0 * (diffuse + specular) + _OmbreCouleur;
}
#define CALCULER_LUMIERE_RAMPE CalculerLumiereRampe

#endif