using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics;

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
        }

        public void update(float deltaTime)
        {
            world.Step(deltaTime);
        }

        public World getWorld() { return world; }

        private World world;

    }
}
