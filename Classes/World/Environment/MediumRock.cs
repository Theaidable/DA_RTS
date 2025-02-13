using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World.Environment
{
    /// <summary>
    /// Repræsenterer en mellemstor sten (MediumRock) i miljøet.
    /// Klassen håndterer placeringen og den grafiske fremstilling af en mellemstor sten.
    /// </summary>
    public class MediumRock
    {
        /// <summary>
        /// Gets or sets positionen for MediumRock (øverste venstre hjørne).
        /// </summary>
        public Vector2 Position { get; set; }

        // Teksturen, der bruges til at tegne stenen.
        private Texture2D texture;

        /// <summary>
        /// Konstruerer en ny instans af MediumRock med en specifik position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for stenen.</param>
        /// <param name="texture">Teksturen til stenen.</param>
        public MediumRock(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Tegner MediumRock på skærmen med den angivne lagdybde.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne objektet.</param>
        /// <param name="layerDepth">Lagdybden, som bestemmer tegneordenen (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
