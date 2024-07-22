#version 410 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in float blockID;

uniform mat4 perspective;
uniform mat4 projection;

out DATA
{
	int blockID;
} data_out;
    
void main() 
{
    gl_Position = vec4(aPosition.xyz,1.0);
    data_out.blockID = int(blockID);

}