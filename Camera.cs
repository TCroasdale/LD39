using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD39
{
    class Camera
    {
        Vector2 Position = Vector2.Zero;
        float TargetHeight = 0.0f;
        float scrollSpeed = 32.0f;
        public bool isMoving;

        public void Update(float deltaTime){
            if(TargetHeight > Position.Y + 5.0){
                Position.Y += scrollSpeed * deltaTime;
            }else if(TargetHeight < Position.Y - 5.0){
                Position.Y -= scrollSpeed * deltaTime;
            }
            else
            {
                Position.Y = TargetHeight;
                isMoving = false;
            }
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(new Vector3(Position, 0.0f)) *
                Matrix.CreateRotationZ(0.0f) * Matrix.CreateScale(1.0f, 1.0f, 1.0f) *
                Matrix.CreateTranslation(Vector3.Zero);
        }

        public void setTargetHeight(float height){
            TargetHeight = height;
            isMoving = true;
        }

        public void setHeight(float height)
        {
            TargetHeight = height;
            Position.Y = TargetHeight;
            isMoving = false;
        }

        public void incrementTargetHeight(float delta){
            TargetHeight += delta;
            isMoving = true;
        }

        public float getHeight() { return Position.Y; }

        public bool onScreen(Rectangle bounds){
            Rectangle screen = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point(800, 600));
            return screen.Intersects(bounds);
        }
    }
}
