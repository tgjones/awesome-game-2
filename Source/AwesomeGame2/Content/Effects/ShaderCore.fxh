const matrix WorldViewProjection;
const matrix InverseWorld;
const matrix World;
const float3 CameraPosition;

struct VertexShaderInput
{
	float3 Position : POSITION0;
	float3 Normal   : NORMAL0;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position      : POSITION0;
	float3 Normal        : TEXCOORD0;
	float3 ViewDirection : TEXCOORD1;
	float2 TexCoord      : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 inputPosition = float4(input.Position, 1);
	output.Position = mul(inputPosition, WorldViewProjection);
	
	output.Normal = input.Normal;
	
	float4 worldPosition = mul(inputPosition, World);
	output.ViewDirection = float4(CameraPosition, 0.0f) - worldPosition; // V
	
	output.TexCoord = input.TexCoord;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 lightAmbient = { 0.2f, 0.2f, 0.2f, 1.0f };
	float4 lightDiffuse = { 1.0f, 0.95f, 0.8f, 1.0f };
	float3 lightDirection = mul(float3(1.0f, 0.0f, 0.0f), InverseWorld);
	float3 normal = normalize(input.Normal);
	float3 viewDirection = normalize(input.ViewDirection);
	
	float nDotL = dot(normal, lightDirection);
	
	// R = 2 * (N.L) * N - L
	float3 reflect = normalize(2.0f * nDotL * normal - lightDirection);
	float4 specular = pow(saturate(dot(reflect, viewDirection)), 31); // R.V^n
	
	// I = Acolor + Dcolor * N.L + (R.V)n
	float4 diffuse = saturate(nDotL * lightDiffuse);
	return (lightAmbient + diffuse) * GetColour(input.TexCoord) + specular;
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
		//CullMode = <CullMode>;
		//CullMode = CW;
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