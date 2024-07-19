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

        private static float time = 0;

        private static MyCamera camera = new(new Vector3(0,0,-3),screenSize);
        public static void Main(params string[] args)
        {
            WindowOptions options = WindowOptions.Default;
            options.Title = "test1";
            options.Size = new Vector2D<int>((int)screenSize.X,(int)screenSize.Y);
            options.WindowState = WindowState.Fullscreen;
            options.Samples = 4;

            window = Window.Create(options);
            screenSize = (Vector2)window.GetFullSize();
            //window.WindowState = WindowState.Fullscreen;

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;

            window.Run();
        }

        private static unsafe void OnLoad()
        {
            input = window.CreateInput();
            gl = GL.GetApi(window);
            gl.Viewport(window.GetFullSize());


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
    
                -1.0f, 1.0f,-1.0f,0.0f,1.0f,0.0f,
                -1.0f,-1.0f,-1.0f,0.0f,0.0f,0.0f,
                 1.0f, 1.0f,-1.0f,1.0f,1.0f,0.0f,
                -1.0f,-1.0f,-1.0f,0.0f,0.0f,0.0f,
                 1.0f, 1.0f,-1.0f,1.0f,1.0f,0.0f,
                 1.0f,-1.0f,-1.0f,1.0f,0.0f,0.0f,

                 1.0f, 1.0f,-1.0f,1.0f,1.0f,0.0f,
                 1.0f,-1.0f,-1.0f,1.0f,0.0f,0.0f,
                 1.0f, 1.0f, 1.0f,1.0f,1.0f,1.0f,
                 1.0f,-1.0f,-1.0f,1.0f,0.0f,0.0f,
                 1.0f, 1.0f, 1.0f,1.0f,1.0f,1.0f,
                 1.0f,-1.0f, 1.0f,1.0f,0.0f,1.0f,

                 1.0f, 1.0f, 1.0f,1.0f,1.0f,1.0f,
                 1.0f,-1.0f, 1.0f,1.0f,0.0f,1.0f,
                -1.0f, 1.0f, 1.0f,0.0f,1.0f,1.0f,
                 1.0f,-1.0f, 1.0f,1.0f,0.0f,1.0f,
                -1.0f, 1.0f, 1.0f,0.0f,1.0f,1.0f,
                -1.0f,-1.0f, 1.0f,0.0f,0.0f,1.0f,

                -1.0f, 1.0f, 1.0f,0.0f,1.0f,1.0f,
                -1.0f,-1.0f, 1.0f,0.0f,0.0f,1.0f,
                -1.0f, 1.0f,-1.0f,0.0f,1.0f,0.0f,
                -1.0f,-1.0f, 1.0f,0.0f,0.0f,1.0f,
                -1.0f, 1.0f,-1.0f,0.0f,1.0f,0.0f,
                -1.0f,-1.0f,-1.0f,0.0f,0.0f,0.0f,




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



        private static void OnUpdate(double deltaTime)
        {
            //no openGL
            time+=(float)deltaTime; 
            Vector3 move = Vector3.Zero;
            if (primaryKeyboard.IsKeyPressed(Key.W)) { move += camera.forward; move.Y = 0; }
            if (primaryKeyboard.IsKeyPressed(Key.S)) { move -= camera.forward; move.Y = 0; }
            if (primaryKeyboard.IsKeyPressed(Key.A)) { move -= camera.right; move.Y = 0; }
            if (primaryKeyboard.IsKeyPressed(Key.D)) { move += camera.right; move.Y = 0; }
            if (primaryKeyboard.IsKeyPressed(Key.Up)) camera.pitch += (float)deltaTime*10;
            if (primaryKeyboard.IsKeyPressed(Key.Down)) camera.pitch -= (float)deltaTime*10;
            if (primaryKeyboard.IsKeyPressed(Key.Left)) camera.yaw -= (float)deltaTime*10;
            if (primaryKeyboard.IsKeyPressed(Key.Right)) camera.yaw += (float)deltaTime*10;
            if (primaryKeyboard.IsKeyPressed(Key.ShiftLeft)) move.Y += 1;
            if (primaryKeyboard.IsKeyPressed(Key.Space)) move.Y -= 1;
            if (primaryKeyboard.IsKeyPressed(Key.Escape)) window.Close();
            if(move!=Vector3.Zero) move = Vector3.Normalize(move)*(float)deltaTime*5;
            camera.position += move;


        }

        private static unsafe void OnRender(double deltaTime)
        {
            gl.Enable(EnableCap.DepthTest);
            gl.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);


            gl.BindVertexArray(vao);       

            shader.setUniform("time", time);
            shader.setUniform("perspective", camera.getPerspectiveMatrix());
            shader.setUniform("viewport", camera.getViewportMatrix());
            shader.setUniform("rotation", Matrix4x4.CreateRotationY(0 * MathF.PI));

            gl.DrawArrays(GLEnum.Triangles, 0, 36);
            gl.BindVertexArray(0);
        
        }

    }
}
