using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

namespace DA_RTS.Classes.Units
{
    /// <summary>
    /// Abstrakt basis-klasse for alle enheder. Indeholder fælles egenskaber som position, bevægelseshastighed, liv og sprite.
    /// Hver unit starter sin egen tråd, der kører en løkke, som kalder Update-metoden med et simpelt tidsstempel.
    /// </summary>
    public abstract class Unit
    {
        // Fields
        private int health;
        private float moveSpeed;
        private Vector2 position;
        private Texture2D texture;

        // Thread-felter
        private Thread unitThread;
        private volatile bool isRunning = false; // bruges til at stoppe tråden

        // Egenskaber
        public int Health { get => health; protected set => health = value; }
        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        public Vector2 Position { get => position; protected set => position = value; }
        public Texture2D Texture { get => texture; set => texture = value; }

        // Constructor
        public Unit(Vector2 startPosition, float moveSpeed, int health)
        {
            Position = startPosition;
            MoveSpeed = moveSpeed;
            Health = health;
        }

        /// <summary>
        /// Metode, der opdaterer unitens logik. Skal implementeres i de afledte klasser.
        /// </summary>
        /// <param name="gameTime">Simuleret spil-tid for den pågældende update</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Metode til at tegne enheden. Skal implementeres i de afledte klasser.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch til tegning</param>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Starter den interne tråd for unitens logik.
        /// </summary>
        public void Start()
        {
            isRunning = true;
            unitThread = new Thread(UnitLoop);
            unitThread.IsBackground = true; // gør at tråden ikke forhindrer programmet i at lukke ned
            unitThread.Start();
        }

        /// <summary>
        /// Stopper unitens tråd.
        /// </summary>
        public void Stop()
        {
            isRunning = false;
            if (unitThread != null && unitThread.IsAlive)
            {
                unitThread.Join();
            }
        }

        /// <summary>
        /// Den løbende løkke, der kører i en separat tråd og kalder Update med et simpelt tidsstempel.
        /// Bemærk: Tegning foretages stadig i hovedtråden.
        /// </summary>
        private void UnitLoop()
        {
            DateTime lastUpdate = DateTime.Now;
            while (isRunning)
            {
                DateTime now = DateTime.Now;
                float elapsedSeconds = (float)(now - lastUpdate).TotalSeconds;
                lastUpdate = now;
                // Opret et simpelt GameTime-objekt med det forløbne tidsinterval
                GameTime gameTime = new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(elapsedSeconds));
                Update(gameTime);
                Thread.Sleep(16); // Ca. 60 opdateringer pr. sekund
            }
        }
    }
}
