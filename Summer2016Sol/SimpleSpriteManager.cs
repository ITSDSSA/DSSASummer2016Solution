using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Summer2016Sol
{
    class SimpleSpriteManager : DrawableGameComponent
    {
        Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        SoundEffect _pop;
        LinkedList<SimpleSprite> _baloons = new LinkedList<SimpleSprite>();
        SimpleSprite background;
        public SimpleSpriteManager(Game g) : base(g)
        {
            g.Components.Add(this);
        }

        protected override void LoadContent()
        {
            _pop = Game.Content.Load<SoundEffect>("Pop");

            _textures.Add("baloon", Game.Content.Load<Texture2D>("baloon2016"));
            _textures.Add("background", Game.Content.Load<Texture2D>("bg"));
            for (int i = 0; i < 5; i++)
            {
                orderedInsert(_baloons, new SimpleSprite(Game, _textures["baloon"], 
                    new Vector2(Utilities.Utility.NextRandom(100,400), Utilities.Utility.NextRandom(100, 400))));
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var toPop = _baloons.Where(b => b.Activated == true && !b.Alive).ToList();
            foreach (SimpleSprite item in toPop)
            {
                Game.Components.Remove(item);
                _baloons.Remove(item);
                _pop.Play();
            }
            if (_baloons.Count < 1)
                makeBaloons(gameTime);

            base.Update(gameTime);
        }

        private void makeBaloons(GameTime gameTime)
        {
            for (int i = 0; i < 5; i++)
            {
                SimpleSprite s = new SimpleSprite(Game, gameTime, _textures["baloon"],
                    new Vector2(Utilities.Utility.NextRandom(100, 400), Utilities.Utility.NextRandom(100, 400)));
                orderedInsert(_baloons,s );

            }
        }

        public void orderedInsert(LinkedList<SimpleSprite> mylist, SimpleSprite item)
        {
            // Three Cases
            // 1. item is smaller than current smallest insert before last
            // 2. item is bigger than current largest insert after first
            // 3. item is >= (or <=) a node so insert after that node (or before) 
            // 4. if list is empty then insert at first/last

            // case 4
            if (mylist.Count < 1)
            {
                mylist.AddFirst(item);
            }
            // Case 1
            else if (item.Activate <= mylist.Last.Value.Activate)
            {
                mylist.AddLast(item);
            }

            // case 2
            else if (item.Activate >= mylist.First.Value.Activate)
            {
                mylist.AddFirst(item);
            }
            // Find the position where the item should go
            else
            {
                LinkedListNode<SimpleSprite> searchNode = mylist.Last;

                while (item.Activate <= searchNode.Value.Activate)
                    searchNode = searchNode.Next;
                mylist.AddBefore(searchNode, item);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            sp.Begin();
            sp.Draw(_textures["background"], Game.GraphicsDevice.Viewport.Bounds, Color.White);
            sp.End();
            base.Draw(gameTime);
        }

        public List<SimpleSprite> getSpriteComponents()
        {
            List<SimpleSprite> sps = Game.Components.OfType<SimpleSprite>().ToList();
            return sps;
        }
    }
}
