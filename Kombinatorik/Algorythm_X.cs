using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kombinatorik {
	//Positions: 670
	public class Algorythm_X {
		public static int positioncount;
		public const  int columns = 4;
		public const  int rows = 3;
		public const  int cellcount = rows * columns;
		public static bool[] complete;

		public static int combinations { get; private set; }

		public class Position {
			public readonly bool[] cells;
			public Position(bool[] cells) { this.cells = cells; }
			public Position(params ushort[] pos) { cells = new bool[cellcount]; foreach(ushort p in pos) cells[p] = true; }
			public Position(Shape s, ushort pos) {
				cells = new bool[cellcount];
				switch (s) {
					case Shape.Shape_H: cells[pos] = cells[pos + 1] = cells[pos + 2] = true; break;
					case Shape.Shape_V: cells[pos] = cells[pos + columns] = cells[pos + columns + columns] = true; break;
					case Shape.Shape_L1: cells[pos + 1] = cells[pos + columns] = cells[pos] = true; break;
					case Shape.Shape_L2: cells[pos + 1] = cells[pos] = cells[pos + columns + 1] = true; break;
					case Shape.Shape_L3: cells[pos] = cells[pos + columns] = cells[pos + columns + 1] = true; break;
					case Shape.Shape_L4: cells[pos + 1] = cells[pos + columns] = cells[pos + columns + 1] = true; break;
				}
			}

			public override int GetHashCode() {
				int hash = 0;
				for(int i = 0 ; i < cells.Length ; i++) {
					if (cells[i]) hash += unchecked(1 << i);
				}
				hash = unchecked(hash + cells.Length.GetHashCode());
				return hash;
			}

			public override bool Equals(object obj) {
				return GetHashCode() == obj.GetHashCode();
			}

			public Position(params Position[] Amalgam) { cells = Amalgam.Aggregate(new bool[cellcount], (cells, pos) => { cells.combine(pos.cells); return cells; }); }
			public override string ToString() {
				string result = "";

				for(int i = 0 ; i < rows ; i++) {
					result += "{";
					for (int j = 0 ; j < columns ; j++) {
						result += cells[i * columns + j] ? 1 : 0;
					}
					result += "}\n";
				}
				return result;
			}
		}

		public static Position[][] positions;

		public static Position[][] generate() {
			complete = new bool[cellcount];
			complete.Populate(true);
			positioncount = (int)Enum.GetValues(typeof(Shape)).Cast<Shape>().Sum(s => (columns - s.h() + 1) * (rows - s.w() + 1));
			List<Position[]> positions = new List<Position[]>();
			for (int l = 0 ; l < cellcount / 3 ; l++) {
				List<Position> poslist = new List<Position>();
				if (l == 0) { // root
					foreach (Shape s in Enum.GetValues(typeof(Shape)).Cast<Shape>()) {
						int w = 1 + (int) (columns - s.w() ) , h =  1 +(int)(rows - s.h());
						for (int i = 0 ; i < h ; i++) {
							for (int j = 0 ; j < w ; j++) {
								poslist.Add(new Position(s, (ushort) (j + i * columns)));
							}
						}
					}
				} else {
					if (l > 1) {
						for (int i = l - 1 ; i > 0 ; i--) {
							for (int j = 0 ; j < positions[0].Length ; j++) {
								for (int k = 0 ; k < positions[i].Length ; k++) {
									bool[] temp = new bool[cellcount];
									temp.combine(positions[0][j].cells);
									if (temp.combine(positions[i][k].cells))
										poslist.Add(new Position(temp));
								}
							}
						}
					} else {
						for (int j = 0 ; j < positions[0].Length - 1 ; j++) {
							for (int k = j + 1 ; k < positions[0].Length ; k++) {
								bool[] temp = new bool[cellcount];
								temp.combine(positions[0][j].cells);
								if (temp.combine(positions[0][k].cells))
									poslist.Add(new Position(temp));
							}
						}
					}
				}
				positions.Add(poslist.Distinct().ToArray());
			}
			combinations = positions[positions.Count - 1].Length;
			return positions.ToArray();
		}

		public static int count = 0;

		public static void init() {
			DateTime tStart = DateTime.Now;
			positions = generate();
			List<int> hashes = new List<int>();
			foreach (Position[] plist in positions)
				foreach (Position p in plist) {
					int test = p.GetHashCode();
					hashes.Add(test);
				}
			List<int>distinct = hashes.Distinct().ToList();
			DateTime tEnd = DateTime.Now;
			TimeSpan Delta = (tEnd - tStart);
			Console.WriteLine("Positions generated in " + Math.Floor(Delta.TotalHours).ToString("00") + ":" + Delta.Minutes.ToString("00") + ":" + Delta.Seconds.ToString("00") + "'" + Delta.Milliseconds.ToString("0000"));
			/*
			tStart = DateTime.Now;
			calculate(0, new bool[cellcount]);
			tEnd = DateTime.Now;
			Delta = (tEnd - tStart);
			Console.WriteLine("Calculated in " + Math.Floor(Delta.TotalHours).ToString("00") + ":" + Delta.Minutes.ToString("00") + ":" + Delta.Seconds.ToString("00") + "'" + Delta.Milliseconds.ToString("0000") );
			*/
		}

		public static void calculate(int pos, bool[] state) {
			if (state == complete) {
				combinations++;
			} else {
				for (int i = pos ; i < positioncount ; i++) {
					if (state.combine(positions[0][i].cells)) {
						calculate(i+1, state);
						state.undo(positions[0][i].cells);
					}
				}
			}
		}
	}
	public enum Shape : byte {
		/// <summary>
		/// ---
		/// </summary>
		Shape_H,
		/// <summary>
		/// <para>|</para>
		/// <para>|</para>
		/// <para>|</para>
		/// </summary>
		Shape_V,
		/// <summary>
		/// ADia: 1110
		/// <para>1 1</para>
		/// <para>1 0</para>
		/// </summary>
		Shape_L1,
		/// <summary>
		/// ADia: 1011
		/// <para>1 1</para>
		/// <para>0 1</para>
		/// </summary>
		Shape_L2,
		/// <summary>
		/// ADia: 1101
		/// <para>1 0</para>
		/// <para>1 1</para>
		/// </summary>
		Shape_L3,
		/// <summary>
		/// Adia: 0111
		/// <para>0 1</para>
		/// <para>1 1</para>
		/// </summary>
		Shape_L4
	}
	public static class ShapeExtensions {
		public static uint w(this Shape me) {
			if (me == Shape.Shape_H) return 3;
			if (me == Shape.Shape_V) return 1;
			return 2;
		}

		public static uint h(this Shape me) {
			if (me == Shape.Shape_H) return 1;
			if (me == Shape.Shape_V) return 3;
			return 2;
		}

		public static void undo(this bool[] me, bool[] other) { for (i = 0; i < me.Length ; i++) if (other[i]) me[i] = false; }
		static int[] changes;
		static int j, i;
		public static bool combine(this bool[] me, bool[] other) {
			changes = new int[Algorythm_X.cellcount - 3];
			j = 0;
			for (i = 0 ; i < me.Length ; i++) {
				if (me[i] && other[i]) return false;
				if (other[i]) changes[j++] = i;
			} for ( ; j > 0 ; j--) me[changes[j-1]] = true;
			return true;
		}
		public static void Populate<T>(this T[] me, T value) { for (int i = 0 ; i<me.Length ; ++i) me[i] = value;  }
	}
}
