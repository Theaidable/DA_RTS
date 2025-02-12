using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Test.Classes.Units
{
    public abstract class Unit
    {
        private Vector2 position;
        protected Texture2D texture;
        protected float speed;

        public Vector2 Position { get { return position; } set { position = value; } }

        public Unit(Vector2 position, Texture2D texture, float speed)
        {
            this.Position = position;
            this.texture = texture;
            this.speed = speed;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
