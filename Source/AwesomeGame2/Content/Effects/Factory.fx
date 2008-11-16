const texture Texture;
const bool TextureEnabled;
const float3 DiffuseColour;
const bool IsSelected;

sampler TextureSampler = 
sampler_state
{
    Texture = <Texture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 GetColour(float2 texCoord)
{
	if (TextureEnabled)
		return tex2D(TextureSampler, texCoord);
	else if (IsSelected)
		return float4(DiffuseColour + float3(0.2f, 0.2f, 0.2f), 1);
	else
		return float4(DiffuseColour, 1);
}

const int CullMode;

#include "ShaderCore.fxh"