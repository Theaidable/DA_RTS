using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.World.Environment
{
    /// <summary>
    /// Repræsenterer en lille sten (SmallRock) i miljøet.
    /// Klassen håndterer placeringen og den grafiske fremstilling af en lille sten.
    /// </summary>
    public class SmallRock
    {
        /// <summary>
        /// Gets or sets positionen for SmallRock (øverste venstre hjørne).
        /// </summary>
        public Vector2 Position { get; set; }

        // Teksturen der anvendes til at tegne stenen.
        private Texture2D texture;

        /// <summary>
        /// Konstruerer en ny instans af SmallRock med en specificeret position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for stenen.</param>
        /// <param name="texture">Teksturen til stenen.</param>
        public SmallRock(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Tegner SmallRock på skærmen med en angivet lagdybde.
        /// Lagdybden bestemmer tegneordenen, hvor lavere værdier vises forrest.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne objektet.</param>
        /// <param name="layerDepth">Lagdybden for tegningen.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
