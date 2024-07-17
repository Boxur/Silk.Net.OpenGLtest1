#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
uniform vec2 screenSize;
out vec4 vertexColor;
    
void main() 
{
    vertexColor = vec4(aColor.rgb, 1.0);
    gl_Position = vec4(aPosition.xyzz);
    gl_Position.w+=0.5;
}