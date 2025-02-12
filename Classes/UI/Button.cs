using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DA_RTS.Classes.UI
{
    public class Button
    {
        //Properties
        public Texture2D Texture { get; set; }
        public Texture2D PressedTexture { get; set; }
        public Texture2D Icon { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width * 2, (int)Texture.Height * 2);
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color Tint { get; set; } = Color.White;
        public bool IsHovering { get; private set; }
        public bool IsPressed { get; private set; }
        public Rectangle IconSourceRect { get; set; } = Rectangle.Empty;


        public event EventHandler Click;

        private MouseState currentMouse;
        private MouseState previousMouse;

        public Button(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            IsHovering = false;
            IsPressed = false;

            if (mouseRectangle.Intersects(Bounds))
            {
                IsHovering = true;

                if (currentMouse.LeftButton == ButtonState.Pressed)
                {
                    IsPressed = true;
                }

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            Texture2D textureToDraw = (IsPressed && PressedTexture != null) ? PressedTexture : Texture;

            spriteBatch.Draw(textureToDraw, Position, null, Tint, 0f, Vector2.Zero, 2.1f, SpriteEffects.None, 0.1f);

            if (Icon != null)
            {
                Rectangle sourceRect = IconSourceRect != Rectangle.Empty ? IconSourceRect : new Rectangle(0, 0, Icon.Width, Icon.Height);
                Vector2 iconPosition = Position + new Vector2((Texture.Width - sourceRect.Width) / 2, (Texture.Height - sourceRect.Height) / 2);

                Vector2 iconOffset = new Vector2(10, 0);
                iconPosition += iconOffset;

                spriteBatch.Draw(Icon, iconPosition, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00f);
            }

            if (!string.IsNullOrEmpty(Text) && Font != null)
            {
                Vector2 textSize = Font.MeasureString(Text);
                Vector2 textPosition = Position + new Vector2((Texture.Width - textSize.X) / 2, (Texture.Height - textSize.Y) / 2);
                spriteBatch.DrawString(Font, Text, textPosition, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00f);
            }
        }
    }
}
