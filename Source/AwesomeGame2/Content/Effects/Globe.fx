const matrix WorldViewProjection;
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

struct VertexShaderInput
{
	float3 Position : POSITION0;
	float3 Normal: NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 Normal: TEXCOORD0;
	float2 TexCoord : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	output.Position = mul(float4(input.Position, 1), WorldViewProjection);
	output.Normal = input.Normal;
	output.TexCoord = input.TexCoord;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Get offset from texture.
	return tex2D(PlanetTextureSampler, input.TexCoord)
		* tex2D(DetailTextureSampler, input.TexCoord * 20);
}

float4 SimplePixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return float4(1, 1, 1, 0.1);
}

technique Technique1
{
	pass Pass1
	{
		FillMode = SOLID;
		CullMode = CW;
		AlphaBlendEnable = false;
		DepthBias = 0;
	
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
	
	/*pass Pass1
	{
		FillMode = WIREFRAME;
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha; 
		DestBlend = InvSrcAlpha;
		DepthBias = -0.01;
		
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 SimplePixelShaderFunction();
	}*/
}