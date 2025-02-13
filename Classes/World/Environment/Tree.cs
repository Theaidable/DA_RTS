using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World.Environment
{
    /// <summary>
    /// Repræsenterer et træ i miljøet.
    /// Klassen håndterer placeringen og den grafiske fremstilling af et træ.
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Gets or sets positionen for træet (øverste venstre hjørne).
        /// </summary>
        public Vector2 Position { get; set; }

        // Teksturen der anvendes til at tegne træet.
        private Texture2D texture;

        /// <summary>
        /// Konstruerer en ny instans af Tree med den specificerede position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for træet.</param>
        /// <param name="texture">Teksturen, der skal bruges til at tegne træet.</param>
        public Tree(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Tegner træet på skærmen med en angivet lagdybde.
        /// Lagdybden bestemmer tegneordenen, så objekter med lavere layerDepth vises forrest.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne træet.</param>
        /// <param name="layerDepth">Lagdybden for træets tegning.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
