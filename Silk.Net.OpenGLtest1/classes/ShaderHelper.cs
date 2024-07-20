using Silk.NET.OpenGL;
using System;
using System.Numerics;

namespace Silk.Net.OpenGLtest1.classes
{
    internal class ShaderHelper
    {
        private GL gl;
        private uint program;

        public ShaderHelper(GL gl,string vertexShaderPath,string fragmentShaderPath)
        {
            this.gl = gl;
            program = this.gl.CreateProgram();

            uint vShader = generateShader(vertexShaderPath,ShaderType.VertexShader);
            uint fShader = generateShader(fragmentShaderPath,ShaderType.FragmentShader);

            loadShaders(vShader,fShader);
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

        private void loadShaders(uint vShader,uint fShader)
        {
            gl.LinkProgram(program);
            gl.DetachShader(program, vShader);
            gl.DetachShader(program,fShader);
            gl.DeleteShader(vShader);
            gl.DeleteShader(fShader);
            gl.UseProgram(program);
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
