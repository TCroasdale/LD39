using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace LD39
{
    class EnemyActor : Actor
    {
        public override void Draw(SpriteBatch sb)
        {
           
        }

        public override void initialise()
        {
            setSprite(ArtManager.Instance.getTexture("DroneLeft"));

            body = BodyFactory.CreateBody(PhysicsManager.Instance.getWorld(), getPosition());
            circleshape = new CircleShape(24f, 1f);
            fixture = body.CreateFixture(circleshape);
            body.BodyType = BodyType.Dynamic;
            body.Mass = 1.0f;
            PhysicsManager.Instance.registerFixture(fixture, this);

            tag = "Enemy";
            setSize(new Vector2(48, 48));
        }

        public override bool OnCollision(collision info)
        {
            if (info.other == null) return false;

            if (info.other.tag == "Fist" && canTakeDamage)
            {
                canTakeDamage = false;
                AudioManager.Instance.fireSfx("Hit");
                hp--;
                if (hp == 0){
                    ActorManager.Instance.deleteActor(this);
                }
                isHit = true;
                knockbackDir = getPosition() - info.other.getPosition();
                canMove = true;
                return true;
            }
            else if(info.other.tag == "Player")
            {
                isHit = true;
                knockbackDir = getPosition() - info.other.getPosition();
            }

            
            return false;
        }

        public override void OnDestroy()
        {
            PhysicsManager.Instance.removeAndUnRegister(fixture, body);
        }

        public override void OnSeparation(collision info)
        {
            
        }

        public override int Update(float deltaTime)
        {
            Vector2 targPos = targ.getPosition();
            if (targPos.X < getPosition().X) setSprite(ArtManager.Instance.getTexture("DroneLeft"));
            if (targPos.X > getPosition().X) setSprite(ArtManager.Instance.getTexture("DroneRight"));
            body.LinearVelocity = Vector2.Zero;
            if (!isHit)
            {
                if (canMove)
                {
                    Vector2 diff = targPos - getPosition();
                    if (diff.Length() > 16)
                    {
                        diff.Normalize();
                        diff *= moveSpeed;
                        body.LinearVelocity = diff;
                    }
                }
            }
            else
            {
                hitTimer -= deltaTime;
                if(hitTimer <= 0){
                    isHit = false;
                    hitTimer = knockBackTime;
                }

                float percKnock = hitTimer / knockBackTime;

                knockbackDir.Normalize();
                knockbackDir *= knockBackSpeed * percKnock;
                body.LinearVelocity = knockbackDir;
            }

            if (!canTakeDamage){
                dmgTimer -= deltaTime;
                if(dmgTimer <= 0)
                {
                    dmgTimer = dmgTimeout;
                    canTakeDamage = true;
                }
            }

            return 0;
        }

        public void setActor(Actor act) { targ = act; }

        public override Vector2 getPosition(){
            if (body != null){
                return body.Position - getSize() / 2;
            }
            else{
                return base.getPosition();
            }
        }

        Body body;
        Shape circleshape;
        Fixture fixture;

        float moveSpeed = 64.0f;
        Actor targ;

        float knockBackSpeed = 128.0f;
        float hitTimer = 1.5f;
        float knockBackTime = 1.5f;
        bool isHit = false;
        Vector2 knockbackDir;
        int hp = 2;


        bool canTakeDamage = true;
        float dmgTimeout = 0.5f;
        float dmgTimer = 0.5f;

        public bool canMove = true;
    }
}
