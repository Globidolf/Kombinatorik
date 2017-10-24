using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace Kombinatorik {
	class Grid {

		public int W { get; }
		public int H { get; }

		internal void AddBlock(Block blck) {
			Content.Add(blck);
		}

		public Grid(Point p) : this(p.X, p.Y) { }
		public Grid(int Width, int Height) {
			List<Block> availables;
			Random rng = new Random();
			W = Width;
			H = Height;
			Content = new List<Block>();
			List<Block> fillwith = new List<Block>(Block.BAll);
			fillwith.Reverse();
			while((availables = fillwith.FindAll(blck => PotentialPlacement(blck).Count > 0)).Count > 0) {
				List<Point> free = FreeLocations;
				Block selected = availables[rng.Next(availables.Count)];
				List<Point> temp = PotentialPlacement(selected);
				Point target = temp[rng.Next(temp.Count)];
				selected.Apply(this, target);
			}
		}

		private List<Block> Content { get; }

		public Block this[Point P] { get { return this[P.X, P.Y]; } }
		public Block this[int x, int y] {
			get {
				//iterate each block and check if it occupies the position
				foreach (Block blck in Content)
					if (blck.Contains(x, y))
						return blck;
				return null;
			}
		}
		
		public List<Point> FreeLocations { get {
				List<Point> result = new List<Point>();
				IterateGrid((x, y) => { if (this[x, y] == null) result.Add(new Point(x, y)); } );
				return result;
			} }

		public List<Point> PotentialPlacement(Block blck) {
			List<Point> result = new List<Point>();
			IterateGrid((x, y) =>
			{
				bool posValid = true;
				blck.IterateLayout((x2, y2, occ) =>
				{
					if (occ) {
						if (x + x2 >= W || y + y2 >= H || this[x + x2, y + y2] != null) // check bounds and occupation
							posValid = false;
					}// else continue
				});
				if (posValid) // position is valid, add to list
					result.Add(new Point(x, y));
			});
			return result;
		}

		public void IterateGrid(GridIterationDelegate action) {
			for (int x = 0 ; x < W ; x++)
				for (int y = 0 ; y < H ; y++)
					action(x, y);
		}

		public delegate void GridIterationDelegate(int X, int Y);

	}
}
