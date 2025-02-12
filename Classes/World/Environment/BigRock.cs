using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World.Environment
{
    public class BigRock
    {
        public Vector2 Position { get; set; }
        private Texture2D texture;

        public BigRock(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
