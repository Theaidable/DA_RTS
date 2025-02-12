using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.Mines
{
    public class Mine
    {
        private Vector2 position;
        private Texture2D texture;

        public Mine(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public Vector2 GetPosition() { return position; }


        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
