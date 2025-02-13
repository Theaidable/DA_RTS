using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.Mines
{
    /// <summary>
    /// Repræsenterer en Mine i spillet.
    /// Klassen håndterer positionen og tegningen af en mine.
    /// </summary>
    public class Mine
    {
        private Vector2 position;
        private Texture2D texture;

        /// <summary>
        /// Initialiserer en ny instans af Mine med den angivne position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for minen (øverste venstre hjørne).</param>
        /// <param name="texture">Teksturen, der skal bruges til at tegne minen.</param>
        public Mine(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Returnerer positionen for minen.
        /// </summary>
        /// <returns>En Vector2 der repræsenterer minens position.</returns>
        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Tegner minen på skærmen med en angivet lagdybde.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne objektet.</param>
        /// <param name="layerDepth">Lagdybden, som bestemmer tegneordenen (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
