using System; 
using System.Data;
using System.Drawing; 
using System.Text;
using System.Windows.Forms;

namespace WindGoes.Forms
{
	public partial class VanishingForm : Form
	{
		Form parentFrom;
        Size originalSize = new Size();
        Point originalLocation = new Point();
        double radio = 1.0f;
        int count = -1;
        int vanishStyle = 0;
        /// <summary>
        /// 消失的样式。
        /// </summary>
        public int VanishStyle
        {
            get { return vanishStyle; }
            set { vanishStyle = value; }
        }

        bool showParentForm = false;
        /// <summary>
        /// 显示完以后是否显示主窗体。
        /// </summary>
        public bool ShowParentForm
        {
            get { return showParentForm; }
            set { showParentForm = value; }
        }


        public VanishingForm() { InitializeComponent(); }
		public VanishingForm(Form f)
		{
			InitializeComponent();
            TransparencyKey = Color.FromArgb(0, 0, 1);
            this.BackColor = TransparencyKey;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

			parentFrom = f;
			LoadData();
		}
        Bitmap bmp = null;

		public void LoadData()
		{
			bmp = new Bitmap(parentFrom.Width, parentFrom.Height);
			Graphics g = Graphics.FromImage(bmp);
			g.CopyFromScreen(new Point(parentFrom.Left, parentFrom.Top), new Point(0, 0), parentFrom.Size);
            
			this.Top = parentFrom.Top;
			this.Left = parentFrom.Left;
			this.Width = parentFrom.Width;
            this.Height = parentFrom.Height;


            this.StartPosition = FormStartPosition.Manual;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImage = bmp;

			originalLocation = Location;
			originalSize = this.Size;
		}

		public void Start()
		{
			timer1.Start();
			this.Show();
		}

        public void Start(int s)
        {
            vanishStyle = s;
            timer1.Start();
            this.Show();
        }

        public void Transform()
        {
            switch (vanishStyle)
            {
                case 0:  
                    LargeAway(); 
                    break;
                case 1: 
                    UpAway();
                    break;
                case 3:

                    break;
                default: break;
            }
            angel += 1f;
        }

		private void timer1_Tick(object sender, EventArgs e)
		{
            Transform();
		}



        public void LargeAway()
        {
            count++;
            if (count < 0)
            {
                return;
            }
            if (count < 100 && radio < 1.02)
            {
                radio += 0.01f;
                this.Width = (int)(originalSize.Width * radio);
                this.Height = (int)(originalSize.Height * radio);
                Top = originalLocation.Y - (Height - originalSize.Height) / 2;
                Left = originalLocation.X - (Width - originalSize.Width) / 2;
                Opacity -= 0.1;
            }
            else
            {
                Opacity -= 0.05;
            }

            if (Opacity <= 0.05)
            {
                Close();
            }
        }

        public void UpAway()
        {
            Top--;
            Opacity -= 0.05;
            if (Opacity <= 0.05)
            {
                Close();
            }
        }


        private void VanishingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (showParentForm)
            {
                parentFrom.Show();
            }
            else
            {
                //parentFrom.Close();
            }
        }

        float angel = 0;
        private void VanishingForm_Paint(object sender, PaintEventArgs e)
        {
       
        }
	}
}
