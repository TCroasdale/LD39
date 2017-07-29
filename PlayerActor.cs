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
            circleshape = new CircleShape(2f, 5f);
            fixture = playerBody.CreateFixture(circleshape);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Mass = 90.0f;
        }

        public override int Update(float deltaTime)
        {

            return 0;
        }

        public override Vector2 getPosition() {
            if (playerBody != null){
                Console.WriteLine(playerBody.Position);
                return playerBody.Position;
            }
            else{
                return base.getPosition();
            }
        }

        Body playerBody;
        CircleShape circleshape;
        Fixture fixture;
    }
}
