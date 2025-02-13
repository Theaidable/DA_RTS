using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DA_RTS.Classes.Units
{
    /// <summary>
    /// Abstrakt baseklasse for alle enheder i spillet.
    /// Indeholder fælles egenskaber som position, tekstur og hastighed,
    /// samt abstrakte og virtuelle metoder til at opdatere logik og tegne enheden.
    /// </summary>
    public abstract class Unit
    {
        // Felt til at lagre enhedens position
        private Vector2 position;
        // Teksturen der repræsenterer enheden grafisk
        protected Texture2D texture;
        // Hastigheden hvormed enheden bevæger sig
        protected float speed;

        /// <summary>
        /// Gets or sets positionen for enheden.
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }

        /// <summary>
        /// Konstruerer en ny Unit med den specificerede position, tekstur og hastighed.
        /// </summary>
        /// <param name="position">Startpositionen for enheden.</param>
        /// <param name="texture">Teksturen til enheden.</param>
        /// <param name="speed">Bevægelighedshastigheden for enheden.</param>
        public Unit(Vector2 position, Texture2D texture, float speed)
        {
            Position = position;
            this.texture = texture;
            this.speed = speed;
        }

        /// <summary>
        /// Opdaterer enhedens logik baseret på den forløbne tid.
        /// Denne metode skal implementeres af de afledte klasser.
        /// </summary>
        /// <param name="deltaTime">Den tid, der er gået siden sidste opdatering, i sekunder.</param>
        public abstract void UpdateLogic(float deltaTime);

        /// <summary>
        /// Tegner enheden på skærmen.
        /// Standardimplementeringen tegner teksturen i dens nuværende position med den angivne lagdybde.
        /// Afledte klasser kan overstyre denne metode for at tilføje animation eller andre effekter.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch der bruges til at tegne enheden.</param>
        /// <param name="layerDepth">Lagdybden, der bestemmer tegneordenen (lavere værdi vises forrest).</param>
        public virtual void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
