﻿using System;
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
    class CoinActor : Actor
    {
        public override void Draw(SpriteBatch sb)
        {
            
        }

        public override void initialise()
        {
            setSprite(ArtManager.Instance.getTexture("SilverCoin"));

            body = BodyFactory.CreateBody(PhysicsManager.Instance.getWorld(), getPosition());
            circleshape = new CircleShape(8f, 1f);
            fixture = body.CreateFixture(circleshape);
            body.BodyType = BodyType.Static;
            body.Mass = 1.0f;
            PhysicsManager.Instance.registerFixture(fixture, this);

            tag = "Coin";
            setSize(new Vector2(12, 12));
        }

        public override void OnDestroy()
        {
            PhysicsManager.Instance.removeAndUnRegister(fixture, body);
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
            return 0;
        }

        Body body;
        Shape circleshape;
        Fixture fixture;

        public int amount = 50;
    }
}
