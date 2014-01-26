using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Vest.graphics
{
    public class OSpriteBatchGroup
        : BatchableItem
    {
        public float Depth { get; set; }

        public Texture2D Texture
        {
            get { return GroupItems.First ().Texture; }
        }

        public List<SpriteBatchItem> GroupItems = new List<SpriteBatchItem>();
        private readonly Queue<SpriteBatchItem> freeItemQueue;

        public OSpriteBatchGroup(Queue<SpriteBatchItem> freeItemQueue)
        {
            this.freeItemQueue = freeItemQueue;
        }

        public SpriteBatchItem CreateBatchItem()
        {
            SpriteBatchItem item = freeItemQueue.Count > 0
                ? freeItemQueue.Dequeue ()
                : new SpriteBatchItem ();

            GroupItems.Add (item);
            return item;
        }
    }
}
