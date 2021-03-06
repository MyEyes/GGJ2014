﻿/******************************************************************************
 * Spine Runtime Software License - Version 1.1
 * 
 * Copyright (c) 2013, Esoteric Software
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms in whole or in part, with
 * or without modification, are permitted provided that the following conditions
 * are met:
 * 
 * 1. A Spine Essential, Professional, Enterprise, or Education License must
 *    be purchased from Esoteric Software and the license must remain valid:
 *    http://esotericsoftware.com/
 * 2. Redistributions of source code must retain this license, which is the
 *    above copyright notice, this declaration of conditions and the following
 *    disclaimer.
 * 3. Redistributions in binary form must reproduce this license, which is the
 *    above copyright notice, this declaration of conditions and the following
 *    disclaimer, in the documentation and/or other materials provided with the
 *    distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Otherworld.Utilities;

namespace Vest.graphics
{
    // #region License
    // /*
    // Microsoft Public License (Ms-PL)
    // MonoGame - Copyright © 2009 The MonoGame Team
    // 
    // All rights reserved.
    // 
    // This license governs use of the accompanying software. If you use the software, you accept this license. If you do not
    // accept the license, do not use the software.
    // 
    // 1. Definitions
    // The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under 
    // U.S. copyright law.
    // 
    // A "contribution" is the original software, or any additions or changes to the software.
    // A "contributor" is any person that distributes its contribution under this license.
    // "Licensed patents" are a contributor's patent claims that read directly on its contribution.
    // 
    // 2. Grant of Rights
    // (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
    // each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
    // (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
    // each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
    // 
    // 3. Conditions and Limitations
    // (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
    // (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
    // your patent license from such contributor to the software ends automatically.
    // (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution 
    // notices that are present in the software.
    // (D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
    // a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object 
    // code form, you may only do so under a license that complies with this license.
    // (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees
    // or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent
    // permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular
    // purpose and non-infringement.
    // */
    // #endregion License
    // 

    /// <summary>
    /// This class handles the queueing of batch items into the GPU by creating the triangle tesselations
    /// that are used to draw the sprite textures. This class supports int.MaxValue number of sprites to be
    /// batched and will process them into short.MaxValue groups (strided by 6 for the number of vertices
    /// sent to the GPU). 
    /// </summary>
    public class SpriteBatcher
    {
        /// <summary>
        /// Initialization size for the batch item list and queue.
        /// </summary>
        private const int InitialBatchSize = 256;
        /// <summary>
        /// The maximum number of batch items that can be processed per iteration
        /// </summary>
        private const int MaxBatchSize = short.MaxValue / 6; // 6 = 4 vertices unique and 2 shared, per quad
        /// <summary>
        /// Initialization size for the vertex array, in batch units.
        /// </summary>
        private const int InitialVertexArraySize = 256;

        /// <summary>
        /// The list of batch items to process.
        /// </summary>
        private readonly List<BatchableItem> _batchItemList;

        /// <summary>
        /// The available SpriteBatchItem queue so that we reuse these objects when we can.
        /// </summary>
        private readonly Queue<SpriteBatchItem> _freeBatchItemQueue;

        /// <summary>
        /// Vertex index array. The values in this array never change.
        /// </summary>
        private short[] _index;

        private VertexPositionColorTexture[] _vertexArray;

        public SpriteBatcher()
        {
            _batchItemList = new List<BatchableItem> (InitialBatchSize);
            _freeBatchItemQueue = new Queue<SpriteBatchItem> (InitialBatchSize);

            EnsureArrayCapacity (InitialBatchSize);
        }

        /// <summary>
        /// Create an instance of SpriteBatchItem if there is none available in the free item queue. Otherwise,
        /// a previously allocated SpriteBatchItem is reused.
        /// </summary>
        /// <returns></returns>
        public SpriteBatchItem CreateBatchItem()
        {
            SpriteBatchItem item;
            if (_freeBatchItemQueue.Count > 0)
                item = _freeBatchItemQueue.Dequeue ();
            else
                item = new SpriteBatchItem ();
            _batchItemList.Add (item);
            return item;
        }

        public OSpriteBatchGroup CreateBatchGroup()
        {
            var group = new OSpriteBatchGroup (_freeBatchItemQueue);
            _batchItemList.Add (group);
            return group;
        }

        /// <summary>
        /// Resize and recreate the missing indices for the index and vertex position color buffers.
        /// </summary>
        /// <param name="numBatchItems"></param>
        private void EnsureArrayCapacity(int numBatchItems)
        {
            int neededCapacity = 6 * numBatchItems;
            if (_index != null && neededCapacity <= _index.Length)
            {
                // Short circuit out of here because we have enough capacity.
                return;
            }
            short[] newIndex = new short[6 * numBatchItems];
            int start = 0;
            if (_index != null)
            {
                _index.CopyTo (newIndex, 0);
                start = _index.Length / 6;
            }
            for (var i = start; i < numBatchItems; i++)
            {
                /*
                 *  TL    TR
                 *   0----1 0,1,2,3 = index offsets for vertex indices
                 *   |   /| TL,TR,BL,BR are vertex references in SpriteBatchItem.
                 *   |  / |
                 *   | /  |
                 *   |/   |
                 *   2----3
                 *  BL    BR
                 */
                // Triangle 1
                newIndex[i * 6 + 0] = (short)(i * 4);
                newIndex[i * 6 + 1] = (short)(i * 4 + 1);
                newIndex[i * 6 + 2] = (short)(i * 4 + 2);
                // Triangle 2
                newIndex[i * 6 + 3] = (short)(i * 4 + 1);
                newIndex[i * 6 + 4] = (short)(i * 4 + 3);
                newIndex[i * 6 + 5] = (short)(i * 4 + 2);
            }
            _index = newIndex;

            _vertexArray = new VertexPositionColorTexture[4 * numBatchItems];
        }

        public void ExpandBatchItemList()
        {
            // This is slow as balls, but I'm getting it done for the JAM
            for (var i = 0; i < _batchItemList.Count; i++)
            {
                var group = _batchItemList[i] as OSpriteBatchGroup;
                if (group != null)
                {
                    _batchItemList.RemoveAt (i);

                    foreach (SpriteBatchItem item in group.GroupItems)
                    {
                        _batchItemList.Insert (i++, item);
                    }
                    i--;
                }
            }
        }

        public void DrawBatch (SpriteSortMode sortMode, GraphicsDevice device)
        {
            // nothing to do
            if (_batchItemList.Count == 0)
                return;

            // sort the batch items
            switch (sortMode)
            {
                case SpriteSortMode.Texture:
                    _batchItemList.Sort (CompareTexture);
                    break;
                case SpriteSortMode.FrontToBack:
                    _batchItemList.Sort (CompareDepth);
                    break;
                case SpriteSortMode.BackToFront:
                    _batchItemList.Sort (CompareReverseDepth );
                    break;
            }

            // Currently we're storing Batchables which could
            // be both BatchItems and BatchGroups. Now that
            // we're sorted (The group is sorted as a whole),
            // we expand the batch groups which flatterns the tree
            ExpandBatchItemList();

            // Determine how many iterations through the drawing code we need to make
            int batchIndex = 0;
            int batchCount = _batchItemList.Count;
            // Iterate through the batches, doing short.MaxValue sets of vertices only.
            while (batchCount > 0)
            {
                // setup the vertexArray array
                var startIndex = 0;
                var index = 0;
                Texture2D tex = null;

                int numBatchesToProcess = batchCount;
                if (numBatchesToProcess > MaxBatchSize)
                {
                    numBatchesToProcess = MaxBatchSize;
                }
                EnsureArrayCapacity (numBatchesToProcess);
                // Draw the batches
                for (int i = 0; i < numBatchesToProcess; i++, batchIndex++)
                {
                    // Only SpriteBatchItem's now, since we've expanded the list
                    SpriteBatchItem item = (SpriteBatchItem)_batchItemList[batchIndex];
                    
                    // if the texture changed, we need to flush and bind the new texture
                    var shouldFlush = !ReferenceEquals (item.Texture, tex);
                    if (shouldFlush)
                    {
                        FlushVertexArray (device, startIndex, index);

                        tex = item.Texture;
                        startIndex = index = 0;
                        device.Textures[0] = tex;
                    }

                    // store the SpriteBatchItem data in our vertexArray
                    _vertexArray[index++] = item.vertexTL;
                    _vertexArray[index++] = item.vertexTR;
                    _vertexArray[index++] = item.vertexBL;
                    _vertexArray[index++] = item.vertexBR;

                    // Release the texture and return the item to the queue.
                    item.Texture = null;
                    _freeBatchItemQueue.Enqueue (item);
                }
                // flush the remaining vertexArray data
                FlushVertexArray (device, startIndex, index);
                // Update our batch count to continue the process of culling down large batches
                batchCount -= numBatchesToProcess;
            }
            _batchItemList.Clear ();
        }

        /// <summary>
        /// Sends the triangle list to the graphics device. Here is where the actual drawing starts.
        /// </summary>
        /// <param name="start">Start index of vertices to draw. Not used except to compute the count of vertices to draw.</param>
        /// <param name="end">End index of vertices to draw. Not used except to compute the count of vertices to draw.</param>
        private void FlushVertexArray(GraphicsDevice device, int start, int end)
        {
            if (start == end)
                return;

            var vertexCount = end - start;

            device.DrawUserIndexedPrimitives (
                 PrimitiveType.TriangleList,
                 _vertexArray,
                 0,
                 vertexCount,
                 _index,
                 0,
                 (vertexCount / 4) * 2,
                 VertexPositionColorTexture.VertexDeclaration);
        }

        int CompareTexture(BatchableItem a, BatchableItem b)
        {
            int hashA = a.Texture.GetHashCode();
            int hashB = b.Texture.GetHashCode();

            if (hashA < hashB)
                return -1;
            if (hashA > hashB)
                return 1;

            return 0;
        }

        int CompareDepth(BatchableItem a, BatchableItem b)
        {
            return a.Depth.CompareTo (b.Depth);
        }

        int CompareReverseDepth(BatchableItem a, BatchableItem b)
        {
            return b.Depth.CompareTo (a.Depth);
        }
    }

    public interface BatchableItem
    {
        float Depth { get; }
        Texture2D Texture { get; }
    }
}
