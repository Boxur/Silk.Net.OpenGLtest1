using System;
using System.Numerics;

namespace Silk.Net.OpenGLtest1.classes
{
    public class Camera
    {
        public Vector3 position;
        public Vector2 screenSize;

        public Vector3 forward;
        public Vector3 up;
        public Vector3 right;

        //all in degrees!!!
        public float yaw;
        public float pitch;
        public float fov;

        public Camera(Vector3 position,Vector2 screenSize)
        {
            setValues(position, screenSize,0,90,90);
        }

        public void setValues(Vector3 position,Vector2 screenSize,float yaw,float pitch,float fov)
        {
            this.position=position;
            this.screenSize=screenSize;
            this.yaw=yaw;
            this.pitch=pitch;
            this.fov=fov;
        }

        public Matrix4x4 getViewportMatrix()
        {
            float yawRad = MathHelper.DegToRad(yaw);
            float pitchRad = MathHelper.DegToRad(pitch);

            forward  = new(MathF.Sin(yawRad)*MathF.Sin(pitchRad),MathF.Cos(pitchRad), MathF.Cos(yawRad) * MathF.Sin(pitchRad));
            //pitchRad += ;
            up = new(MathF.Sin(yawRad) * MathF.Sin(pitchRad+ MathF.PI / 2), MathF.Cos(pitchRad + MathF.PI / 2), MathF.Cos(yawRad) * MathF.Sin(pitchRad + MathF.PI / 2));
            right = new(MathF.Sin(yawRad + MathF.PI / 2), 0.0f, MathF.Cos(yawRad + MathF.PI / 2));
            return Matrix4x4.CreateLookAt(position,forward+position,up);
        }

        public Matrix4x4 getPerspectiveMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegToRad(fov), screenSize.X / screenSize.Y, 0.0001f, 100f);
        }
    }
}
