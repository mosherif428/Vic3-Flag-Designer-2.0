Texture2D<float4> inputTexture : register(t0);
SamplerState textureSampler : register(s0);

float4 OverlayColor : register(c0);
float BlendMode : register(c1);

float4 main(float4 pos : SV_POSITION, float2 texCoord : TEXCOORD) : SV_TARGET
{
    // Sample the input texture
	float4 texColor = inputTexture.Sample(textureSampler, texCoord);

    // Apply color overlay
	float4 overlay = lerp(texColor, OverlayColor, BlendMode);

	return overlay;
}
