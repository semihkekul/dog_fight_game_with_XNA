using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    struct Asteroid
    {
        public Vector3 position;
        public Vector3 direction ;
        public float speed;
        public void Update(float delta)
        {
            position += direction * speed *
                        5.0f * delta;

            if (position.X > 8000)
                position.X -= 2 * 8000;
            if (position.X < -8000)
                position.X += 2 * 8000;
            if (position.Y > 8000)
                position.Y -= 2 * 8000;
            if (position.Y < -8000)
                position.Y += 2 * 8000;
        }
        
    }
}
