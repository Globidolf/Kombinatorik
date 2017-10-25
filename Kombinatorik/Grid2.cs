using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombinatorik {
	public class Grid2 {
		
		public event Action ViableCombinationFound;

		[Flags]
		public enum Blocks : byte {
			Empty = 0,
			/// <summary>
			/// Position of the Block
			/// </summary>
			P00 = 0x1,
			/// <summary>
			/// One right of the Position
			/// </summary>
			P10 = 0x1<<1,
			/// <summary>
			/// One below the Position
			/// </summary>
			P01 = 0x1<<2,
			/// <summary>
			/// One below, one right from the Position
			/// </summary>
			P11 = 0x1<<3,
			/// <summary>
			/// Two right of the Position
			/// </summary>
			P20 = 0x1<<4,
			/// <summary>
			/// Two below the Position
			/// </summary>
			P02 = 0x1<<5,
			Vertical =		P00 | P01 | P02,
			Horizontal =	P00 | P10 | P20,
			CTopLeft =		P00 | P10 | P01,
			CBottomLeft =	P00 | P01 | P11,
			CTopRight =		P00 | P10 | P11,
			CBottomRight =	P01 | P10 | P11,
			E = Empty,
			V = Vertical,
			H = Horizontal,
			TL = CTopLeft,
			BL = CBottomLeft,
			TR = CTopRight,
			BR = CBottomRight
		}

		public static readonly List<Blocks> Variations = new List<Blocks>() { Blocks.V, Blocks.H, Blocks.TL, Blocks.TR, Blocks.BL, Blocks.BR };

		Blocks[,] Content;

		private struct HistAction {
			public int X, Y;
			public Blocks BlockBefore, BlockAfter;
		}

		private List<HistAction> History;
		private int PointInHistory;

		public int ViableCombinations;

		public void Undo() {
			if (History.Count == 0) return;
			HistAction act = History[PointInHistory - 1];
			Content[act.X, act.Y] = act.BlockBefore;
			PointInHistory--; 
		}
		public void Redo() {
			if (History.Count <= PointInHistory) return;
			HistAction act = History[PointInHistory];
			Content[act.X, act.Y] = act.BlockAfter;
			PointInHistory++;
		}

		public Grid2(int width, int height) {
			Width = width;
			Height = height;
			Content = new Blocks[width, height];
			History = new List<HistAction>();
		}

		public void Recourse2(int layer = 0, int x = 0, int y = 0) {
			int xbase = x;
			int ybase = y;
			if (layer == MaxBlockCount) {
				ViableCombinations++;
				//ViableCombinationFound?.Invoke();
				return;
			}
			foreach (Blocks blck in Variations) {
				x = xbase;
				y = ybase;
				while(y < Height) {
					while(x < Width) {
						if (CanPlace(blck, x, y)) {
							this[x, y] = blck;
							Recourse2(layer + 1, x, y);
							Undo();
						}
						x++;
					}
					x = 0;
					y++;
				}
			}
		}

		private void Recourse(int x = 0, int y = 0, int layer = 0) {
			if (!IsStateValid) return;
			if (x >= Width) { // next row if row completed
				x = 0;
				y++;
				if (y >= Height) return;
			}
			Recourse(x + 1, y, layer + 1);
			Variations.ForEach(current =>
			{
				if (CanPlace(current, x, y)) {
					this[x, y] = current;
					if (IsFull) ViableCombinations++;
					else 
						Recourse(
							x + (current.HasFlag(Blocks.P20) ? 2 : 1), 
							y,
							layer + 1);
					Undo();
				}
			});
		}

		public bool IsStateValid { get {
				if (IsFull) return true;
				for( int i = Width-1 ; i >= 0 ; i--)
					for(int j = Height-1 ; j >= 0 ; j--)
						if (Variations.Any(blck => CanPlace(blck, i, j))) return true;
				return false;
			}
		}

		public bool IsFull {
			get {
				for (int i = 0 ; i < Width ; i++)
					for (int j = 0 ; j < Height ; j++)
						if (this[i, j] == Blocks.E) return false;
				return true;
			}
		}

		public void Revert() { while (PointInHistory > 0) Undo(); }
		
		public int MaxBlockCount { get { return ((Width * Height) / 3); } }

		public int Width { get; }
		public int Height { get; }

		public bool IsInBounds(Blocks b, int x, int y) {
			int rwidth = Width - (b.HasFlag(Blocks.P20) ? 2 : (b.HasFlag(Blocks.P10) || b.HasFlag(Blocks.P11)) ? 1 : 0) - 1;
			int rheight = Height - (b.HasFlag(Blocks.P02) ? 2 : (b.HasFlag(Blocks.P01) || b.HasFlag(Blocks.P11)) ? 1 : 0) - 1;
			return !(x > rwidth ||
				y >  rheight ||
				x < 0 || y < 0);
		}

		public bool CanPlace(Blocks b, int x, int y) {
			// check all used cells for obstructions. if any of these is true, the block cannot be placed.
			return !(!IsInBounds(b,x,y) ||
					 (b.HasFlag(Blocks.P00) && this[x, y] != Blocks.E) ||
					 (b.HasFlag(Blocks.P10) && this[x + 1, y] != Blocks.E) ||
					 (b.HasFlag(Blocks.P20) && this[x + 2, y] != Blocks.E) ||
					 (b.HasFlag(Blocks.P11) && this[x + 1, y + 1] != Blocks.E) ||
					 (b.HasFlag(Blocks.P01) && this[x, y + 1] != Blocks.E) ||
					 (b.HasFlag(Blocks.P02) && this[x, y + 2] != Blocks.E));
		}

		public Blocks this[int x, int y] {
			get {
				//Out of bounds
				if (x >= Width || y >= Height || x < 0 || y < 0) throw new Exception("Invalid position!");

				//Block at current position fills current position
				if (Content[x, y].HasFlag(Blocks.P00)) return Content[x, y];

				//No block at current position OR the block does not have the P00 flag
				if (x > 0) {
					if (x > 1 && Content[x - 2, y].HasFlag(Blocks.P20)) return Content[x - 2, y];           // check two spaces left
					if (Content[x - 1, y].HasFlag(Blocks.P10)) return Content[x - 1, y];                    // check one space left
					if (y > 0 && Content[x - 1, y - 1].HasFlag(Blocks.P11)) return Content[x - 1, y - 1];   // check one space left, one above
				}
				if (y > 0) {
					if (Content[x, y - 1].HasFlag(Blocks.P01)) return Content[x, y - 1];					// check one space above
					if (y > 1 && Content[x, y - 2].HasFlag(Blocks.P02)) return Content[x, y - 2];			// check two spaces above
				}

				// position is free
				return Blocks.E;
			}
			set {
				if (History.Count != PointInHistory) History.RemoveRange(PointInHistory, History.Count - PointInHistory);
				History.Add(new HistAction() { BlockBefore = Content[x, y], BlockAfter = value, X = x, Y = y });
				PointInHistory++;
				Content[x, y] = value;
			}
		}

		public override string ToString() {
			string result = "";

			for (int y = 0 ; y < Height ; y++) {
				for (int x = 0 ; x < Width ; x++) {
					char c;
					switch (this[x, y]) {
						case Blocks.E: c = '0'; break;
						case Blocks.V: c = '1'; break;
						case Blocks.H: c = '2'; break;
						case Blocks.TL: c = '3'; break;
						case Blocks.TR: c = '4'; break;
						case Blocks.BL: c = '5'; break;
						case Blocks.BR: c = '6'; break;
						default: throw new Exception("Scripting Error");
					}
					result += c;
				}
				if (y != Height-1) result += ",\n";
			}

			return result;
		}
	}
}
