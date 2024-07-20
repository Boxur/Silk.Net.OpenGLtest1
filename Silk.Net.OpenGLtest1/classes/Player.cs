using Silk.Net.OpenGLtest1.interfaces;
using Silk.NET.Input;
using Silk.NET.Windowing;
using System.Numerics;

namespace Silk.Net.OpenGLtest1.classes
{
    public class Player : IEntity
    {
        private int maxHealth = 20;
        private int health = 20;

        public Camera camera;
        public Vector2 mouseSensitivity = new(0.1f, 0.1f);

        public void HandleMouseEvents(Vector2 mousePos, Vector2 screenSize)
        {

            this.camera.pitch += (mousePos.Y - screenSize.Y / 2) * -mouseSensitivity.Y;
            this.camera.yaw += (mousePos.X - screenSize.X / 2) * mouseSensitivity.X;
            if (this.camera.pitch < 0.0001f) this.camera.pitch = 0.0001f;
            if (this.camera.pitch > 179.9999f) this.camera.pitch = 179.9999f;


        }

        public void HandleKeybdEvents(IKeyboard primaryKeyboard, double deltaTime,IWindow window)
        {
            Vector3 move = Vector3.Zero;
            if (primaryKeyboard.IsKeyPressed(Key.W)) move += this.camera.forward;
            if (primaryKeyboard.IsKeyPressed(Key.S)) move -= this.camera.forward;
            move.Y = 0;
            if (move != Vector3.Zero) move = Vector3.Normalize(move);
            if (primaryKeyboard.IsKeyPressed(Key.A)) move -= this.camera.right;
            if (primaryKeyboard.IsKeyPressed(Key.D)) move += this.camera.right;
            move.Y = 0;
            if (move != Vector3.Zero) move = Vector3.Normalize(move);
            if (primaryKeyboard.IsKeyPressed(Key.ShiftLeft)) move.Y += 1;
            if (primaryKeyboard.IsKeyPressed(Key.Space)) move.Y -= 1;
            if (primaryKeyboard.IsKeyPressed(Key.Escape)) window.Close();
            if (move != Vector3.Zero) move = Vector3.Normalize(move) * (float)deltaTime * 3;
            this.camera.position += move;
        }

        public int MaxHealth 
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
    }
}
