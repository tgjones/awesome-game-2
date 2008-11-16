const matrix WorldViewProjection;
const matrix InverseWorld;

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
	float4 lightAmbient = { 0.2f, 0.2f, 0.2f, 1.0f};
	float4 lightDiffuse = { 0.5f, 0.5f, 0.5f, 1.0f };
	float3 lightDirection = { 1.0f, 0.0f, 0.0f };
	lightDirection = normalize(mul(lightDirection, InverseWorld));
	float4 light = lightAmbient + saturate(dot(normalize(input.Normal), lightDirection));
	
	return light * GetColour(input.TexCoord);
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
		CullMode = <CullMode>;
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