float time=0;
float strength=0;

texture lightWorld;
sampler lightTex = sampler_state{Texture = lightWorld;};

texture darkWorld;
sampler  darkTex = sampler_state{Texture = darkWorld;};

texture mask;
sampler maskTex = sampler_state{Texture = mask;};

float2 position;

float2 TextureSize=float2(1808,781);
float2 MaskSize=float2(1280,1280);

float masked;

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float2 Pposition=input.TexCoord*TextureSize;
	float2 diff = Pposition-position;
	float dist = length(diff);

	//float factor = pow(saturate(dist/(1+strength)),2);
	float factor = tex2D(maskTex, input.TexCoord);
	factor=10*factor-masked;
	factor=saturate(factor);
	
	float4 color = (1-factor)*tex2D(darkTex, input.TexCoord) + factor*tex2D(lightTex, input.TexCoord);
    return color;
}

float4 PixelShaderFunction2(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float2 Pposition=input.TexCoord*TextureSize;
	float2 diff = Pposition-position;
	float dist = length(diff);

	float2 offset=diff/10*(1+sin(time+8*dist/(1+strength)))/TextureSize;

	float factor = pow(saturate(dist/(1+strength)),2);

	float4 color = (1-factor)*tex2D(darkTex, input.TexCoord+offset*(1-factor)) + factor*tex2D(lightTex, input.TexCoord+offset*(1-factor));
    return color;
}

float4 PixelShaderFunction3(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	float2 Pposition=input.TexCoord*TextureSize;
	float2 diff = Pposition-position;
	float dist = length(diff);

	float2 offset=diff/10*(1+sin(time+8*dist/(1+strength)))/TextureSize;

	float factor = 1-pow(saturate(dist/(1+strength)),2);
		
	float factor2 = tex2D(maskTex, input.TexCoord);
	factor2=masked-10*factor2;
	factor2=saturate(factor2);
	
	float4 color = factor*factor2*tex2D(darkTex, input.TexCoord+offset*(factor)) + (1-(factor*factor2))*tex2D(lightTex, input.TexCoord+offset*(factor));
    return color;
}

float4 PixelShaderFunction4(VertexShaderOutput input) : COLOR0
{
	float2 Pposition=input.TexCoord*TextureSize;
	float2 diff = Pposition-position;
	float dist = length(diff);

	//float factor = pow(saturate(dist/(1+strength)),2);
	float factor = tex2D(maskTex, 5*input.TexCoord);
	factor=10*factor-masked;
	factor=0.5f*sin(0.5f*factor)+0.5f;
	//factor=saturate(factor);
	
	float4 color = (1-factor)*tex2D(darkTex, input.TexCoord) + factor*tex2D(lightTex, input.TexCoord);
    return color;
}

float4 BlendPixelShader(VertexShaderOutput input) : COLOR0
{
	float factor = tex2D(maskTex, input.TexCoord);
	
	float4 color = (1-factor)*tex2D(darkTex, input.TexCoord) + factor*tex2D(lightTex, input.TexCoord);
    return color;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique Technique2
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction2();
	}
}

technique Technique3
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction3();
	}
}

technique Technique4
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction4();
	}
}

technique Blend
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 BlendPixelShader();
	}
}