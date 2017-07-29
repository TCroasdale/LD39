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
            world = new World(new Vector2(0f, 9.8f));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(16f);
            staticColliders = new List<Body>();
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
            Shape circleshape = new PolygonShape(vertices, 1.0f);
            Fixture fixture = staticBody.CreateFixture(circleshape);
            staticBody.BodyType = BodyType.Static;
            staticColliders.Add(staticBody);
            fixture.OnCollision += onStaticColliderCollision;
            Console.WriteLine("Added static collider at: x: {0}, y: {1}, with w: {2} and h:{3}", x, y, w, h);
        }

        public World getWorld() { return world;}

        private World world;
        private List<Body> staticColliders;

        public bool onStaticColliderCollision(Fixture f1, Fixture f2, Contact contact){
            Console.WriteLine("Collided with {0}");
            return true;
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
