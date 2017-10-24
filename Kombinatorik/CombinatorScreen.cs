using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kombinatorik {
	public partial class CombinatorScreen : Form {

		Grid grid;

		public CombinatorScreen() {
			InitializeComponent();
			ele_nud_gridx.Value = Properties.Settings.Default.gridX;
			ele_nud_gridy.Value = Properties.Settings.Default.gridY;
			ele_nud_gridx.ValueChanged += (obj, arg) => updateDisplay();
			ele_nud_gridy.ValueChanged += (obj, arg) => updateDisplay();

			//gets largest available screen
			Screen current = Screen.AllScreens.OrderByDescending(scr => scr.Bounds.Width * scr.Bounds.Height).First();
			ele_pb_display.Image = new Bitmap(current.Bounds.Width, current.Bounds.Height);
			updateDisplay();
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			base.OnFormClosing(e);
			Properties.Settings.Default.gridX = (byte) ele_nud_gridx.Value;
			Properties.Settings.Default.gridY = (byte) ele_nud_gridy.Value;
			Properties.Settings.Default.Save();
		}

		protected void updateDisplay() {
			grid = new Grid((int)ele_nud_gridx.Value, (int)ele_nud_gridy.Value);

			Image img = ele_pb_display.Image;
			int i_width = img.Width / grid.W;
			int i_heigth = img.Height / grid.H;
			Graphics G = Graphics.FromImage(img);

			G.Clear(Color.Black);
			
			if (grid.W * grid.H % 3 == 0) {
				// Valid Grid Size

				for (int X = 0 ; X <= grid.W ; X++) {
					for (int Y = 0 ; Y <= grid.H ; Y++) {
						Block b = grid[X,Y];
						if (b != null) {
							Rectangle rekt = new Rectangle(X*i_width, Y * i_heigth, i_width, i_heigth);
							Pen p = new Pen(b.Color);
							Brush br = p.Brush;
							G.FillRectangle(br, rekt);
							br.Dispose();
							p.Dispose();
						}
					}
				}

				for (int i = i_width * grid.W ; i >= 1 ; i -= i_width)
					G.DrawLine(Pens.Black, i, 0, i, img.Height);

				for (int i = i_heigth * grid.H ; i >= 1 ; i -= i_heigth)
					G.DrawLine(Pens.Black, 0, i, img.Width, i);

				G.Dispose();
			} else {
				// Invalid Grid Size
				Font f = new Font(FontFamily.GenericSansSerif, 12 * ((float)ele_pb_display.Image.Width / ele_pb_display.Width));
				G.DrawString("Cannot be Filled. "+
					"("+grid.W+"*"+grid.H+"="+grid.W * grid.H+";" + 
					grid.W * grid.H + "/3 = Rest:"+ grid.W * grid.H % 3 +")", f, Brushes.White, 20,20);
				f.Dispose();
			}
			ele_pb_display.Invalidate();
		}

		private void ele_btn_start_Click(object sender, System.EventArgs e) {
			updateDisplay();
		}
	}
}
