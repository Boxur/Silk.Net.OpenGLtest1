#version 330

layout(points) in;
layout(triangle_strip,max_vertices=14) out;

uniform mat4 perspective;
uniform mat4 projection;

out vec3 color;

in DATA
{
	vec3 color;

} data_in[];

void makeACube(ivec3 pos)
{
	color = data_in[0].color;
	gl_Position = perspective*projection*(vec4(pos+vec3(0.,0.,0.),1.));		//000
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(1.,0.,0.),1.));		//100
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(0.,1.,0.),1.));		//010
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(1.,1.,0.),1.));		//110
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(1.,1.,1.),1.));		//111
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(1.,0.,0.),1.));		//100
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(1.,0.,1.),1.));		//101
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(0.,0.,0.),1.));		//000
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(0.,0.,1.),1.));		//001
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(0.,1.,0.),1.));		//010
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(0.,1.,1.),1.));		//011
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(1.,1.,1.),1.));		//111
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(0.,0.,1.),1.));		//001
	gl_Position.y*=-1;
	EmitVertex();

	gl_Position = perspective*projection*(vec4(pos+vec3(1.,0.,1.),1.));		//101
	gl_Position.y*=-1;
	EmitVertex();

	EndPrimitive();
}

void main()
{
	ivec3 pos = ivec3(gl_in[0].gl_Position);
	makeACube(pos);
}