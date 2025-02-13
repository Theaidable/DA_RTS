using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World.Environment
{
    /// <summary>
    /// Repræsenterer en stor sten (BigRock) i miljøet.
    /// Klassen håndterer placeringen og den grafiske fremstilling af en stor sten.
    /// </summary>
    public class BigRock
    {
        /// <summary>
        /// Gets or sets positionen for BigRock (øverste venstre hjørne).
        /// </summary>
        public Vector2 Position { get; set; }

        // Teksturen, der bruges til at tegne stenen.
        private Texture2D texture;

        /// <summary>
        /// Konstruerer en ny instans af BigRock med en specifik position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for stenen.</param>
        /// <param name="texture">Teksturen til stenen.</param>
        public BigRock(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Tegner BigRock på skærmen med en angivet lagdybde.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne objektet.</param>
        /// <param name="layerDepth">Lagdybden, der bestemmer tegneordenen (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
