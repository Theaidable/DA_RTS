using DA_RTS.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;

namespace Test.Classes.Units
{
    public class Miner : Unit
    {
        private int frameWidth;
        private int frameHeight;
        private int currentFrame;
        private int totalFrames;
        private int totalRows;
        private float timePerFrame;
        private float elapsedTime;

        private enum MinerState { MovingToMine, Mining, Returning }
        private MinerState currentState;

        private Vector2 targetMinePosition;
        private Vector2 townHallPosition;

        private int goldCarried;
        private int goldCapacity;

        public event EventHandler<int> GoldDelivered;

        public Miner(Vector2 startPosition, Texture2D texture, float speed, Vector2 targetMine, Vector2 townHall) : base(startPosition, texture, speed)
        {
            targetMinePosition = targetMine;
            townHallPosition = townHall;

            currentState = MinerState.MovingToMine;

            goldCarried = 0;
            goldCapacity = 50;

            totalFrames = 6;
            totalRows = 6;
            currentFrame = 0;
            elapsedTime = 0f;
            timePerFrame = 0.2f;

            frameWidth = texture.Width / totalFrames;
            frameHeight = texture.Height / totalRows;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            elapsedTime += deltaTime;
            if (elapsedTime > timePerFrame)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
                elapsedTime -= timePerFrame;
            }

            switch (currentState)
            {
                case MinerState.MovingToMine:
                    MoveTowards(targetMinePosition, deltaTime);
                    if (Vector2.Distance(Position, targetMinePosition) < 5f)
                    {
                        currentState = MinerState.Mining;
                    }
                    break;
                case MinerState.Mining:
                    goldCarried = goldCapacity;
                    currentState = MinerState.Returning;
                    break;
                case MinerState.Returning:
                    Vector2 offset = new Vector2(75, 0);
                    Vector2 modifiedTownHallPosition = townHallPosition + offset;
                    MoveTowards(modifiedTownHallPosition, deltaTime);
                    if (Vector2.Distance(Position, modifiedTownHallPosition) < 5f)
                    {
                        GoldDelivered?.Invoke(this, goldCarried);
                        goldCarried = 0;
                        currentState = MinerState.MovingToMine;
                    }
                    break;
            }
        }

        private void MoveTowards(Vector2 target, float deltaTime)
        {
            Vector2 direction = target - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                Position += direction * speed * deltaTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            int animationRow = 1;

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, animationRow * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(texture, Position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
