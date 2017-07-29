using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;

namespace LD39
{

    public struct collision
    {
        public Actor other;
        public Vector2 contactPoint;
        public collision(Actor a, Vector2 p)
        {
            other = a;
            contactPoint = p;
        }
    }

    class PhysicsManager
    {
        #region SINGLETON IMPL
        private static PhysicsManager instance;

        private PhysicsManager() { }

        public static PhysicsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PhysicsManager();
                }
                return instance;
            }
        }
        #endregion


        public void initialise()
        {
            world = new World(new Vector2(0f, 98f));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(16f);
            staticColliders = new List<Body>();
            fixtureActorMapping = new Dictionary<Fixture, Actor>();
        }

        public void update(float deltaTime)
        {
            world.Step(deltaTime);
        }

        public void addStaticCollider(int x, int y, int w, int h){
            Body staticBody = BodyFactory.CreateBody(world, new Vector2(x, y));
            Vertices vertices = new Vertices(4);
            vertices.Add(new Vector2(0, h));
            vertices.Add(new Vector2(w, h));
            vertices.Add(new Vector2(w, 0));
            vertices.Add(new Vector2(0, 0));
            // Shape circleshape = new PolygonShape(vertices, 1.0f);
            Shape circleshape = new EdgeShape(new Vector2(0, 0), new Vector2(w, 0));
            Fixture fixture = staticBody.CreateFixture(circleshape);
            staticBody.BodyType = BodyType.Static;
            staticColliders.Add(staticBody);
            //fixture.OnCollision += onStaticColliderCollision;
            Console.WriteLine("Added static collider at: x: {0}, y: {1}, with w: {2} and h:{3}", x, y, w, h);
        }

        public World getWorld() { return world;}

        private World world;
        private List<Body> staticColliders;
        private Dictionary<Fixture, Actor> fixtureActorMapping;

        public void registerFixture(Fixture f, Actor a){
            fixtureActorMapping.Add(f, a);
            f.OnCollision += onActorCollision;
            f.OnSeparation += onActorSeperation;
        }

        public bool onStaticColliderCollision(Fixture f1, Fixture f2, Contact contact){
            Console.WriteLine("Collided with {0}");
            return true;
        }

        public bool onActorCollision(Fixture f1, Fixture f2, Contact contact)
        {
            bool ret1 = true, ret2 = true;
            if (fixtureActorMapping.ContainsKey(f1))
            {
                Actor other = null;
                Vector2 pos = Vector2.Zero;
                if (fixtureActorMapping.ContainsKey(f2)){
                    other = fixtureActorMapping[f2];
                }
                else
                {
                    ret2 = false;
                }

                ret1 = fixtureActorMapping[f1].OnCollision(new collision(other, pos));
            }
            if (fixtureActorMapping.ContainsKey(f2))
            {
                Actor other = null;
                if (fixtureActorMapping.ContainsKey(f1))
                {
                    other = fixtureActorMapping[f1];
                }
                else
                {
                    ret1 = false;
                }
                ret2 = fixtureActorMapping[f2].OnCollision(new collision(other, Vector2.Zero));
            }

            return ret1 || ret2;
        }

        public void onActorSeperation(Fixture f1, Fixture f2)
        {
            if (fixtureActorMapping.ContainsKey(f1))
            {
                Actor other = null;
                if (fixtureActorMapping.ContainsKey(f2))
                    other = fixtureActorMapping[f2];
                fixtureActorMapping[f1].OnSeparation(new collision(other, Vector2.Zero));
            }
            if (fixtureActorMapping.ContainsKey(f2))
            {
                Actor other = null;
                if (fixtureActorMapping.ContainsKey(f1))
                    other = fixtureActorMapping[f1];
                fixtureActorMapping[f2].OnSeparation(new collision(other, Vector2.Zero));
            }
        }

        public void debugDraw(SpriteBatch batch)
        {
            foreach(Body body in world.BodyList)
            {
                batch.Draw(ArtManager.Instance.getTexture("Player"), body.Position, Color.Blue); 
            }
        }

    }
}
