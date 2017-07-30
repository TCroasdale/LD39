﻿using System;
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
                setSprite(ArtManager.Instance.getTexture("Player_left"));
                direction = 0;
            }else if (keyRight && !keyLeft) {
                moveHor = 1.0f;
                setSprite(ArtManager.Instance.getTexture("Player_right"));
                direction = 2;
            }
            else{
                setSprite(ArtManager.Instance.getTexture("Player"));
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
                FistActor fist = ActorManager.Instance.createActor<FistActor>("Fist", getPosition() + new Vector2(0, 8)) as FistActor;
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
            Console.WriteLine("CurrentPower: {0}", currPower);
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
                if (playerBody.LinearVelocity.Y > 0 || (playerBody.LinearVelocity.X > -0.1 && playerBody.LinearVelocity.X < 0.1))
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
                }
                else if (info.other.tag == "Coin")
                {
                    score += ((CoinActor)info.other).amount;
                    ActorManager.Instance.deleteActor(info.other);
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

        Body playerBody;
        CircleShape circleshape;
        Fixture fixture;

        float moveSpeed = 64.0f;
        float inAirMoveSpeedModifier = 0.75f;
        float jumpForce;
        float mass = 5.0f;

        bool isGrounded;


        float maxPower = 150.0f;
        float currPower = 100.0f;
        float walkPowerDecrease = 0.25f;
        float jumpPowerDecrease = 1f;

        int score = 0;

        public bool canMove = true;
        public bool isAttacking = false;
    }
}
