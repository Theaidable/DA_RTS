using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DA_RTS.Classes.Units
{
    public class Soldier : Unit
    {
        private Vector2 targetPosition;              // Målposition, fx fjendens base
        private float attackTimer = 0f;                // Tæller til at styre angrebstakten
        private readonly float attackInterval;         // Tid mellem angreb (i sekunder)
        private readonly int attackDamage;             // Hvor meget skade soldaten forvolder pr. angreb

        // Konstant, der definerer hvor tæt en soldier skal være målet for at skifte til angrebs-tilstand
        private const float ArrivalThreshold = 5f;

        // Simpel state-machine for soldaten
        private enum SoldierState { MovingToTarget, Attacking }
        private SoldierState state;

        /// <summary>
        /// Constructor for Soldier.
        /// </summary>
        /// <param name="startPosition">Startposition for soldaten</param>
        /// <param name="moveSpeed">Bevægelseshastighed</param>
        /// <param name="health">Soldatens liv</param>
        /// <param name="targetPosition">Målposition, fx fjendens base</param>
        /// <param name="attackDamage">Angrebsskade (default: 10)</param>
        /// <param name="attackInterval">Angrebshastighed (default: 1.0 sekund)</param>
        public Soldier(Vector2 startPosition, float moveSpeed, int health, Vector2 targetPosition, int attackDamage = 10, float attackInterval = 1.0f)
            : base(startPosition, moveSpeed, health)
        {
            this.targetPosition = targetPosition;
            this.attackDamage = attackDamage;
            this.attackInterval = attackInterval;
            state = SoldierState.MovingToTarget;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (state)
            {
                case SoldierState.MovingToTarget:
                    MoveTowards(targetPosition, elapsed);
                    // Når soldaten er tæt nok på målet, skift til angrebs-tilstand
                    if (Vector2.Distance(Position, targetPosition) < ArrivalThreshold)
                    {
                        state = SoldierState.Attacking;
                        attackTimer = 0f;
                    }
                    break;

                case SoldierState.Attacking:
                    attackTimer += elapsed;
                    if (attackTimer >= attackInterval)
                    {
                        attackTimer = 0f;
                        // Her kan du udvide med et rigtigt kamp-system,
                        // fx ved at kalde en metode på en fjende-enhed eller fjendens base for at reducere dens helbred.
                        Console.WriteLine($"Soldier ved {Position} angriber for {attackDamage} skade.");
                    }
                    break;
            }
        }

        /// <summary>
        /// Hjælpefunktion til at flytte soldaten mod en given position.
        /// </summary>
        /// <param name="target">Målposition</param>
        /// <param name="elapsed">Forløbet tid siden sidste opdatering</param>
        private void MoveTowards(Vector2 target, float elapsed)
        {
            Vector2 direction = target - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                Position += direction * MoveSpeed * elapsed;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }
        }
    }
}
