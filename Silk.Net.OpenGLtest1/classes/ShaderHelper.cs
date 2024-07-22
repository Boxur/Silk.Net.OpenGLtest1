using Silk.NET.OpenGL;
using System;
using System.Numerics;

namespace Silk.Net.OpenGLtest1.classes
{
    internal class ShaderHelper
    {
        private GL gl;
        private uint program;

        public ShaderHelper(GL gl,string[] paths, ShaderType[] types)
        {
            this.gl = gl;
            program = this.gl.CreateProgram();

            if (paths.Length != types.Length) throw new ArgumentException("paths hould have as many items as types");
            uint[] shaders = new uint[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                shaders[i] = generateShader(paths[i], types[i]);

            }           
            
            gl.LinkProgram(program);
            gl.UseProgram(program);
            for (int i = 0; i < types.Length; i++)
            {
                gl.DetachShader(program, shaders[i]);
                gl.DeleteShader(shaders[i]);
            }



        }

        private uint generateShader(string filePath, ShaderType shaderType)
        {
            uint current = gl.CreateShader(shaderType);
            gl.ShaderSource(current, File.ReadAllText(filePath));
            gl.CompileShader(current);

            string infoLog = gl.GetShaderInfoLog(current);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine($"Error compiling {shaderType} {infoLog}");
            }

            gl.AttachShader(program, current);

            return current;
        }

        public void setUniform(string name, int value)
        {
            int location = gl.GetUniformLocation(program, name);
            if(location ==-1) throw new Exception($"{name} uniform not found on shader.");
            gl.Uniform1(location, value);
        }

        public void setUniform(string name, float value)
        {
            int location = gl.GetUniformLocation(program, name);
            //if (location == -1) throw new Exception($"{name} uniform not found on shader.");
            gl.Uniform1(location, value);
        }

        public unsafe void setUniform(string name, Matrix4x4 value)
        {
            int location = gl.GetUniformLocation(program, name);
            //if (location == -1) throw new Exception($"{name} uniform not found on shader.");
            gl.UniformMatrix4(location,1, false, (float*) &value);
        }

        public void Dispose()
        {
            gl.DeleteProgram(program);
        }


    }
}
