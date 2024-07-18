using Silk.NET.Maths;
using System;
using System.Numerics;

namespace Silk.Net.OpenGLtest1.classes
{
    public class MyCamera
    {
        public Vector3 position;
        public Vector2 screenSize;

        //all in degrees!!!
        public float yaw;
        public float pitch;
        public float fov;

        public MyCamera(Vector3 position,Vector2 screenSize)
        {
            setValues(position, screenSize,0,0,90);
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
            float yawRad = MyMath.DegToRad(yaw);
            float pitchRad = MyMath.DegToRad(pitch);

            Vector3 target  = new(MathF.Sin(yawRad)*MathF.Sin(pitchRad)+position.X,MathF.Cos(yawRad)*MathF.Sin(pitchRad)+position.Y,MathF.Cos(pitchRad)+position.Z);
            pitchRad += MathF.PI / 2;
            Vector3 up = new(MathF.Sin(yawRad) * MathF.Sin(pitchRad), MathF.Cos(yawRad) * MathF.Sin(pitchRad), MathF.Cos(pitchRad));
            return Matrix4x4.CreateLookAt(position,target,up);
        }

        public Matrix4x4 getPerspectiveMatrix()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(MyMath.DegToRad(fov), screenSize.X / screenSize.Y, 0.1f, 100f);
        }
    }
}
