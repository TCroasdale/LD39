using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Input;

namespace LD39
{
    class PlayerActor : Actor
    {
        public override void Draw(SpriteBatch sb)
        {
            
        }

        public override void initialise()
        {
            setSprite(ArtManager.Instance.getTexture("Player"));
            
            playerBody = BodyFactory.CreateBody(PhysicsManager.Instance.getWorld(), getPosition());
            circleshape = new CircleShape(16f, 5f);
            fixture = playerBody.CreateFixture(circleshape);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Mass = 90.0f;
        }

        public override int Update(float deltaTime)
        {
            float moveHor = 0f;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)){
                moveHor = -1.0f;
            }else if (Keyboard.GetState().IsKeyDown(Keys.Right)) {
                moveHor = 1.0f;
            }

            float currYVelocity = 9.8f * 32.0f;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                currYVelocity = -currYVelocity;
            }

            playerBody.LinearVelocity = new Vector2(moveHor * moveSpeed, currYVelocity);
            return 0;
        }

        public override Vector2 getPosition() {
            if (playerBody != null){
                return playerBody.Position - new Vector2(16, 16);
            }
            else{
                return base.getPosition();
            }
        }

        Body playerBody;
        CircleShape circleshape;
        Fixture fixture;

       float  moveSpeed = 16.0f;
    }
}
