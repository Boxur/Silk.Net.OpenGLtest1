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
        private static ShaderHelper shader;

        private static IKeyboard primaryKeyboard;
        private static IMouse primaryMouse;

        private static uint vao;
        private static uint vbo;

        private static Vector2 screenSize = new(1280,720);
        private static Matrix4x4 perspective;

        private static float time = 0;


        private static Player player = new Player();
        public static void Main(params string[] args)
        {
            WindowOptions options = WindowOptions.Default;
            options.Title = "test1";
            options.Size = new Vector2D<int>((int)screenSize.X,(int)screenSize.Y);
            options.WindowState = WindowState.Fullscreen;
            options.Samples = 4;

            window = Window.Create(options);

            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.Position = new Vector2D<int>(0,0);

            window.Run();
        }

        private static unsafe void OnLoad()
        {
            player.camera = new Camera(new Vector3(0, 0, -3), screenSize);
            gl = GL.GetApi(window);
            gl.Viewport(window.GetFullSize());
            screenSize = (Vector2)window.GetFullSize();

            input = window.CreateInput();
            primaryKeyboard = input.Keyboards.FirstOrDefault();
            primaryMouse = input.Mice.FirstOrDefault();
            primaryMouse.Cursor.CursorMode = CursorMode.Hidden;
            primaryMouse.Position = screenSize / 2;

            gl.ClearColor(0.2f, 0.2f, 0.2f, 0.0f);

            shader = new ShaderHelper(gl, ".\\shaders\\VertexShader.glsl",".\\shaders\\GeometryShader.glsl", ".\\shaders\\FragmentShader.glsl");
            
            int[] blocks =
            {

                 0,0,0,1,0,0,
                 1,0,0,0,1,0,

            };

            vao = gl.GenVertexArray();
            gl.BindVertexArray(vao);

            vbo = gl.GenBuffer();
            gl.BindBuffer(GLEnum.ArrayBuffer, vbo);
            fixed (void* p = &blocks[0])
            {
                gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(int) * blocks.Length), p, GLEnum.StaticDraw);
            }


            gl.VertexAttribPointer(0, 3, GLEnum.Int, false, 6 * sizeof(int), 0);
            gl.EnableVertexAttribArray(0);

            gl.VertexAttribPointer(1, 3, GLEnum.Int, false, 6 * sizeof(int), 3 * sizeof(int));
            gl.EnableVertexAttribArray(1);

            gl.BindBuffer(GLEnum.ArrayBuffer, 0);
            gl.BindVertexArray(0);

            gl.PointSize(10.0f);
        }


        private static void OnUpdate(double deltaTime)
        {
            //no openGL
            time+=(float)deltaTime;

            player.HandleMouseEvents(primaryMouse.Position, screenSize);
            player.HandleKeybdEvents(primaryKeyboard, deltaTime,window);

            primaryMouse.Position = screenSize / 2;

        }

        private static unsafe void OnRender(double deltaTime)
        {
            gl.Enable(EnableCap.DepthTest);
            gl.Clear(ClearBufferMask.ColorBufferBit|ClearBufferMask.DepthBufferBit);

            gl.BindVertexArray(vao);       

            shader.setUniform("time", time);
            shader.setUniform("perspective", player.camera.getPerspectiveMatrix());
            shader.setUniform("projection", player.camera.getViewportMatrix());

            gl.DrawArrays(GLEnum.Points, 0, 2);
            gl.BindVertexArray(0);
        
        }

    }
}
