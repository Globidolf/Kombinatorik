using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kombinatorik {
	public partial class CombinatorScreen : Form {

		Grid grid;

		public CombinatorScreen() {
			InitializeComponent();
			ele_nud_gridx.Value = Properties.Settings.Default.gridX;
			ele_nud_gridy.Value = Properties.Settings.Default.gridY;

			//gets largest available screen
			Screen current = Screen.AllScreens.OrderByDescending(scr => scr.Bounds.Width * scr.Bounds.Height).First();
			ele_pb_display.Image = new Bitmap(current.Bounds.Width, current.Bounds.Height);
			//RunCalc();
		}

		public async Task RunCalc() {
			grid = new Grid((int) ele_nud_gridx.Value, (int) ele_nud_gridy.Value);
			grid.Parent = this;
			grid.ResultCallback += (combis) => {
				ele_l_result.Text = "!" + combis.ToString();
				ele_l_result.Invalidate();
				Graphics G = Graphics.FromImage(ele_pb_display.Image);

				G.Clear(Color.Black);

				G.DrawString("Duration: " + (grid.End - grid.Start), new Font(FontFamily.Families[0],120, FontStyle.Regular), Brushes.White, 20, 20);

				G.Dispose();
				ele_pb_display.Invalidate();
			};
			grid.UpdateCallback += (combis) => {
				ele_l_result.Text = "..."+combis.ToString();
				ele_l_result.Invalidate();
			};
			await grid.combinations();
		}

		protected override void OnFormClosing(FormClosingEventArgs e) {
			base.OnFormClosing(e);
			Properties.Settings.Default.gridX = (byte) ele_nud_gridx.Value;
			Properties.Settings.Default.gridY = (byte) ele_nud_gridy.Value;
			Properties.Settings.Default.Save();
		}

		private void ele_btn_start_Click(object sender, System.EventArgs e) {
			Task.Run(RunCalc);
		}
	}
}
