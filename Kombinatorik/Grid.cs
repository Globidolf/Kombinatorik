using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kombinatorik {
	
	enum Direction {
		L,
		T,
		R,
		B
	}
	class Path {
		public int width { get { return 1 + (D1 == Direction.R ? 1 + D2 == Direction.R ? 1 : 0 : 0); } }
		public Direction D1, D2;
		public static Path Initial { get { return new Path { D1 = Direction.R, D2 = Direction.T }; } }
		public bool next() {
			if (D2 == Direction.B) {
				if (D1 == Direction.B) return false;
				D2 = Direction.L;
				D1++;
			} else D2++;
			if (D2 == D1 - 2) D2++; // no top-bottom or left-right combinations allowed
			return true;
		}
		public override string ToString() {
			return D1 + "|" + D2;
		}
	}

	class Grid {

		public DateTime Start, End;
		

		public Form Parent { get; set; }
		public event Action<int> UpdateCallback;

		public event Action<int> ResultCallback;

		bool[] grid;
		int width, height, maxiterations;
		public Grid(int width, int height) {
			this.width = width;
			this.height = height;
			maxiterations = width * height / 3;
			grid = new bool[width * height];
		}

		public async Task<int> combinations2(int pos = 0, int iteration = 0) {
			if (iteration == 0) Start = DateTime.Now;
			if (iteration == maxiterations) return 1;
			int sum = 0;
			Path p = Path.Initial;
			if (pos >= width * height) return 0;

			while (grid[pos]) {
				pos++;
				if (pos >= width * height) return 0;
			}
			if (applySpecial(pos)) {
				sum += await combinations2(pos+2, iteration + 1);
				revertSpecial(pos);
				if (iteration == 0 && UpdateCallback != null)
					Parent.Invoke(UpdateCallback, sum);
			}
			do {
				if (apply(p, pos)) {
					sum += await combinations2(pos+1, iteration + 1);
					revert(p, pos);
					if (iteration == 0 && UpdateCallback != null)
						Parent.Invoke(UpdateCallback, sum);
				}
			} while (p.next());

			if (iteration == 0) End = DateTime.Now;
			if (iteration == 0 && ResultCallback != null)
				Parent.Invoke(ResultCallback, sum);
			return sum;
		}

		public async Task<int> combinations(int x = 0, int y = 0, int iteration = 0) {
			if (iteration == 0) Start = DateTime.Now;
			if (iteration == maxiterations) {
				return 1;
			}
			int sum = 0;
			Path p = Path.Initial;
			if (x >= width) {
				if (++y == height) return 0;
				x = 0;
			}
			while (grid[x+ y*width]) {
				x++;
				if (x >= width) {
					if (++y == height) return 0;
					x = 0;
				}
			}
			if (applySpecial(x, y)) {
				sum += await combinations(x + 2, y, iteration + 1);
				revertSpecial(x, y);
				if (iteration == 0 && UpdateCallback != null)
					Parent.Invoke(UpdateCallback,sum);
			}
			do {
				if (apply(p, x, y)) {
					sum += await combinations(x+1, y, iteration + 1);
					revert(p, x, y);
					if (iteration == 0 && UpdateCallback != null)
						Parent.Invoke(UpdateCallback, sum);
				}
			} while (p.next());

			if (iteration == 0) End = DateTime.Now;
			if (iteration == 0 && ResultCallback != null)
				Parent.Invoke(ResultCallback, sum);
			return sum;
		}

		void revertSpecial(int x, int y) {
			// revert state
			grid[x + y * width] = grid[x+1 + y * width] = grid[x + (y+1) * width ] = false;
		}

		void revertSpecial(int pos) {
			grid[pos] = grid[pos + 1] = grid[pos + height] = false;
		}
		bool applySpecial(int x, int y) {
			// verify location and check coordinates
			if (grid[x + y * width] || x + 1 >= width || y + 1 >= height || grid[x + (y + 1)*width] || grid[x+1 + y * width]) return false;
			// apply state
			grid[x + y * width] = grid[x+ (y+1)*width] = grid[x+1+ y*width] = true;
			// success
			return true;
		}

		bool applySpecial(int pos) {
			if (pos < 0 || pos + width >= width * height ||
				grid[pos+1] || grid[pos+width]) return false;
			return (grid[pos] = grid[pos+1] = grid[pos+height] = true);
		}
		bool apply(Path p, int pos) {
			int pos1 = pos, pos2;
			switch (p.D1) {
				case Direction.B: pos1 += width; break;
				case Direction.T: pos1 -= width; break;
				case Direction.L: pos1 -= 1; break;
				case Direction.R: pos1 += 1; break;
			}
			pos2 = pos1;
			switch (p.D1) {
				case Direction.B: pos2 += width; break;
				case Direction.T: pos2 -= width; break;
				case Direction.L: pos2 -= 1; break;
				case Direction.R: pos2 += 1; break;
			}
			if (pos1 < 0 || pos2 < 0 || pos1 >= width * height || pos2 >= width * height ||
				grid[pos1] || grid[pos2]) return false;
			return (grid[pos] = grid[pos1] = grid[pos2] = true);
		}

		bool apply(Path p, int x, int y) {
			// assign coordinates
			int x1 = x + (p.D1 == Direction.L ? -1 : p.D1 == Direction.R ? 1 : 0);
			int x2 = x1 + (p.D2 == Direction.L ? -1 : p.D2 == Direction.R ? 1 : 0);
			int y1 = y + (p.D1 == Direction.T ? -1 : p.D1 == Direction.B ? 1 : 0);
			int y2 = y1 + (p.D2 == Direction.T ? -1 : p.D2 == Direction.B ? 1 : 0);
			// validate coordinates
			if (x1 < 0 || x2 < 0 || x1 >= width || x2 >= width || y1 < 0 || y2 < 0 || y1 >= height || y2 >= height) return false;
			// check coordinates
			if (grid[x1 + y1 * width] || grid[x2+ y2 * width]) return false;
			// apply state
			grid[x+ y * width] = grid[x1+ y1 * width] = grid[x2+ y2 * width] = true;
			// success
			return true;
		}

		void revert(Path p, int x, int y) {
			// assign coordinates
			int x1 = x + (p.D1 == Direction.L ? -1 : p.D1 == Direction.R ? 1 : 0);
			int x2 = x1 + (p.D2 == Direction.L ? -1 : p.D2 == Direction.R ? 1 : 0);
			int y1 = y + (p.D1 == Direction.T ? -1 : p.D1 == Direction.B ? 1 : 0);
			int y2 = y1 + (p.D2 == Direction.T ? -1 : p.D2 == Direction.B ? 1 : 0);
			// revert state
			grid[x+ y * width] = grid[x1+ y1 * width] = grid[x2+ y2 * width] = false;
		}
		void revert(Path p, int pos) {
			int pos1 = pos, pos2;
			switch (p.D1) {
				case Direction.B: pos1 += width; break;
				case Direction.T: pos1 -= width; break;
				case Direction.L: pos1 -= 1; break;
				case Direction.R: pos1 += 1; break;
			}
			pos2 = pos1;
			switch (p.D1) {
				case Direction.B: pos2 += width; break;
				case Direction.T: pos2 -= width; break;
				case Direction.L: pos2 -= 1; break;
				case Direction.R: pos2 += 1; break;
			}
			grid[pos] = grid[pos1] = grid[pos2] = false;
		}
	}
}
