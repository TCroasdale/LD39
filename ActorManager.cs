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
            actors = new Dictionary<string, Actor>();
            actorsToAdd = new Dictionary<string, Actor>();
            actorsToDelete = new List<string>();
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

        public Actor createActor<T>(string name, Vector2 position) where T : Actor{
            Actor newActor = (T)Activator.CreateInstance(typeof(T));

            newActor.setPosition(position);
            newActor.initialise();
            while(actors.ContainsKey(name)){
                name += "_";
            }
            actors.Add(name, newActor);

            return newActor;
        }

        public void Update(float deltaTime){
            foreach (KeyValuePair<string, Actor> actor in actors){
                int status = actor.Value.Update(deltaTime);
                //If the actor update returns non zero, delete it.
                if(status != 0){
                    actorsToDelete.Add(actor.Key);
                }
            }

            foreach (string actor in actorsToDelete)
            {
                actors.Remove(actor);
            }
            actorsToDelete.Clear();

            foreach (KeyValuePair<string, Actor> actor in actorsToAdd)
            {
                actors.Add(actor.Key, actor.Value);
            }
            actorsToAdd.Clear();
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (KeyValuePair<string, Actor> actor in actors)
            {
                sb.Draw(actor.Value.getSprite(), actor.Value.getPosition(), Color.White);
                actor.Value.Draw(sb);
            }
        }

        public void deleteActor(Actor actor){
            if (actors.ContainsValue(actor))
            {
                actorsToDelete.Add(getKeyForActor(actor));
            }
        }

        public string getKeyForActor(Actor actor)
        {
            foreach(KeyValuePair<string, Actor> actor2 in actors)
            {
                if(actor == actor2.Value)
                {
                    return actor2.Key;
                }
            }
            return "NONE";
        }

        private Dictionary<string, Actor> actors;
        private Dictionary<string, Actor> actorsToAdd;
        private List<string> actorsToDelete;
    }
}
