const texture DayTexture;
const texture BumpTexture;
const texture SpecularTexture;

sampler DayTextureSampler = 
sampler_state
{
    Texture = <DayTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler NormalMapSampler = 
sampler_state
{
    Texture = <BumpTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler SpecularSampler = 
sampler_state
{
    Texture = <SpecularTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

float3 GetNormal(float3 normal, float2 texCoord, float3x3 tangentToWorld)
{
	// look up the normal from the normal map, and transform from tangent space
  // into world space using the matrix created above.  normalize the result
  // in case the matrix contains scaling.
  float3 normalFromMap = tex2D(NormalMapSampler, texCoord);
  normalFromMap = mul(normalFromMap, tangentToWorld);
  normalFromMap = normalize(normalFromMap);
  return normalFromMap;
}

float GetSpecularFactor(float2 texCoord)
{
	return tex2D(SpecularSampler, texCoord).r;
}

float4 GetColour(float2 texCoord)
{
	return tex2D(DayTextureSampler, texCoord)
		;//* tex2D(DetailTextureSampler, texCoord * 20);
}

const int CullMode;

#include "ShaderCore.fxh"