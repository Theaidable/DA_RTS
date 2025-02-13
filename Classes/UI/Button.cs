using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DA_RTS.Classes.UI
{
    /// <summary>
    /// Repræsenterer en UI-knap, der kan opdateres og tegnes.
    /// Knappen understøtter en normal og pressed tekstur, et ikon samt tekst.
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Teksturen for knappen i normal tilstand.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Teksturen for knappen, når den er trykket.
        /// </summary>
        public Texture2D PressedTexture { get; set; }

        /// <summary>
        /// Ikonet, der vises på knappen.
        /// </summary>
        public Texture2D Icon { get; set; }

        /// <summary>
        /// Positionen (øverste venstre hjørne) for knappen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Den klikbare område for knappen. Her antages en skalering på 2.
        /// (Bemærk: hvis du ændrer skaleringen i Draw, bør denne også justeres.)
        /// </summary>
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width * 2, (int)Texture.Height * 2);

        /// <summary>
        /// Teksten, der vises på knappen.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Den SpriteFont, der bruges til at tegne teksten.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Tint-farven for knappen (standard: hvid).
        /// </summary>
        public Color Tint { get; set; } = Color.White;

        /// <summary>
        /// Indikerer om musen er over knappen.
        /// </summary>
        public bool IsHovering { get; private set; }

        /// <summary>
        /// Indikerer om knappen er trykket.
        /// </summary>
        public bool IsPressed { get; private set; }

        /// <summary>
        /// Source rectangle for ikonet, der gør det muligt at "cutte" en del ud af ikonet.
        /// </summary>
        public Rectangle IconSourceRect { get; set; } = Rectangle.Empty;

        /// <summary>
        /// Event, der udløses, når knappen klikkes.
        /// </summary>
        public event EventHandler Click;

        private MouseState currentMouse;
        private MouseState previousMouse;

        /// <summary>
        /// Konstruerer en ny Button med den angivne tekstur og position.
        /// </summary>
        /// <param name="texture">Teksturen for knappen i normal tilstand.</param>
        /// <param name="position">Positionen (øverste venstre hjørne) for knappen.</param>
        public Button(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        /// <summary>
        /// Opdaterer knapens tilstand baseret på musens position og knaptryk.
        /// Hvis musen er inden for knapens klikbare område, opdateres IsHovering og IsPressed, og
        /// Click-eventet udløses ved frigivelse af venstre musetast.
        /// </summary>
        /// <param name="gameTime">Spiltidens information.</param>
        public void Update(GameTime gameTime)
        {
            // Gem den tidligere musetilstand og hent den aktuelle
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            // Opret en lille rectangle, der repræsenterer musens position
            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            IsHovering = false;
            IsPressed = false;

            // Tjek, om musen er inden for knapens område
            if (mouseRectangle.Intersects(Bounds))
            {
                IsHovering = true;

                // Hvis venstre musetast er trykket, sæt IsPressed til true
                if (currentMouse.LeftButton == ButtonState.Pressed)
                {
                    IsPressed = true;
                }

                // Hvis musen netop er blevet sluppet, og knappen var trykket, udløs Click-eventet
                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Tegner knappen, inklusiv baggrund, ikon og tekst, med den angivne lagdybde.
        /// Lagdybden (layerDepth) bruges til at bestemme, hvor knappen vises i forhold til andre objekter.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne knappen.</param>
        /// <param name="layerDepth">Lagdybden for knappen (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            // Vælg hvilken tekstur, der skal tegnes, baseret på om knappen er trykket.
            Texture2D textureToDraw = (IsPressed && PressedTexture != null) ? PressedTexture : Texture;

            // Tegn knapbaggrunden med en fast skalering (2.1f)
            spriteBatch.Draw(textureToDraw, Position, null, Tint, 0f, Vector2.Zero, 2.1f, SpriteEffects.None, 0.1f);

            // Hvis der er sat et ikon, så tegn det
            if (Icon != null)
            {
                // Bestem source rectangle for ikonet. Hvis der ikke er sat et specifikt, bruges hele ikonet.
                Rectangle sourceRect = IconSourceRect != Rectangle.Empty ? IconSourceRect : new Rectangle(0, 0, Icon.Width, Icon.Height);
                // Beregn positionen for ikonet, centreret på knappen
                Vector2 iconPosition = Position + new Vector2((Texture.Width - sourceRect.Width) / 2, (Texture.Height - sourceRect.Height) / 2);
                // Tilføj et offset, fx for at flytte ikonet lidt til højre
                Vector2 iconOffset = new Vector2(10, 0);
                iconPosition += iconOffset;
                // Tegn ikonet med standard skala og ingen rotation
                spriteBatch.Draw(Icon, iconPosition, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00f);
            }

            // Hvis der er sat tekst, så tegn teksten centreret på knappen
            if (!string.IsNullOrEmpty(Text) && Font != null)
            {
                Vector2 textSize = Font.MeasureString(Text);
                Vector2 textPosition = Position + new Vector2((Texture.Width - textSize.X) / 2, (Texture.Height - textSize.Y) / 2);
                spriteBatch.DrawString(Font, Text, textPosition, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00f);
            }
        }
    }
}
