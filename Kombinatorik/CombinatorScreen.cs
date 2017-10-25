using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static Kombinatorik.Grid2;

namespace Kombinatorik {
	public partial class CombinatorScreen : Form {

		Grid2 grid;

		public CombinatorScreen() {
			InitializeComponent();
			ele_nud_gridx.Value = Properties.Settings.Default.gridX;
			ele_nud_gridy.Value = Properties.Settings.Default.gridY;

			//gets largest available screen
			Screen current = Screen.AllScreens.OrderByDescending(scr => scr.Bounds.Width * scr.Bounds.Height).First();
			ele_pb_display.Image = new Bitmap(current.Bounds.Width, current.Bounds.Height);
			RunCalc();
		}



		public void RunCalc() {
			grid = new Grid2((int) ele_nud_gridx.Value, (int) ele_nud_gridy.Value);
			grid.ViableCombinationFound += updateDisplay;
			grid.Recourse2();
			updateDisplay();
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			base.OnFormClosing(e);
			Properties.Settings.Default.gridX = (byte) ele_nud_gridx.Value;
			Properties.Settings.Default.gridY = (byte) ele_nud_gridy.Value;
			Properties.Settings.Default.Save();
		}

		protected void updateDisplay() {

			ele_l_result.Text = grid.ViableCombinations.ToString();
			Image img = ele_pb_display.Image;
			int i_width = img.Width / grid.Width;
			int i_heigth = img.Height / grid.Height;
			Graphics G = Graphics.FromImage(img);

			G.Clear(Color.Black);

			for (int X = 0 ; X < grid.Width ; X++) {
				for (int Y = 0 ; Y < grid.Height ; Y++) {
					Blocks b = grid[X,Y];
					if (b != Blocks.E) {
						Rectangle rekt = new Rectangle(X*i_width, Y * i_heigth, i_width, i_heigth);
						Brush br;
						switch (b) {
							case Blocks.V: br = Brushes.Yellow; break;
							case Blocks.H: br = Brushes.Red; break;
							case Blocks.TL: br = Brushes.Cyan; break;
							case Blocks.TR: br = Brushes.Purple; break;
							case Blocks.BL: br = Brushes.Green; break;
							case Blocks.BR: br = Brushes.Blue; break;
							default: br = Brushes.White; break;
						}
						G.FillRectangle(br, rekt);
					}
				}
			}

			for (int i = i_width * grid.Width ; i >= 1 ; i -= i_width)
				G.DrawLine(Pens.Black, i, 0, i, img.Height);

			for (int i = i_heigth * grid.Height ; i >= 1 ; i -= i_heigth)
				G.DrawLine(Pens.Black, 0, i, img.Width, i);

			G.Dispose();
			ele_l_result.Invalidate();
			ele_pb_display.Invalidate();
		}

		private void ele_btn_start_Click(object sender, System.EventArgs e) {
			RunCalc();
		}
	}
}
