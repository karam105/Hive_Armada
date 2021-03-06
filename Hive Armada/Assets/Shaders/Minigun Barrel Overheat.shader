// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:True,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|diff-6824-OUT,diffpow-7265-OUT,spec-1813-OUT,gloss-1813-OUT,normal-5964-RGB,emission-6129-OUT;n:type:ShaderForge.SFN_Color,id:6665,x:31829,y:32890,ptovrint:False,ptlb:AlbedoColor,ptin:_AlbedoColor,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7736,x:31414,y:32666,ptovrint:True,ptlb:AlbedoTexture,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:571f686a3519a0743a0d29a932411e4e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5964,x:32224,y:32947,ptovrint:True,ptlb:NormalMapTexture,ptin:_BumpMap,varname:_BumpMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c2930cffacfc3db489822638ce94f355,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Slider,id:1813,x:31957,y:32511,ptovrint:False,ptlb:Smoothness,ptin:_Smoothness,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.75,max:1;n:type:ShaderForge.SFN_Color,id:2479,x:33030,y:32680,ptovrint:False,ptlb:overheatColor,ptin:_overheatColor,varname:node_2479,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Set,id:3385,x:33204,y:32680,varname:overheat_color,prsc:2|IN-2479-RGB;n:type:ShaderForge.SFN_Get,id:5115,x:31573,y:33132,varname:node_5115,prsc:2|IN-3385-OUT;n:type:ShaderForge.SFN_Multiply,id:2301,x:31791,y:33132,varname:node_2301,prsc:2|A-5115-OUT,B-5115-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9889,x:33030,y:32867,ptovrint:False,ptlb:_overheatPercent,ptin:_overheatPercent,varname:node_9889,prsc:2,glob:True,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Set,id:821,x:33204,y:32867,varname:overheat_percent,prsc:2|IN-9889-OUT;n:type:ShaderForge.SFN_Get,id:3518,x:31436,y:33305,varname:node_3518,prsc:2|IN-821-OUT;n:type:ShaderForge.SFN_Clamp01,id:2834,x:31621,y:33305,varname:node_2834,prsc:2|IN-3518-OUT;n:type:ShaderForge.SFN_Multiply,id:3572,x:32010,y:33188,varname:node_3572,prsc:2|A-2301-OUT,B-1324-OUT;n:type:ShaderForge.SFN_RemapRange,id:1324,x:31791,y:33305,varname:node_1324,prsc:2,frmn:0,frmx:1,tomn:-0.6,tomx:1|IN-2834-OUT;n:type:ShaderForge.SFN_Clamp01,id:6129,x:32167,y:33188,varname:node_6129,prsc:2|IN-3572-OUT;n:type:ShaderForge.SFN_Tex2d,id:3319,x:32125,y:32337,ptovrint:False,ptlb:MetallicTexture,ptin:_MetallicTexture,varname:node_3319,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f3c64de03fb08e84f873cd8874f580e0,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7265,x:32416,y:32474,varname:node_7265,prsc:2|A-3319-RGB,B-1813-OUT;n:type:ShaderForge.SFN_Multiply,id:9917,x:31735,y:32737,varname:node_9917,prsc:2|A-7736-RGB,B-8362-OUT;n:type:ShaderForge.SFN_OneMinus,id:8362,x:31503,y:32841,varname:node_8362,prsc:2|IN-1324-OUT;n:type:ShaderForge.SFN_Multiply,id:6824,x:32276,y:32645,varname:node_6824,prsc:2|A-9086-OUT,B-6665-RGB;n:type:ShaderForge.SFN_If,id:9086,x:32035,y:32624,varname:node_9086,prsc:2|A-8362-OUT,B-6775-OUT,GT-9917-OUT,EQ-6665-RGB,LT-6665-RGB;n:type:ShaderForge.SFN_Vector1,id:6775,x:31655,y:32518,varname:node_6775,prsc:2,v1:0;proporder:5964-6665-7736-1813-3319;pass:END;sub:END;*/

Shader "Hive Armada/Minigun Barrel Overheat" {
	Properties{
		_BumpMap("NormalMapTexture", 2D) = "black" {}
	_AlbedoColor("AlbedoColor", Color) = (1,1,1,1)
		_MainTex("AlbedoTexture", 2D) = "white" {}
	_Smoothness("Smoothness", Range(0, 1)) = 0.75
		_MetallicTexture("MetallicTexture", 2D) = "white" {}
	}
		SubShader{
		Tags{
		"RenderType" = "Opaque"
	}
		Pass{
		Name "FORWARD"
		Tags{
		"LightMode" = "ForwardBase"
	}


		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_FORWARDBASE
#define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
#define _GLOSSYENV 1
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardBRDF.cginc"
#pragma multi_compile_fwdbase_fullshadows
#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
#pragma multi_compile_fog
#pragma only_renderers d3d9 d3d11 glcore gles 
#pragma target 3.0
		uniform float4 _AlbedoColor;
	uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
	uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
	uniform float _Smoothness;
	uniform float4 _overheatColor;
	uniform float _overheatPercent;
	uniform sampler2D _MetallicTexture; uniform float4 _MetallicTexture_ST;
	struct VertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 texcoord0 : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
		float2 texcoord2 : TEXCOORD2;
	};
	struct VertexOutput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		float2 uv2 : TEXCOORD2;
		float4 posWorld : TEXCOORD3;
		float3 normalDir : TEXCOORD4;
		float3 tangentDir : TEXCOORD5;
		float3 bitangentDir : TEXCOORD6;
		LIGHTING_COORDS(7,8)
			UNITY_FOG_COORDS(9)
#if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
			float4 ambientOrLightmapUV : TEXCOORD10;
#endif
	};
	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.uv1 = v.texcoord1;
		o.uv2 = v.texcoord2;
#ifdef LIGHTMAP_ON
		o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		o.ambientOrLightmapUV.zw = 0;
#endif
#ifdef DYNAMICLIGHTMAP_ON
		o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
		o.normalDir = UnityObjectToWorldNormal(v.normal);
		o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
		o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
		o.posWorld = mul(unity_ObjectToWorld, v.vertex);
		float3 lightColor = _LightColor0.rgb;
		o.pos = UnityObjectToClipPos(v.vertex);
		UNITY_TRANSFER_FOG(o,o.pos);
		TRANSFER_VERTEX_TO_FRAGMENT(o)
			return o;
	}
	float4 frag(VertexOutput i) : COLOR{
		i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	float4 _BumpMap_var = tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap));
	float3 normalLocal = _BumpMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
	float3 viewReflectDirection = reflect(-viewDirection, normalDirection);
	float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
	float3 lightColor = _LightColor0.rgb;
	float3 halfDirection = normalize(viewDirection + lightDirection);
	////// Lighting:
	float attenuation = LIGHT_ATTENUATION(i);
	float3 attenColor = attenuation * _LightColor0.xyz;
	float Pi = 3.141592654;
	float InvPi = 0.31830988618;
	///////// Gloss:
	float gloss = _Smoothness;
	float perceptualRoughness = 1.0 - _Smoothness;
	float roughness = perceptualRoughness * perceptualRoughness;
	float specPow = exp2(gloss * 10.0 + 1.0);
	/////// GI Data:
	UnityLight light;
#ifdef LIGHTMAP_OFF
	light.color = lightColor;
	light.dir = lightDirection;
	light.ndotl = LambertTerm(normalDirection, light.dir);
#else
	light.color = half3(0.f, 0.f, 0.f);
	light.ndotl = 0.0f;
	light.dir = half3(0.f, 0.f, 0.f);
#endif
	UnityGIInput d;
	d.light = light;
	d.worldPos = i.posWorld.xyz;
	d.worldViewDir = viewDirection;
	d.atten = attenuation;
#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
	d.ambient = 0;
	d.lightmapUV = i.ambientOrLightmapUV;
#else
	d.ambient = i.ambientOrLightmapUV;
#endif
#if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
	d.boxMin[0] = unity_SpecCube0_BoxMin;
	d.boxMin[1] = unity_SpecCube1_BoxMin;
#endif
#if UNITY_SPECCUBE_BOX_PROJECTION
	d.boxMax[0] = unity_SpecCube0_BoxMax;
	d.boxMax[1] = unity_SpecCube1_BoxMax;
	d.probePosition[0] = unity_SpecCube0_ProbePosition;
	d.probePosition[1] = unity_SpecCube1_ProbePosition;
#endif
	d.probeHDR[0] = unity_SpecCube0_HDR;
	d.probeHDR[1] = unity_SpecCube1_HDR;
	Unity_GlossyEnvironmentData ugls_en_data;
	ugls_en_data.roughness = 1.0 - gloss;
	ugls_en_data.reflUVW = viewReflectDirection;
	UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data);
	lightDirection = gi.light.dir;
	lightColor = gi.light.color;
	////// Specular:
	float NdotL = saturate(dot(normalDirection, lightDirection));
	float LdotH = saturate(dot(lightDirection, halfDirection));
	float3 specularColor = _Smoothness;
	float specularMonochrome;
	float overheat_percent = _overheatPercent;
	float node_1324 = (saturate(overheat_percent)*1.6 + -0.6);
	float node_8362 = (1.0 - node_1324);
	float node_9086_if_leA = step(node_8362,0.0);
	float node_9086_if_leB = step(0.0,node_8362);
	float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
	float3 node_9917 = (_MainTex_var.rgb*node_8362);
	float3 diffuseColor = (lerp((node_9086_if_leA*_AlbedoColor.rgb) + (node_9086_if_leB*node_9917),_AlbedoColor.rgb,node_9086_if_leA*node_9086_if_leB)*_AlbedoColor.rgb); // Need this for specular when using metallic
	diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, specularColor, specularColor, specularMonochrome);
	specularMonochrome = 1.0 - specularMonochrome;
	float NdotV = abs(dot(normalDirection, viewDirection));
	float NdotH = saturate(dot(normalDirection, halfDirection));
	float VdotH = saturate(dot(viewDirection, halfDirection));
	float visTerm = SmithJointGGXVisibilityTerm(NdotL, NdotV, roughness);
	float normTerm = GGXTerm(NdotH, roughness);
	float specularPBL = (visTerm*normTerm) * UNITY_PI;
#ifdef UNITY_COLORSPACE_GAMMA
	specularPBL = sqrt(max(1e-4h, specularPBL));
#endif
	specularPBL = max(0, specularPBL * NdotL);
#if defined(_SPECULARHIGHLIGHTS_OFF)
	specularPBL = 0.0;
#endif
	half surfaceReduction;
#ifdef UNITY_COLORSPACE_GAMMA
	surfaceReduction = 1.0 - 0.28*roughness*perceptualRoughness;
#else
	surfaceReduction = 1.0 / (roughness*roughness + 1.0);
#endif
	specularPBL *= any(specularColor) ? 1.0 : 0.0;
	float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
	half grazingTerm = saturate(gloss + specularMonochrome);
	float3 indirectSpecular = (gi.indirect.specular);
	indirectSpecular *= FresnelLerp(specularColor, grazingTerm, NdotV);
	indirectSpecular *= surfaceReduction;
	float3 specular = (directSpecular + indirectSpecular);
	/////// Diffuse:
	NdotL = max(0.0,dot(normalDirection, lightDirection));
	half fd90 = 0.5 + 2 * LdotH * LdotH * (1 - gloss);
	float nlPow5 = Pow5(1 - NdotL);
	float nvPow5 = Pow5(1 - NdotV);
	float3 directDiffuse = ((1 + (fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
	float3 indirectDiffuse = float3(0,0,0);
	indirectDiffuse += gi.indirect.diffuse;
	float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
	////// Emissive:
	float3 overheat_color = _overheatColor.rgb;
	float3 node_5115 = overheat_color;
	float3 emissive = saturate(((node_5115*node_5115)*node_1324));
	/// Final Color:
	float3 finalColor = diffuse + specular + emissive;
	fixed4 finalRGBA = fixed4(finalColor,1);
	UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
	return finalRGBA;
	}
		ENDCG
	}
		Pass{
		Name "FORWARD_DELTA"
		Tags{
		"LightMode" = "ForwardAdd"
	}
		Blend One One


		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_FORWARDADD
#define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
#define _GLOSSYENV 1
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardBRDF.cginc"
#pragma multi_compile_fwdadd_fullshadows
#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
#pragma multi_compile_fog
#pragma only_renderers d3d9 d3d11 glcore gles 
#pragma target 3.0
		uniform float4 _AlbedoColor;
	uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
	uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
	uniform float _Smoothness;
	uniform float4 _overheatColor;
	uniform float _overheatPercent;
	uniform sampler2D _MetallicTexture; uniform float4 _MetallicTexture_ST;
	struct VertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 texcoord0 : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
		float2 texcoord2 : TEXCOORD2;
	};
	struct VertexOutput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		float2 uv2 : TEXCOORD2;
		float4 posWorld : TEXCOORD3;
		float3 normalDir : TEXCOORD4;
		float3 tangentDir : TEXCOORD5;
		float3 bitangentDir : TEXCOORD6;
		LIGHTING_COORDS(7,8)
			UNITY_FOG_COORDS(9)
	};
	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.uv1 = v.texcoord1;
		o.uv2 = v.texcoord2;
		o.normalDir = UnityObjectToWorldNormal(v.normal);
		o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
		o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
		o.posWorld = mul(unity_ObjectToWorld, v.vertex);
		float3 lightColor = _LightColor0.rgb;
		o.pos = UnityObjectToClipPos(v.vertex);
		UNITY_TRANSFER_FOG(o,o.pos);
		TRANSFER_VERTEX_TO_FRAGMENT(o)
			return o;
	}
	float4 frag(VertexOutput i) : COLOR{
		i.normalDir = normalize(i.normalDir);
	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
	float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
	float4 _BumpMap_var = tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap));
	float3 normalLocal = _BumpMap_var.rgb;
	float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
	float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
	float3 lightColor = _LightColor0.rgb;
	float3 halfDirection = normalize(viewDirection + lightDirection);
	////// Lighting:
	float attenuation = LIGHT_ATTENUATION(i);
	float3 attenColor = attenuation * _LightColor0.xyz;
	float Pi = 3.141592654;
	float InvPi = 0.31830988618;
	///////// Gloss:
	float gloss = _Smoothness;
	float perceptualRoughness = 1.0 - _Smoothness;
	float roughness = perceptualRoughness * perceptualRoughness;
	float specPow = exp2(gloss * 10.0 + 1.0);
	////// Specular:
	float NdotL = saturate(dot(normalDirection, lightDirection));
	float LdotH = saturate(dot(lightDirection, halfDirection));
	float3 specularColor = _Smoothness;
	float specularMonochrome;
	float overheat_percent = _overheatPercent;
	float node_1324 = (saturate(overheat_percent)*1.6 + -0.6);
	float node_8362 = (1.0 - node_1324);
	float node_9086_if_leA = step(node_8362,0.0);
	float node_9086_if_leB = step(0.0,node_8362);
	float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
	float3 node_9917 = (_MainTex_var.rgb*node_8362);
	float3 diffuseColor = (lerp((node_9086_if_leA*_AlbedoColor.rgb) + (node_9086_if_leB*node_9917),_AlbedoColor.rgb,node_9086_if_leA*node_9086_if_leB)*_AlbedoColor.rgb); // Need this for specular when using metallic
	diffuseColor = DiffuseAndSpecularFromMetallic(diffuseColor, specularColor, specularColor, specularMonochrome);
	specularMonochrome = 1.0 - specularMonochrome;
	float NdotV = abs(dot(normalDirection, viewDirection));
	float NdotH = saturate(dot(normalDirection, halfDirection));
	float VdotH = saturate(dot(viewDirection, halfDirection));
	float visTerm = SmithJointGGXVisibilityTerm(NdotL, NdotV, roughness);
	float normTerm = GGXTerm(NdotH, roughness);
	float specularPBL = (visTerm*normTerm) * UNITY_PI;
#ifdef UNITY_COLORSPACE_GAMMA
	specularPBL = sqrt(max(1e-4h, specularPBL));
#endif
	specularPBL = max(0, specularPBL * NdotL);
#if defined(_SPECULARHIGHLIGHTS_OFF)
	specularPBL = 0.0;
#endif
	specularPBL *= any(specularColor) ? 1.0 : 0.0;
	float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
	float3 specular = directSpecular;
	/////// Diffuse:
	NdotL = max(0.0,dot(normalDirection, lightDirection));
	half fd90 = 0.5 + 2 * LdotH * LdotH * (1 - gloss);
	float nlPow5 = Pow5(1 - NdotL);
	float nvPow5 = Pow5(1 - NdotV);
	float3 directDiffuse = ((1 + (fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
	float3 diffuse = directDiffuse * diffuseColor;
	/// Final Color:
	float3 finalColor = diffuse + specular;
	fixed4 finalRGBA = fixed4(finalColor * 1,0);
	UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
	return finalRGBA;
	}
		ENDCG
	}
		Pass{
		Name "Meta"
		Tags{
		"LightMode" = "Meta"
	}
		Cull Off

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#define UNITY_PASS_META 1
#define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
#define _GLOSSYENV 1
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "UnityPBSLighting.cginc"
#include "UnityStandardBRDF.cginc"
#include "UnityMetaPass.cginc"
#pragma fragmentoption ARB_precision_hint_fastest
#pragma multi_compile_shadowcaster
#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
#pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
#pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
#pragma multi_compile_fog
#pragma only_renderers d3d9 d3d11 glcore gles 
#pragma target 3.0
		uniform float4 _AlbedoColor;
	uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
	uniform float _Smoothness;
	uniform float4 _overheatColor;
	uniform float _overheatPercent;
	struct VertexInput {
		float4 vertex : POSITION;
		float2 texcoord0 : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
		float2 texcoord2 : TEXCOORD2;
	};
	struct VertexOutput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		float2 uv2 : TEXCOORD2;
		float4 posWorld : TEXCOORD3;
	};
	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.uv1 = v.texcoord1;
		o.uv2 = v.texcoord2;
		o.posWorld = mul(unity_ObjectToWorld, v.vertex);
		o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
		return o;
	}
	float4 frag(VertexOutput i) : SV_Target{
		float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
		UnityMetaInput o;
		UNITY_INITIALIZE_OUTPUT(UnityMetaInput, o);

		float3 overheat_color = _overheatColor.rgb;
		float3 node_5115 = overheat_color;
		float overheat_percent = _overheatPercent;
		float node_1324 = (saturate(overheat_percent)*1.6 + -0.6);
		o.Emission = saturate(((node_5115*node_5115)*node_1324));

		float node_8362 = (1.0 - node_1324);
		float node_9086_if_leA = step(node_8362,0.0);
		float node_9086_if_leB = step(0.0,node_8362);
		float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
		float3 node_9917 = (_MainTex_var.rgb*node_8362);
		float3 diffColor = (lerp((node_9086_if_leA*_AlbedoColor.rgb) + (node_9086_if_leB*node_9917),_AlbedoColor.rgb,node_9086_if_leA*node_9086_if_leB)*_AlbedoColor.rgb);
		float specularMonochrome;
		float3 specColor;
		diffColor = DiffuseAndSpecularFromMetallic(diffColor, _Smoothness, specColor, specularMonochrome);
		float roughness = 1.0 - _Smoothness;
		o.Albedo = diffColor + specColor * roughness * roughness * 0.5;

		return UnityMetaFragment(o);
	}
		ENDCG
	}
	}
		FallBack "Diffuse"
		CustomEditor "ShaderForgeMaterialInspector"
}
