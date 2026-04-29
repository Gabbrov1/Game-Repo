#version 330 core

layout (location = 0) in vec3 aPosition;
layout (location = 1) in float aBrightness;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out float vBrightness;

void main()
{
    vBrightness = aBrightness;
    gl_Position = uProjection * uView * uModel * vec4(aPosition, 1.0);
}