using System;
using System.Numerics;

namespace Silk.Net.OpenGLtest1.classes
{
    public class Camera
    {
        Vector3 position;

        //all in degrees!!!
        float yaw;
        float pitch;
        public Camera(Vector3 position)
        {
            this.position = position;
            this.yaw = 0.0f;
            this.pitch = 0.0f;
        }

        public Camera(float x, float y, float z)
        {
            this.position = new Vector3(x, y, z);
            this.yaw = 0.0f;
            this.pitch = 0.0f;
        }

        public Camera(Vector3 position, float yaw, float pitch, float roll)
        {
            this.position = position;
            this.yaw = yaw;
            this.pitch = pitch;
        }

        public Matrix4x4 GetViewportMatrix()
        {
            float yawRad = MyMath.DegToRad(yaw);
            float pitchRad = MyMath.DegToRad(pitch);

            Vector3 target = new(MathF.Sin(yawRad)*MathF.Sin(pitchRad)+position.X,MathF.Cos(yawRad)*MathF.Sin(pitchRad)+position.Y,MathF.Cos(pitchRad)+position.Z);
            pitchRad += MathF.PI / 2;
            Vector3 up = new(MathF.Sin(yawRad) * MathF.Sin(pitchRad) + position.X, MathF.Cos(yawRad) * MathF.Sin(pitchRad) + position.Y, MathF.Cos(pitchRad) + position.Z);
            return Matrix4x4.CreateLookAt(position,target,up);
        }
    }
}
