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
        private static MyShader shader;
        private static IKeyboard primaryKeyboard;

        private static uint vao;
        private static uint vbo;

        private static Vector2 screenSize = new(1280,720);
        private static Matrix4x4 perspective;

        private static float time;

        private static MyCamera camera = new(new Vector3(0,0,-3),screenSize);
        public static void Main(params string[] args)
        {
            WindowOptions options = WindowOptions.Default;
            options.Title = "test1";
            options.Size = new Vector2D<int>((int)screenSize.X,(int)screenSize.Y);
            //options.WindowState = WindowState.Fullscreen;
            options.Samples = 4;

            window = Window.Create(options);
            //window.WindowState = WindowState.Fullscreen;

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;

            window.Run();
        }

        private static unsafe void OnLoad()
        {
            input = window.CreateInput();
            gl = window.CreateOpenGL();

            primaryKeyboard = input.Keyboards.FirstOrDefault();

            perspective = camera.getPerspectiveMatrix();             

            gl.ClearColor(0.2f, 0.2f, 0.2f, 0.0f);

            shader = new MyShader(gl, ".\\shaders\\VertexShader.glsl", ".\\shaders\\FragmentShader.glsl");

            vao = gl.GenVertexArray();
            vbo = gl.GenBuffer();

            gl.BindVertexArray(vao);

            gl.BindBuffer(GLEnum.ArrayBuffer, vbo);

            float[] quad =
            {


                -1.0f, 1.0f,1.0f,1.0f,0.0f,1.0f, //top left
                -1.0f,-1.0f,1.0f,0.0f,1.0f,1.0f, //bottom left
                 1.0f, 1.0f,1.0f,1.0f,1.0f,0.0f, //top right
                -1.0f,-1.0f,1.0f,0.0f,1.0f,1.0f, //bottom left
                 1.0f, 1.0f,1.0f,1.0f,1.0f,0.0f, //top right
                 1.0f,-1.0f,1.0f,0.0f,0.0f,0.0f,  //bottom right

                -1.0f, 1.0f,0.0f,0.0f,1.0f,0.0f, //top left
                -1.0f,-1.0f,0.0f,1.0f,0.0f,0.0f, //bottom left
                 1.0f, 1.0f,0.0f,0.0f,0.0f,1.0f, //top right
                -1.0f,-1.0f,0.0f,1.0f,0.0f,0.0f, //bottom left
                 1.0f, 1.0f,0.0f,0.0f,0.0f,1.0f, //top right
                 1.0f,-1.0f,0.0f,1.0f,1.0f,1.0f  //bottom right


                


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



        private static void OnUpdate(double d)
        {
            //no openGL
            time = DateTime.Now.Millisecond;
            if (primaryKeyboard.IsKeyPressed(Key.W)) camera.position.Z += (float)d;
            if (primaryKeyboard.IsKeyPressed(Key.S)) camera.position.Z -= (float)d;
            if (primaryKeyboard.IsKeyPressed(Key.A)) camera.position.X += (float)d;
            if (primaryKeyboard.IsKeyPressed(Key.D)) camera.position.X -= (float)d;
            if (primaryKeyboard.IsKeyPressed(Key.ShiftLeft)) camera.position.Y -= (float)d;
            if (primaryKeyboard.IsKeyPressed(Key.Space)) camera.position.Y += (float)d;

        }

        private static unsafe void OnRender(double d)
        {
            gl.Clear(ClearBufferMask.ColorBufferBit);


            gl.BindVertexArray(vao);

            Matrix4x4 p = camera.getPerspectiveMatrix();
            Matrix4x4 v = camera.getViewportMatrix();
            //v = Matrix4x4.Transpose(v);

            //gl.Uniform1(gl.GetUniformLocation(program, "time"), time);
            shader.setUniform("time", time);
            shader.setUniform("perspective", p);
            shader.setUniform("viewport",v);

            gl.LineWidth(20);
            gl.DrawArrays(GLEnum.Triangles, 0, 12);
            gl.BindVertexArray(0);
        
        }

    }
}
