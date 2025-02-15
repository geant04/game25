#ifndef SHADOWDISTANCE_INCLUDED
#define SHADOWDISTANCE_INCLUDED

void ShadowDistance_float(float3 camPos, float3 worldPos, float4 scene_color, float threshold, float blur_distance, out float4 Out)
{
    float dist = length(worldPos - camPos);
    
    if (dist < threshold + blur_distance)
    {
        Out = lerp(scene_color, float4(0, 0, 0, 1), saturate((dist - threshold) / blur_distance));
    }
    else
    {
        Out = float4(0, 0, 0, 1);
    }
}

#endif
