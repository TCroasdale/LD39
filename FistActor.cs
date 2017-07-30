using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;

namespace LD39
{
    class FistActor : Actor
    {
        public override void Draw(SpriteBatch sb)
        {
            Rectangle destRect;
            Texture2D tex = ArtManager.Instance.getTexture("Player_Arm");
            if (Direction == 0){
                destRect = new Rectangle((int)getPosition().X, (int)getPosition().Y, (int)currDist, 8);
            }else if(Direction == 2){
                destRect = new Rectangle((int)originalPos.X, (int)getPosition().Y, (int)currDist, 8);
            }else{
                destRect = new Rectangle((int)getPosition().X, (int)getPosition().Y, 8, (int)currDist);
                tex = ArtManager.Instance.getTexture("Player_Arm_Vert");
            }
            sb.Draw(tex, destRect, Color.White);
        }

        public override void initialise()
        {
            setSprite(ArtManager.Instance.getTexture("Player_Fist"));

            body = BodyFactory.CreateBody(PhysicsManager.Instance.getWorld(), getPosition());
            circleshape = new CircleShape(5f, 1f);
            fixture = body.CreateFixture(circleshape);
            body.BodyType = BodyType.Kinematic;
            body.Mass = 1.0f;
            PhysicsManager.Instance.registerFixture(fixture, this);

            tag = "Fist";
        }

        public override bool OnCollision(collision info)
        {

            return false;
        }

        public override void OnSeparation(collision info)
        {
            
        }

        public override int Update(float deltaTime)
        {
            if (Direction == 0)
            {
                body.LinearVelocity = new Vector2(-moveSpeed, 0);
            }
            else if (Direction == 2)
            {
                body.LinearVelocity = new Vector2(moveSpeed, 0);
            }
            else
            {
                body.LinearVelocity = new Vector2(0, -moveSpeed);
            }

            currDist = (originalPos - getPosition()).Length();
            if(currDist > maxDist){
                ((PlayerActor)owner).stopAttacking();
                return 1;
            }
           
            return 0;
        }

        public void setUp(int dir, Actor a){
            originalPos = getPosition();
            Direction = dir;
            if(Direction == 1)
            {
                setSprite(ArtManager.Instance.getTexture("Player_Fist_Vert"));
            }
            if(Direction == 2)
            {
                setSprite(ArtManager.Instance.getTexture("Player_Fist_right"));
            }
            owner = a;
        }

        public override Vector2 getPosition()
        {
            if (body != null)
            {
                return body.Position - new Vector2(4, 4);
            }
            else
            {
                return base.getPosition();
            }
        }

        public override void OnDestroy()
        {
            PhysicsManager.Instance.removeAndUnRegister(fixture, body);
        }

        private Vector2 originalPos;
        private int Direction; //0=left, 1= up, 2 = right
        private Actor owner;

        private float moveSpeed = 256.0f;
        private float maxDist = 72.0f;
        private float currDist = 0.0f;

        Body body;
        Shape circleshape;
        Fixture fixture;
    }
}
