#version 330 core

in float vBrightness;
out vec4 FragColor;

void main()
{
    vec3 grassColor = vec3(0.4, 0.8, 0.3);
    FragColor = vec4(grassColor * vBrightness, 1.0);
}