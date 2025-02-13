using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World.Environment
{
    /// <summary>
    /// Repræsenterer en stor busk (BigBrush) i miljøet.
    /// Klassen håndterer placeringen og den grafiske tegning af en stor busk.
    /// </summary>
    public class BigBrush
    {
        /// <summary>
        /// Gets or sets positionen for BigBrush (øverste venstre hjørne).
        /// </summary>
        public Vector2 Position { get; set; }

        // Teksturen der bruges til at tegne busken.
        private Texture2D texture;

        /// <summary>
        /// Konstruerer en ny instans af BigBrush med en specifik position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for busken.</param>
        /// <param name="texture">Teksturen til busken.</param>
        public BigBrush(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Tegner BigBrush på skærmen med en angivet lagdybde.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne busken.</param>
        /// <param name="layerDepth">Lagdybden, der bestemmer tegneordenen (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
