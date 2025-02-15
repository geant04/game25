#ifndef SHADOWDISTANCE_INCLUDED
#define SHADOWDISTANCE_INCLUDED

void ShadowDistance_float(float depth, float4 scene_color, float threshold, float blur_distance, out float4 Out)
{
    if (depth < threshold + blur_distance) {
        Out = lerp(float4(0, 0, 0, 1), scene_color, saturate((depth - threshold) / blur_distance));
    } else {
        Out = scene_color;
    }
}

#endif
