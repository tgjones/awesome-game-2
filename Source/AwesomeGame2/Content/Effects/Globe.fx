const texture PlanetTexture;
const texture DetailTexture;

sampler PlanetTextureSampler = 
sampler_state
{
    Texture = <PlanetTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

sampler DetailTextureSampler = 
sampler_state
{
    Texture = <DetailTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 GetColour(float2 texCoord)
{
	return tex2D(PlanetTextureSampler, texCoord)
		* tex2D(DetailTextureSampler, texCoord * 20);
}

const int CullMode;

#include "ShaderCore.fxh"