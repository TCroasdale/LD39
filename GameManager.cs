using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD39
{
    class GameManager
    {
        #region SINGLETON IMPL
        private static GameManager instance;

        private GameManager()
        {
            
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }
        #endregion

        private Camera cam;
        private int stage = 1;

        bool canProgress = false;
        bool isProgressing = false;

        public void initialise(Camera c)
        {
            cam = c;
        }
        
        public void progressGame(){
            cam.setTargetHeight(600 * (stage-1));
            LevelManager.Instance.AddRandomLevel(600 * (stage-1));
            stage++;
            foreach(Actor actor in ActorManager.Instance.getAllWithTag("Enemy"))
            {
                EnemyActor enemy = (EnemyActor)actor;
                enemy.canMove = false;
            }
            isProgressing = true;
        }

        public void Update(){
            if(ActorManager.Instance.getNumWithTag("Enemy") == 0 &&!isProgressing){
                canProgress = true;
                UiManager.Instance.getUi("ClearMsg").isVisible = true;
            }
            if (canProgress)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    canProgress = false;
                    UiManager.Instance.getUi("ClearMsg").isVisible = false;
                    progressGame();
                }
            }
            if (isProgressing){
                isProgressing = cam.isMoving;
                if (!isProgressing){
                    foreach (Actor actor in ActorManager.Instance.getAllWithTag("Enemy"))
                    {
                        EnemyActor enemy = (EnemyActor)actor;
                        enemy.canMove = true;
                    }
                }
            }
        }

    }
}
