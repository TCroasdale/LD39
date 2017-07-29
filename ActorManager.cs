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

        public ActorManager(){
            actors = new Dictionary<string, Actor>();
        }

        public Dictionary<string, Actor> actors;

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
                actor.Value.Update(deltaTime);
                //If the actor update returns non zero, delete it.
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (KeyValuePair<string, Actor> actor in actors)
            {
                sb.Draw(actor.Value.getSprite(), actor.Value.getPosition(), Color.White);
                actor.Value.Draw(sb);
            }
        }

        //TODO:
        //deletion
    }
}
