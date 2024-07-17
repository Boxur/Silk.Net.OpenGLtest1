using System;
using System.IO;
using System.Numerics;
using Silk.Net.OpenGLtest1.classes;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Silk.Net.OpenGLtest1
{
    class Program
    {
        private static IWindow window;
        private static IInputContext input;
        private static GL gl;
        private static uint program;

        private static uint vao;
        private static uint vbo;

        private static Vector2 screenSize = new(1280,720);
        private static Matrix4X4<float> perspective;

        private static float time;

        private static Camera camera = new(1,1,0);
        public static void Main(params string[] args)
        {
            WindowOptions options = WindowOptions.Default;
            options.Title = "test1";
            options.Size = new Vector2D<int>((int)screenSize.X,(int)screenSize.Y);
            options.Samples = 4;

            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;

            window.Run();
        }

        private static unsafe void OnLoad()
        {
            input = window.CreateInput();
            gl = window.CreateOpenGL();

            perspective = Matrix4X4.CreatePerspectiveFieldOfView<float>( MyMath.DegToRad(90.0f), screenSize.X / screenSize.Y, 0.1f, 100f); 
            Console.WriteLine(camera.GetViewportMatrix());
            

            foreach (IMouse mouse in input.Mice) 
            {
                mouse.Click += (IMouse cursor,MouseButton button,System.Numerics.Vector2 pos) => { Console.WriteLine("clcicked"); };
            }
            gl.ClearColor(0.2f, 0.2f, 0.2f, 0.0f);
            program = gl.CreateProgram();
            

            Shader vshader = GenerateShader("C:\\Users\\Burek\\source\\repos\\Silk.Net.OpenGLtest1\\Silk.Net.OpenGLtest1\\shaders\\VertexShader.glsl",
                                            ShaderType.VertexShader);
            Shader fshader = GenerateShader("C:\\Users\\Burek\\source\\repos\\Silk.Net.OpenGLtest1\\Silk.Net.OpenGLtest1\\shaders\\FragmentShader.glsl",
                                            ShaderType.FragmentShader);

            LoadShaders(vshader, fshader);

            vao = gl.GenVertexArray();
            vbo = gl.GenBuffer();

            gl.BindVertexArray(vao);

            gl.BindBuffer(GLEnum.ArrayBuffer, vbo);

            float[] quad =
            {
                -0.5f, 0.5f,0.0f,0.0f,1.0f,0.0f, //top left
                -0.5f,-0.5f,0.0f,1.0f,0.0f,0.0f, //bottom left
                 0.5f, 0.5f,0.0f,0.0f,0.0f,1.0f, //top right
                 0.5f,-0.5f,0.0f,1.0f,1.0f,1.0f  //bottom right

            };


            fixed (float* pQuad = &quad[0])
            {
                gl.BufferData(GLEnum.ArrayBuffer, sizeof(float) * (nuint)quad.Length, pQuad, GLEnum.StaticDraw);
            }

            gl.VertexAttribPointer(0, 3, GLEnum.Float, false, 6 * sizeof(float), 0);
            gl.EnableVertexAttribArray(0);

            gl.VertexAttribPointer(1, 3, GLEnum.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            gl.EnableVertexAttribArray(1);

            gl.BindBuffer(GLEnum.ArrayBuffer, 0);
            gl.BindVertexArray(0);


        }

     

        private static Shader GenerateShader(string filePath, ShaderType type)
        {
            uint shader = gl.CreateShader(type);
            gl.ShaderSource(shader,File.ReadAllText(filePath));
            gl.CompileShader(shader);

            string infoLog = gl.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine($"Error compiling {type} {infoLog}");
            }

            gl.AttachShader(program, shader);

            return new Shader { id = shader };

        }

        private static unsafe void LoadShaders(Shader vertexShader, Shader fragmentShader)
        {

            gl.LinkProgram(program);
            gl.DetachShader(program, vertexShader.id);
            gl.DetachShader(program, fragmentShader.id);
            gl.DeleteShader(vertexShader.id);
            gl.DeleteShader(fragmentShader.id);
            gl.UseProgram(program);
            gl.Uniform2(gl.GetUniformLocation(program, "screenSize"), ref screenSize);

        }


        private static void OnUpdate(double d)
        {
            //no openGL
            time = DateTime.Now.Millisecond;
        }

        private static unsafe void OnRender(double d)
        {
            gl.Clear(ClearBufferMask.ColorBufferBit);


            gl.BindVertexArray(vao);
        
            
            gl.Uniform1(gl.GetUniformLocation(program, "time"), time);

            gl.LineWidth(20);
            gl.DrawArrays(GLEnum.TriangleStrip, 0, 4);
            gl.BindVertexArray(0);
        
        }
    }

    public struct Shader
    {
        public uint id;
    }
}
