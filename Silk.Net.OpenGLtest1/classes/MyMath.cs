using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silk.Net.OpenGLtest1.classes
{
    public class MyMath
    {
        public static float DegToRad(float deg)
        {
            return (deg * MathF.PI/180f)%(2*MathF.PI);
        }
    }
}
