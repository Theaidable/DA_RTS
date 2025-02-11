using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DA_RTS.Classes.Units
{
    public class TownHall
    {
        private int gold;
        private readonly object goldLock = new object();

        // Position og sprite for at kunne tegne basen i spillet
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }

        // Nye felter for basens liv
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }

        // Constructor – vi sætter et default maxHealth (kan tilpasses)
        public TownHall(Vector2 position, int maxHealth = 1000)
        {
            Position = position;
            gold = 150;
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        /// <summary>
        /// Sætter guldet ind på en trådsikker måde.
        /// </summary>
        /// <param name="amount">Mængden der skal sættes ind</param>
        public void DepositGold(int amount)
        {
            lock (goldLock)
            {
                gold += amount;
                Console.WriteLine($"TownHall: Afleverede {amount} guld. Total guld: {gold}");
            }
        }

        /// <summary>
        /// Trækker guldet fra på en trådsikker måde. Returnerer true, hvis operationen lykkedes.
        /// </summary>
        /// <param name="amount">Mængden der skal trækkes</param>
        public bool UseGold(int amount)
        {
            lock (goldLock)
            {
                if (gold >= amount)
                {
                    gold -= amount;
                    Console.WriteLine($"TownHall: Brugt {amount} guld. Tilbage: {gold}");
                    return true;
                }
                else
                {
                    Console.WriteLine("Ikke nok guld!");
                    return false;
                }
            }
        }

        /// <summary>
        /// Returnerer den aktuelle guldmængde på en trådsikker måde.
        /// </summary>
        public int Gold
        {
            get
            {
                lock (goldLock)
                {
                    return gold;
                }
            }
        }

        /// <summary>
        /// Reducerer basens health med en given mængde skade.
        /// </summary>
        /// <param name="damage">Skademængden</param>
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
                Health = 0;
            Console.WriteLine($"TownHall: Tog {damage} skade. Resterende health: {Health}");
        }

        /// <summary>
        /// Returnerer, om basen er ødelagt.
        /// </summary>
        public bool IsDestroyed => Health <= 0;

        /// <summary>
        /// Tegner TownHall samt et simpelt UI-overlay med gold og health.
        /// Bemærk: font skal loades i din LoadContent i GameWorld og sendes ind her.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch til tegning</param>
        /// <param name="font">SpriteFont til at tegne tekst</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Color.White);
            }

            /* Tegn UI-overlay, fx i øverste venstre hjørne
            string uiText = $"Health: {Health}/{MaxHealth}" +
                $"\nGold: {Gold}";
            spriteBatch.DrawString(font, uiText, new Vector2(10, 10), Color.Yellow);
            */
        }
    }
}
