using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprites
{
    public class SimpleSprite : DrawableGameComponent
    {
        public Texture2D Image;
        public Vector2 Position;
        public Rectangle BoundingRect;

        // Added Fields
        TimeSpan _activate;
        TimeSpan _survival;
        string Name;
        bool _activated = false;
        public bool Alive = true;

        public TimeSpan Activate
        {
            get
            {
                return _activate;
            }

            set
            {
                _activate = value;
            }
        }

        public TimeSpan Survival
        {
            get
            {
                return _survival;
            }

            set
            {
                _survival = value;
            }
        }

        public bool Activated
        {
            get
            {
                return _activated;
            }

            set
            {
                _activated = value;
            }
        }

        // Constructor expects to see a loaded Texture
        // and a start position
        public SimpleSprite(Game g, Texture2D spriteImage,
                            Vector2 startPosition) : base(g)
        {
            Setup(g, spriteImage, startPosition);
        }

        private void Setup(Game g, Texture2D spriteImage,
                            Vector2 startPosition)
        {
            g.Components.Add(this);
            //
            // Take a copy of the texture passed down
            Image = spriteImage;
            // Take a copy of the start position
            Position = startPosition;
            // Calculate the bounding rectangle
            BoundingRect = new Rectangle((int)startPosition.X, (int)startPosition.Y, Image.Width, Image.Height);
            Activate = new TimeSpan(0, 0, 0, 0, Utilities.Utility.NextRandom(1200));
            Survival = Activate + new TimeSpan(0, 0, 0, 0, Utilities.Utility.NextRandom(1200));

        }

        public SimpleSprite(Game g, GameTime gameTime, Texture2D spriteImage,
                    Vector2 startPosition) : base(g)
        {
            Setup(g, spriteImage, startPosition);
            Activate = new TimeSpan(0, 0, 0, 0, Utilities.Utility.NextRandom(1200));
            Survival = Activate + new TimeSpan(0, 0, 0, 0, Utilities.Utility.NextRandom(1200));
            Activate += gameTime.TotalGameTime;
            Survival += gameTime.TotalGameTime;
        }
    
    
        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > Activate)
                Activated = true;
            if (gameTime.TotalGameTime > Survival)
            {
                Alive = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            if (Image != null && Activated)
            {
                sp.Begin();
                sp.Draw(Image, BoundingRect, Color.White);
                sp.End();
            }

            base.Draw(gameTime);
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, Image.Width, Image.Height);
            BoundingRect.X = (int)Position.X;
            BoundingRect.Y = (int)Position.Y;
        }

    }
}
