using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World.Environment
{
    /// <summary>
    /// Repræsenterer en svamp (Shroom) i miljøet.
    /// Klassen håndterer placeringen og den grafiske fremstilling af en svamp.
    /// </summary>
    public class Shroom
    {
        /// <summary>
        /// Gets or sets positionen for svampen (øverste venstre hjørne).
        /// </summary>
        public Vector2 Position { get; set; }

        // Teksturen, der repræsenterer svampen grafisk.
        private Texture2D texture;

        /// <summary>
        /// Konstruerer en ny instans af Shroom med en specifik position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for svampen.</param>
        /// <param name="texture">Teksturen, der anvendes til at tegne svampen.</param>
        public Shroom(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Tegner svampen på skærmen med den angivne lagdybde.
        /// Lagdybden styrer tegneordenen, så objekter med lavere layerDepth vises forrest.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne objektet.</param>
        /// <param name="layerDepth">Lagdybden for svampen.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
