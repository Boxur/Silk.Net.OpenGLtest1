#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aColor;
uniform mat4 perspective;
uniform mat4 viewport;
out vec4 vertexColor;
    
void main() 
{
    vertexColor = vec4(aColor.rgb, 1.0);
    vec4 rotated = vec4(aPosition.xyz,1.0);
    gl_Position = perspective*viewport*rotated;

}