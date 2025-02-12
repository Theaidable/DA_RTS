using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test.Classes.Castle
{
    public class TownHall
    {
        private Vector2 position;
        private Texture2D texture;

        public TownHall(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public Vector2 Position { get { return position; } }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
