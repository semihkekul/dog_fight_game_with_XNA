using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame1
{
    class Viper
    {
        // Set the 3D model to draw.
        public Model model;
  
        public Matrix RotationMatrix = Matrix.Identity;
        public Vector3 position = new Vector3(.0f,.0f, -10000.0f);
        public Matrix[] transforms;
        // Set the velocity of the model, applied each frame to the model's position.
        public Vector3 velocity = Vector3.Zero;

        public bool loadModel()
        {
            // Copy any parent transforms.
            transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            return true;
        }
        private float rotation = .0f;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.PiOver2)
                {
                    newVal = rotation;
                }
                while (newVal <= -MathHelper.PiOver2)
                {
                    newVal = rotation;
                }

                if (rotation != newVal)
                {
                    rotation = newVal;
                    RotationMatrix = Matrix.CreateRotationY(rotation);
                }

            }
        }
        private float pitch = .0f;
        public float Pitch
        {
            get { return pitch; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.PiOver4)
                {
                    newVal = pitch;
                }
                while (newVal <= -MathHelper.PiOver4)
                {
                    newVal = pitch;
                }

                if (pitch != newVal)
                {
                    pitch = newVal;
                    RotationMatrix = Matrix.CreateRotationX(pitch);
                }

            }
        }
        public void Update(KeyboardState key)
        {
            if (key.IsKeyDown(Keys.Left))
            {
                Rotation += 0.10f;
            }
            if (key.IsKeyDown(Keys.Right))
            {
                Rotation -= 0.10f;
            }

            if (key.IsKeyDown(Keys.Up))
            {
                velocity -= RotationMatrix.Forward*2;

            }

            if (key.IsKeyDown(Keys.W))
            {
                Pitch += 0.10f; ;
            }
            if (key.IsKeyDown(Keys.S))
            {
                Pitch -= 0.10f; ;
            }
            
            
        }

    }
}
