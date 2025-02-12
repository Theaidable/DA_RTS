using DA_RTS.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;

namespace DA_RTS.Classes.Units
{
    public class Soldier : Unit
    {
        private Vector2 targetPosition;
        private float attackCooldown = 2f;
        private float elapsedTime = 0f;
        private int attackDamage = 10;
        private bool reachedTarget = false;

        private int frameWidth;
        private int frameHeight;
        private int currentFrame;
        private int totalFrames;
        private float timePerFrame;
        private float animationTimer;

        public event EventHandler<int> DamageDealt;

        public Soldier(Vector2 startPosition, Texture2D texture, float speed, Vector2 target)
            : base(startPosition, texture, speed)
        {
            targetPosition = target;
            totalFrames = 6; // Juster dette baseret på spritesheetet
            currentFrame = 0;
            timePerFrame = 0.1f; // 0.1 sek per frame
            animationTimer = 0f;

            frameWidth = texture.Width / totalFrames;
            frameHeight = texture.Height / 8; // Antal rækker i spritesheetet (fx idle, walk, attack)
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Animation update
            animationTimer += deltaTime;
            if (animationTimer >= timePerFrame)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
                animationTimer = 0f;
            }

            if (!reachedTarget)
            {
                MoveTowards(targetPosition, deltaTime);
                if (Vector2.Distance(Position, targetPosition) < 5f)
                {
                    reachedTarget = true;
                }
            }
            else
            {
                elapsedTime += deltaTime;
                if (elapsedTime >= attackCooldown)
                {
                    DamageDealt?.Invoke(this, attackDamage);
                    elapsedTime = 0f;
                }
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
            int animationRow = reachedTarget ? 2 : 1; // F.eks. række 1 = gå, række 2 = angreb

            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, animationRow * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(texture, Position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.7f);
        }
    }
}
