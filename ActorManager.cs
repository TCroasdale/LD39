using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD39
{
    class ActorManager
    {

        #region SINGLETON IMPL
        private static ActorManager instance;

        private ActorManager() {
            actors = new List<Actor>();
            actorsToAdd = new List<Actor>();
            actorsToDelete = new List<Actor>();
        }

        public static ActorManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorManager();
                }
                return instance;
            }
        }
        #endregion

        public void reset()
        {
            actors.Clear();
            actorsToAdd.Clear();
            actorsToDelete.Clear();
        }
        
        public Actor createActor<T>(string name, Vector2 position) where T : Actor{
            Actor newActor = (T)Activator.CreateInstance(typeof(T));

            newActor.setPosition(position);
            newActor.initialise();
            actorsToAdd.Add(newActor);
            newActor.id = currID;
            currID++;
            return newActor;
        }

        public void Update(float deltaTime){
            foreach (Actor actor in actors){
                int status = actor.Update(deltaTime);
                //If the actor update returns non zero, delete it.
                if(status != 0){
                    actorsToDelete.Add(actor);
                }
            }

            foreach (Actor actor in actorsToDelete)
            {
                actor.OnDestroy();
                actors.Remove(actor);
            }
            actorsToDelete.Clear();

            foreach (Actor actor in actorsToAdd)
            {
                actors.Add(actor);
            }
            actorsToAdd.Clear();
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Actor actor in actors)
            {
                actor.Draw(sb);
                sb.Draw(actor.getSprite(), actor.getPosition(), Color.White);
            }
        }

        public void deleteActor(Actor actor){
            if (actors.Contains(actor))
            {
                actorsToDelete.Add(actor);
            }
        }


        public Actor getFirstWithTag(string tag)
        {
            foreach (Actor actor in actors)
            {
                if (actor.tag == tag)
                {
                    return actor;
                }
            }
            foreach (Actor actor in actorsToAdd)
            {
                if (actor.tag == tag)
                {
                    return actor;
                }
            }
            return null;
        }

        public int getNumWithTag(string tag)
        {
            int ret = 0;
            foreach (Actor actor in actors)
            {
                if (actor.tag == tag)
                {
                    ret++;
                }
            }
            return ret;
        }

        public List<Actor> getAllWithTag(string tag)
        {
            List<Actor> list = new List<Actor>();
            foreach (Actor actor in actors)
            {
                if (actor.tag == tag)
                {
                    list.Add(actor);
                }
            }
            foreach (Actor actor in actorsToAdd)
            {
                if (actor.tag == tag)
                {
                    list.Add(actor);
                }
            }
            return list;
        }


        private List<Actor> actors;
        private List<Actor> actorsToAdd;
        private List<Actor> actorsToDelete;

        int currID = 0;
    }
}
