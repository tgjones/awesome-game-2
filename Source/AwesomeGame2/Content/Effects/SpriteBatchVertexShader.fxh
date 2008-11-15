float2   ViewportSize    : register(c0);
float2   TextureSize     : register(c1);
float4x4 MatrixTransform : register(c2);
sampler  TextureSampler  : register(s0);

void SpriteVertexShader(
	inout float4 position : POSITION0,
	inout float4 color    : COLOR0,
	inout float2 texCoord : TEXCOORD0 )
{
    // Apply the matrix transform.
    position = mul(position, transpose(MatrixTransform));
    
	// Half pixel offset for correct texel centering.
	position.xy -= 0.5;

	// Viewport adjustment.
	position.xy /= ViewportSize;
	position.xy *= float2(2, -2);
	position.xy -= float2(1, -1);

	// Compute the texture coordinate.
	texCoord /= TextureSize;
}