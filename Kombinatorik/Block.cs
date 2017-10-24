using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombinatorik {
	class Block {
		public Grid Parent { get; set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		private const bool F = false;
		private const bool T = true;
		public bool[,] Layout { get; set; }
		public Color Color { get; }
		public static Block B1x3 {
			get {
				return new Block(
					new bool[,]{
						{ T },
						{ T },
						{ T }
					}, Color.Yellow);
			}
		}
		public static Block B3x1 {
			get {
				return new Block(
					new bool[,]{
						{ T , T , T }
					}, Color.Orange);
			}
		}
		public static Block B2x2A {
			get {
				return new Block(
					new bool[,] {
						{ T, T },
						{ T, F }
					}, Color.Cyan);
			}
		}
		public static Block B2x2B {
			get {
				return new Block(
					new bool[,] {
						{ T, T },
						{ F, T }
					}, Color.Blue);
			}
		}
		public static Block B2x2C {
			get {
				return new Block(
					new bool[,] {
						{ F, T },
						{ T, T }
					}, Color.Purple);
			}
		}
		public static Block B2x2D {
			get {
				return new Block(
					new bool[,] {
						{ T, F },
						{ T, T }
					}, Color.Green);
			}
		}

		public Block(bool[,] layout, Color? c = null) {
			Color = c.HasValue ? c.Value : Color.White;
			Layout = layout;
		}

		public bool Contains(int x, int y) {
			return
				//check bounds
				X <= x && Y <= y &&
				X + Layout.GetLength(0) - 1 >= x &&
				Y + Layout.GetLength(1) - 1 >= y &&
				//return layout mapping
				Layout[x - X, y - Y];
		}

		public bool CanApply(Grid grid, int x, int y) {
			// check if placement is inside bounds
			if (x < 0 || y < 0 || x + Layout.GetLength(0) - 1 > grid.W || y + Layout.GetLength(1) > grid.H)
				return false;
			// Check for conflict of each position
			for (int posX = x ; posX < x + Layout.GetLength(0) ; posX++)
				for (int posY = y ; posY < y + Layout.GetLength(1) ; posY++) {
					//Console.WriteLine("Checking Position [" + posX + ", " + posY + "]. Grid is " grid[pos);
					if (Layout[posX - x, posY - y] && grid[posX, posY] != null)
						return false;
				}
			// no obstructions detected, hence can be applied
			return true;
		}

		public void Apply(Grid grid, int x, int y) {
			if (CanApply(grid, x, y)) {
				X = x;
				Y = y;
				grid.AddBlock(this);
			}
		}

		public override string ToString() {
			string str = "";
			for (int y = 0 ; y < Layout.GetLength(1) ; y++) {
				str += "{ ";
				for (int x = 0 ; x < Layout.GetLength(0) ; x++) {
					str += Layout[x,y] ? "1" : "0";
					if (x != Layout.GetLength(0) - 1) str += ", ";
				}
				str += " }\n";
			}
			return str;
		}

	}
}
