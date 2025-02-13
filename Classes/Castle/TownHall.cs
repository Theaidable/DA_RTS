using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.Castle
{
    /// <summary>
    /// Repræsenterer TownHall (basen) i spillet.
    /// Denne klasse håndterer positionen og den grafiske repræsentation af TownHall.
    /// </summary>
    public class TownHall
    {
        private Vector2 position;
        private Texture2D texture;

        /// <summary>
        /// Initialiserer en ny instans af TownHall med den angivne position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for TownHall (øverste venstre hjørne).</param>
        /// <param name="texture">Teksturen, der skal bruges til at tegne TownHall.</param>
        public TownHall(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Returnerer positionen for TownHall.
        /// </summary>
        public Vector2 Position { get { return position; } }

        /// <summary>
        /// Tegner TownHall på skærmen med den specificerede lagdybde.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne grafikken.</param>
        /// <param name="layerDepth">Lagdybden, der bestemmer tegneordenen (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
