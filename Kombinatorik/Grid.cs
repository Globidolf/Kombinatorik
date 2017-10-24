using System;
using System.Collections.Generic;

namespace Kombinatorik {
	class Grid {

		public int W { get; }
		public int H { get; }

		internal void AddBlock(Block blck) {
			Content.Add(blck);
		}

		public Grid(int Width, int Height) {
			W = Width;
			H = Height;
			Content = new List<Block>();
			Random rng = new Random();
			Block blck;
			for(int i = 0 ; i < Width * Height ; i++) {
				switch (rng.Next(6)) {
					case 0: blck = Block.B1x3; break;
					case 1: blck = Block.B2x2A; break;
					case 2: blck = Block.B2x2B; break;
					case 3: blck = Block.B2x2C; break;
					case 4: blck = Block.B2x2D; break;
					default: blck = Block.B3x1; break;
				}
				int x = rng.Next(W - blck.Layout.GetLength(0) + 1);
				int y = rng.Next(H - blck.Layout.GetLength(1) + 1);
				blck.Apply(this, x, y);
			}
		}

		private List<Block> Content { get; }
		
		public Block this[int x, int y] {
			get {
				//iterate each block and check if it occupies the position
				foreach (Block blck in Content)
					if (blck.Contains(x, y))
						return blck;
				return null;
			}
		}
		
	}
}
