using Neko11V2.behaviors;
using Neko11V2.behaviors.reusable;
using System;
using System.Resources;
// using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neko11V2
{
    public partial class NekoForm : Form
    {
        private readonly System.Windows.Forms.Timer timer;
        private int dx = 2, dy = 2; // movement speed
        public const int ImageUpdateFrequency = 10;

        public readonly Dictionary<string, Image> Images = NImages.LoadImages();
        public readonly List<IBehavior> behaviors = [new FollowMouse(), new RandomMovement(), new LazyCat()];
        public int CurrentBehaviorPos = 0;

        private int TicksSinceImageChange = 0;
        private readonly NotifyIcon TrayIcon;
        private IBehavior CurrentBehavior = new LazyCat();

        public NekoForm()
        {
            InitializeComponent();

            // Make form borderless & transparent
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.BackColor = Color.Lime; // key color
            this.TransparencyKey = Color.Lime;

            // Load image
            this.BackgroundImage = this.Images["awake.ico"];
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.ClientSize = this.BackgroundImage.Size;

            // Setup timer for movement
            timer = new()
            {
                Interval = 30 // ~33 FPS
            };
            timer.Tick += MovePet!;
            timer.Start();

            var trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Cycle Behaviors", null, OnCycleBehaviors);
            trayMenu.Items.Add("");
            trayMenu.Items.Add("Exit", null, OnExit);


            TrayIcon = new()
            {
                Text = "Neko11",
                Icon = Icon.ExtractAssociatedIcon("img/awake.ico"),
                ContextMenuStrip = trayMenu,
                Visible = true
            };

        }

        private void OnExit(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnCycleBehaviors(object? sender, EventArgs e)
        {
            if (CurrentBehaviorPos >= behaviors.Count)
            {
                CurrentBehaviorPos = 0;
            } else
            {
                CurrentBehaviorPos += 1;
            }
            CurrentBehavior = behaviors[CurrentBehaviorPos];
        }

        
        

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_TRANSPARENT = 0x20;
                const int WS_EX_TOOLWINDOW = 0x80;
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW;
                return cp;
            }
        }


        private void MovePet(object sender, EventArgs e)
        {
            CurrentBehavior.MoveAndChooseImage(this, ref dx, ref dy, ref TicksSinceImageChange);
        }
    }
}
