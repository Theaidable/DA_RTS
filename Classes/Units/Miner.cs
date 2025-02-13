using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DA_RTS.Classes.Units
{
    /// <summary>
    /// Repræsenterer en miner-enhed, der kan bevæge sig mod en mine, mine guld og returnere til TownHall.
    /// Minerens logik opdateres via UpdateLogic-metoden, og den udløser et GoldDelivered-event,
    /// når den afleverer sit guld ved TownHall.
    /// </summary>
    public class Miner : Unit
    {
        // Animationsegenskaber
        private int frameWidth;
        private int frameHeight;
        private int currentFrame;
        private int totalFrames;
        private int totalRows;
        private float timePerFrame;
        private float elapsedTime;

        // Definér de mulige tilstande for en miner
        private enum MinerState { MovingToMine, Mining, Returning }
        private MinerState currentState;

        // Positioner for mål (minen og TownHall)
        private Vector2 targetMinePosition;
        private Vector2 townHallPosition;

        // Ressourcehåndtering
        private int goldCarried;
        private int goldCapacity;

        // Variabler til visning og mining-timer
        private bool isVisable; // Angiver om mineren skal vises (f.eks. ikke vises under mining)
        private bool isWaiting; //angiver om mineren står venter på at kunne komme ind i minen
        private float miningTimeElapsed = 0f;

        /// <summary>
        /// Udløses, når mineren afleverer sit guld ved TownHall.
        /// Parameteren angiver, hvor meget guld der blev afleveret.
        /// </summary>
        public event EventHandler<int> GoldDelivered;

        // Begræns antal miners der kan være i mining-tilstanden samtidigt
        private static SemaphoreSlim mineSemaphore = new SemaphoreSlim(3);

        /// <summary>
        /// Konstruerer en ny Miner med startposition, tekstur, hastighed, mål-mine og TownHall-position.
        /// </summary>
        /// <param name="startPosition">Startpositionen for mineren.</param>
        /// <param name="texture">Teksturen (spritesheet) for mineren.</param>
        /// <param name="speed">Bevægelighedshastigheden for mineren.</param>
        /// <param name="targetMine">Positionen for minen, som mineren skal gå hen til.</param>
        /// <param name="townHall">Positionen for TownHall, hvor mineren afleverer guld.</param>
        public Miner(Vector2 startPosition, Texture2D texture, float speed, Vector2 targetMine, Vector2 townHall)
            : base(startPosition, texture, speed)
        {
            targetMinePosition = targetMine;
            townHallPosition = townHall;

            currentState = MinerState.MovingToMine;

            goldCarried = 0;
            goldCapacity = 50;

            totalFrames = 6;
            totalRows = 6;
            currentFrame = 0;
            elapsedTime = 0f;
            timePerFrame = 0.2f;

            // Beregn bredden og højden for hvert frame baseret på spritesheetets dimensioner
            frameWidth = texture.Width / totalFrames;
            frameHeight = texture.Height / totalRows;
        }

        /// <summary>
        /// Opdaterer minerens logik baseret på den forløbne tid (deltaTime).
        /// Denne metode håndterer animation, bevægelse og tilstandsskift mellem
        /// MovingToMine, Mining og Returning.
        /// </summary>
        /// <param name="deltaTime">Tid i sekunder siden sidste opdatering.</param>
        public override void UpdateLogic(float deltaTime)
        {
            // Opdater animationstimer
            elapsedTime += deltaTime;
            if (elapsedTime > timePerFrame)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
                elapsedTime -= timePerFrame;
            }

            // Opdater minerens tilstand
            switch (currentState)
            {
                case MinerState.MovingToMine:
                    
                    // Miner skal være synlig, mens den bevæger sig mod minen
                    isVisable = true;
                    
                    if(Vector2.Distance(Position,targetMinePosition) >= 5f)
                    {
                        MoveTowards(targetMinePosition, deltaTime);
                        isWaiting = false;
                    }
                    else
                    {
                        if (mineSemaphore.Wait(0))
                        {
                            currentState = MinerState.Mining;
                            isWaiting = false;
                        }
                        else
                        {
                            isWaiting = true;
                        }
                    }
                    break;

                case MinerState.Mining:
                    // Skjul mineren mens den miner, og akkumuler miningtid
                    isVisable = false;
                    miningTimeElapsed += deltaTime;
                    // Simuler, at det tager 5 sekunder at mine 50 guld (10 guld pr. sekund)
                    if (miningTimeElapsed >= 5f)
                    {
                        goldCarried = goldCapacity;
                        miningTimeElapsed = 0f;
                        currentState = MinerState.Returning;

                        // Frigiv permit, så en anden miner kan komme ind i minen
                        mineSemaphore.Release();
                    }
                    break;

                case MinerState.Returning:
                    // Gør mineren synlig igen, når den vender tilbage mod TownHall
                    isVisable = true;
                    
                    // Tilføj et offset, så mineren ikke går direkte på TownHall, men lidt til højre
                    Vector2 offset = new Vector2(75, 0);
                    Vector2 modifiedTownHallPosition = townHallPosition + offset;
                    MoveTowards(modifiedTownHallPosition, deltaTime);
                    
                    if (Vector2.Distance(Position, modifiedTownHallPosition) < 5f)
                    {
                        // Udløs eventet for aflevering af guld
                        GoldDelivered?.Invoke(this, goldCarried);
                        goldCarried = 0;
                        currentState = MinerState.MovingToMine;
                    }
                    break;
            }
        }

        /// <summary>
        /// Hjælpefunktion, der bevæger mineren mod det angivne mål med en given deltaTime.
        /// </summary>
        /// <param name="target">Målpositionen, som mineren skal bevæge sig mod.</param>
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
        /// Tegner mineren på skærmen, hvis den er synlig.
        /// Hvis mineren er i Returning-tilstanden, vendes sprite vandret.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch der bruges til at tegne mineren.</param>
        /// <param name="layerDepth">Lagdybden for tegning.</param>
        public override void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            // Hvis mineren ikke skal vises (fx under mining), afslut tegningen
            if (!isVisable)
            {
                return;
            }

            int animationRow = 1; // Vælg den række i spritesheetet, der repræsenterer gå-animationen
            
            if(currentState == MinerState.MovingToMine && isWaiting)
            {
                animationRow = 0;
            }
            
            Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, animationRow * frameHeight, frameWidth, frameHeight);
            // Hvis mineren er på vej tilbage, flip sprite vandret
            SpriteEffects effects = (currentState == MinerState.Returning) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, Position, sourceRect, Color.White, 0f, Vector2.Zero, 1f, effects, layerDepth);
        }
    }
}
