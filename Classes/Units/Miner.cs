using DA_RTS.Classes.Castle;
using DA_RTS.Classes.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DA_RTS.Classes.Units
{
    public class Miner : Unit
    {
        private int goldMined;                   // Aktuel mængde guld
        private readonly int goldCapacity = 50;    // Maksimal kapacitet

        // Positioner for mine og base – disse skal sættes, når du initialiserer en Miner
        private Vector2 minePosition;
        private Vector2 basePosition;

        // Timer til mining-logik
        private float miningTimer = 0f;
        private readonly float miningInterval = 1.0f; // F.eks. miner 10 guld pr. sekund

        // Reference til TownHall, hvor guldet skal afleveres
        private TownHall townHall;

        // Definer en threshold for, hvornår vi anser en destination for "nået"
        private const float ArrivalThreshold = 5f;

        // Simpel state-machine
        private enum MinerState { MovingToMine, Mining, ReturningToBase }
        private MinerState state;

        /// <summary>
        /// Constructor for Miner.
        /// </summary>
        /// <param name="startPosition">Startposition for mineren</param>
        /// <param name="moveSpeed">Bevægelseshastighed</param>
        /// <param name="health">Liv</param>
        /// <param name="minePosition">Positionen for minen</param>
        /// <param name="basePosition">Positionen for basen</param>
        /// <param name="townHall">Reference til TownHall for at aflevere guld</param>
        public Miner(Vector2 startPosition, float moveSpeed, int health, Vector2 minePosition, Vector2 basePosition, TownHall townHall)
            : base(startPosition, moveSpeed, health)
        {
            this.minePosition = minePosition;
            this.basePosition = basePosition;
            this.townHall = townHall;
            state = MinerState.MovingToMine;
            goldMined = 0;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (state)
            {
                case MinerState.MovingToMine:
                    MoveTowards(minePosition, elapsed);
                    if (Vector2.Distance(Position, minePosition) < ArrivalThreshold)
                    {
                        state = MinerState.Mining;
                        miningTimer = 0f;
                    }
                    break;

                case MinerState.Mining:
                    miningTimer += elapsed;
                    if (miningTimer >= miningInterval)
                    {
                        miningTimer = 0f;
                        goldMined += 10;
                        // Hvis vi har nået eller overskredet kapaciteten, skift til ReturningToBase
                        if (goldMined >= goldCapacity)
                        {
                            goldMined = goldCapacity;
                            state = MinerState.ReturningToBase;
                        }
                    }
                    break;

                case MinerState.ReturningToBase:
                    MoveTowards(basePosition, elapsed);
                    if (Vector2.Distance(Position, basePosition) < ArrivalThreshold)
                    {
                        // Aflever guldet – TownHall håndterer synkroniseringen internt
                        townHall.DepositGold(goldMined);
                        goldMined = 0;
                        state = MinerState.MovingToMine;
                    }
                    break;
            }
        }

        /// <summary>
        /// Hjælpefunktion til at flytte mineren mod en given position.
        /// </summary>
        /// <param name="target">Målposition</param>
        /// <param name="elapsed">Forløbet tid siden sidste opdatering</param>
        private void MoveTowards(Vector2 target, float elapsed)
        {
            Vector2 direction = target - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                // Opdaterer positionen baseret på bevægelseshastigheden og det forløbne tidsinterval.
                Position += direction * MoveSpeed * elapsed;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Hvis du vil skjule mineren under mining, kan du tilføje betingelser her.
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}
