using Microsoft.Xna.Framework;
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

        bool hasFailed = false;

        public void initialise(Camera c)
        {
            cam = c;
        }
        
        public void progressGame(){
            cam.setTargetHeight(600 * (stage));
            LevelManager.Instance.AddRandomLevel(600 * (stage));
            stage++;
            foreach(Actor actor in ActorManager.Instance.getAllWithTag("Enemy"))
            {
                EnemyActor enemy = (EnemyActor)actor;
                enemy.canMove = false;
            }
            isProgressing = true;
        }

        public void Update(){
            if (!hasFailed)
            {
                if (ActorManager.Instance.getNumWithTag("Enemy") == 0 && !isProgressing)
                {
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
                if (isProgressing)
                {
                    isProgressing = cam.isMoving;
                    if (!isProgressing)
                    {
                        foreach (Actor actor in ActorManager.Instance.getAllWithTag("Enemy"))
                        {
                            EnemyActor enemy = (EnemyActor)actor;
                            enemy.canMove = true;
                        }
                    }
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    resetGame();
                }
            }
        }

        public void SetGameOver()
        {
            cam.setTargetHeight(0);
            hasFailed = true;
        }

        public void resetGame()
        {
            LevelManager.Instance.reset();
            cam.setHeight(0.0f);
            ActorManager.Instance.reset();
            PhysicsManager.Instance.reset();

            LevelManager.Instance.loadFile("BaseLevel.oel");
            AudioManager.Instance.playMusic();

            UiElement ScoreUI = UiManager.Instance.getUi("Score UI");
            UiElement barFG = UiManager.Instance.getUi("BarFG");
            PlayerActor playerActor = ActorManager.Instance.getFirstWithTag("Player") as PlayerActor;
            playerActor.setBarFGUi(barFG);
            playerActor.setScoreUi(ScoreUI);

            UiElement winUI = UiManager.Instance.getUi("ClearMsg");
            winUI.isVisible = false;
            UiElement goUI = UiManager.Instance.getUi("FailureMsg");
            goUI.isVisible = false;

            /*----- TEMP -----*/
            EnemyActor enemy = ActorManager.Instance.createActor<EnemyActor>("enemy", new Vector2(400, 10)) as EnemyActor;
            enemy.setActor(playerActor);

            hasFailed = false;

        }

        public Camera getCamera() { return cam; }

    }
}
