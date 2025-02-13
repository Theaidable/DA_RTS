using DA_RTS.Classes.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DA_RTS.Classes.Units
{
    /// <summary>
    /// Repræsenterer en soldat-enhed, som bevæger sig mod et mål og angriber, når den er på plads.
    /// Når soldaten har nået sit mål, vil den udføre angreb med en bestemt cooldown, og den udløser et DamageDealt-event.
    /// </summary>
    public class Soldier : Unit
    {
        // Målpositionen, som soldaten skal bevæge sig mod
        private Vector2 targetPosition;
        // Cooldown-tiden mellem angreb (i sekunder)
        private float attackCooldown = 2f;
        // Akkumuleret tid for at spore angrebscooldown
        private float elapsedTime = 0f;
        // Angrebsskaden, som soldaten forvolder
        private int attackDamage = 10;
        // Flag, der angiver om soldaten har nået sit mål
        private bool reachedTarget = false;

        // Animationsegenskaber
        private int frameWidth;
        private int frameHeight;
        private int currentFrame;
        private int totalFrames;
        private float timePerFrame;
        private float animationTimer;

        /// <summary>
        /// Udløses, når soldaten udfører et angreb, med angrebsskaden som parameter.
        /// </summary>
        public event EventHandler<int> DamageDealt;

        /// <summary>
        /// Konstruerer en ny Soldier med en startposition, tekstur, hastighed og et mål.
        /// </summary>
        /// <param name="startPosition">Startpositionen for soldaten.</param>
        /// <param name="texture">Teksturen (spritesheet) for soldaten.</param>
        /// <param name="speed">Hastigheden for soldatens bevægelse.</param>
        /// <param name="target">Målpositionen, som soldaten skal bevæge sig mod.</param>
        public Soldier(Vector2 startPosition, Texture2D texture, float speed, Vector2 target)
            : base(startPosition, texture, speed)
        {
            targetPosition = target;
            totalFrames = 6;      // Antal frames i den anvendte animationssekvens
            currentFrame = 0;
            timePerFrame = 0.1f;  // Tid pr. frame i sekunder
            animationTimer = 0f;

            // Beregn frame-dimensionerne baseret på spritesheetets størrelse.
            // Her antages, at spritesheetet har 8 rækker, hvor fx række 1 indeholder gang-animationen
            // og række 2 indeholder angrebsanimationen.
            frameWidth = texture.Width / totalFrames;
            frameHeight = texture.Height / 8;
        }

        /// <summary>
        /// Opdaterer soldatens logik baseret på den forløbne tid (deltaTime).
        /// Hvis soldaten endnu ikke har nået sit mål, bevæger den sig mod det.
        /// Når målet er nået, udføres angreb med en cooldown.
        /// </summary>
        /// <param name="deltaTime">Tid i sekunder siden sidste opdatering.</param>
        public override void UpdateLogic(float deltaTime)
        {
            // Opdater animations-timeren
            animationTimer += deltaTime;
            if (animationTimer >= timePerFrame)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
                animationTimer = 0f;
            }

            // Hvis soldaten endnu ikke har nået sit mål, bevæg den mod målpositionen
            if (!reachedTarget)
            {
                MoveTowards(targetPosition, deltaTime);
                if (Vector2.Distance(Position, targetPosition) < 5f)
                {
                    reachedTarget = true;
                }
            }
            else
            {
                // Når målet er nået, akkumuleres tiden for angreb
                elapsedTime += deltaTime;
                if (elapsedTime >= attackCooldown)
                {
                    // Udløs eventet med angrebsskaden, og nulstil timeren
                    DamageDealt?.Invoke(this, attackDamage);
                    elapsedTime = 0f;
                }
            }
        }

        /// <summary>
        /// Hjælpefunktion, der bevæger soldaten mod en given position med den angivne deltaTime.
        /// </summary>
        /// <param name="target">Målpositionen.</param>
        /// <param name="deltaTime">Tid i sekunder siden sidste opdatering.</param>
        private void MoveTowards(Vector2 target, float deltaTime)
        {
            Vector2 direction = target - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                Position += direction * speed * deltaTime;
            }
        }

        /// <summary>
        /// Tegner soldaten på skærmen med en angivet lagdybde.
        /// Vælger animationsrække baseret på om soldaten har nået sit mål (gang vs. angreb).
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch til tegning.</param>
        /// <param name="layerDepth">Lagdybde for tegningen.</param>
        public override void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            // Vælg animationsrækken: række 1 (index 1) for gang, række 2 (index 2) for angreb.
            int animationRow = reachedTarget ? 2 : 1;
            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, animationRow * frameHeight, frameWidth, frameHeight);
            spriteBatch.Draw(texture, Position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }
    }
}
