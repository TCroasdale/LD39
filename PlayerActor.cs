using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Collision;

namespace LD39
{
    class PlayerActor : Actor
    {
        public override void Draw(SpriteBatch sb)
        {
            
        }

        public override void initialise()
        {
            setSprite(ArtManager.Instance.getTexture("Player"));
            
            playerBody = BodyFactory.CreateBody(PhysicsManager.Instance.getWorld(), getPosition());
            circleshape = new CircleShape(16f, 1f);
            fixture = playerBody.CreateFixture(circleshape);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Mass = mass;
            PhysicsManager.Instance.registerFixture(fixture, this);

            jumpForce = -55000.0f;// calculateJumpForce();
            tag = "Player";
        }

        public override int Update(float deltaTime)
        {
            float moveHor = 0f;
            bool keyLeft = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool keyRight = Keyboard.GetState().IsKeyDown(Keys.Right);

            int direction = 1;

            if (keyLeft && !keyRight){
                moveHor = -1.0f;
                if(!isAttacking) setSprite(ArtManager.Instance.getTexture("Player_left"));
                direction = 0;
            }else if (keyRight && !keyLeft) {
                moveHor = 1.0f;
                if (!isAttacking) setSprite(ArtManager.Instance.getTexture("Player_right"));
                direction = 2;
            }
            else{
                if (!isAttacking) setSprite(ArtManager.Instance.getTexture("Player"));
                direction = 1;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Up) && isGrounded)
            {
                playerBody.ApplyForce(new Vector2(0, jumpForce));
                currPower -= jumpPowerDecrease;
            }

            if (!isGrounded)
            {
                moveHor *= inAirMoveSpeedModifier;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !isAttacking){
                isAttacking = true;
                canMove = false;
                Vector2 offset = Vector2.Zero;
                if (direction == 0) { offset = new Vector2(12, 20); }
                if (direction == 1) { offset = new Vector2(12, 8); }
                if (direction == 2) { offset = new Vector2(20, 20); }

                FistActor fist = ActorManager.Instance.createActor<FistActor>("Fist", getPosition() + offset) as FistActor;
                fist.setUp(direction, this);
            }


            if (canMove)
            {
                currPower -= Math.Abs(moveHor) * walkPowerDecrease;
                playerBody.LinearVelocity = new Vector2(moveHor * moveSpeed, playerBody.LinearVelocity.Y);
            }
            else
            {
                playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
            }

            bool badPosition = (getPosition().X < 0 || getPosition().X > 800 ||getPosition().Y < 0);
            if(currPower <= 0 || badPosition)
            {
                GameOver();
            }

            if (scoreUi != null){
                scoreUi.setContent(score.ToString());
            }
            UpdateBarUi();

            return 0;
        }

        public override Vector2 getPosition() {
            if (playerBody != null){
                return playerBody.Position - new Vector2(16, 16);
            }
            else{
                return base.getPosition();
            }
        }

        public override bool OnCollision(collision info)
        {
            if (info.other == null)
            {
                if (playerBody.LinearVelocity.Y > 0)//|| (playerBody.LinearVelocity.X > -0.1 && playerBody.LinearVelocity.X < 0.1))
                {
                    isGrounded = true;
                    return true;
                }
                return false;
            }
            else
            {
                if (info.other.tag == "Battery")
                {
                    currPower = maxPower;
                    ActorManager.Instance.deleteActor(info.other);
                    AudioManager.Instance.fireSfx("Battery");
                }
                else if (info.other.tag == "Coin")
                {
                    score += ((CoinActor)info.other).amount;
                    ActorManager.Instance.deleteActor(info.other);
                    AudioManager.Instance.fireSfx("Coin");
                }
                else
                {
                    if (info.other.tag == "Enemy")
                    {
                        currPower -= hitPowerDecrease;
                    }
                }
                return false;
            }
        }

        public override void OnSeparation(collision info)
        {
            if (info.other == null)
            {
                isGrounded = false;
            }
            else
            {
            }
        }

        public void stopAttacking()
        {
            canMove = true;
            isAttacking = false;
        }

        public override void OnDestroy()
        {
            PhysicsManager.Instance.removeAndUnRegister(fixture, playerBody);
        }

        public void setScoreUi(UiElement elem){
            scoreUi = elem;
        }

        public void setBarFGUi(UiElement elem){
            barFG = elem;
            initBarWidth = barFG.getSize().X;
            initLeftPos = barFG.getPosition().X;
        }

        public void UpdateBarUi(){
            float percPower = currPower / maxPower;
            float barWidth = initBarWidth * percPower;
            Vector2 Size = barFG.getSize();
            Size.X = barWidth;
            barFG.setSize(Size);


            Vector2 Pos = barFG.getPosition();
            Pos.X = 800 - barWidth;
            barFG.setPosition(Pos);

            if (percPower < 0.25){ barFG.setColor(Color.Red); }
            else if (percPower < 0.5){ barFG.setColor(Color.MonoGameOrange); }
            else{ barFG.setColor(Color.Blue); }
        }

        public void GameOver()
        {
            AudioManager.Instance.fireSfx("Death");
            UiManager.Instance.getUi("FailureMsg").isVisible = true;
            ActorManager.Instance.deleteActor(this);
        }

        Body playerBody;
        CircleShape circleshape;
        Fixture fixture;

        float moveSpeed = 64.0f;
        float inAirMoveSpeedModifier = 0.75f;
        float jumpForce;
        float mass = 5.0f;

        bool isGrounded;


        float maxPower = 200.0f;
        float currPower = 200.0f;
        float walkPowerDecrease = 0.25f;
        float jumpPowerDecrease = 1f;
        float hitPowerDecrease = 5f;

        int score = 0;

        public bool canMove = true;
        public bool isAttacking = false;


        UiElement scoreUi;
        UiElement barFG;
        float initBarWidth;
        float initLeftPos;
    }
}
