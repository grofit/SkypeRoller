using System;
using System.Drawing;
using System.Windows.Forms;
using SkypeRoller.Properties;
using Application = System.Windows.Forms.Application;

namespace SkypeRoller
{
    public partial class RollerTrayForm : Form
    {
        private const int FiveSeconds = (1000 * 5);

        private SkypeMediator skypeMediator;

        private NotifyIcon  trayIcon;
        private ContextMenu trayMenu;
        private Icon skypeProblemIcon, skypeOkIcon;
        
        public RollerTrayForm()
        {
            skypeMediator = new SkypeMediator();

            CreateTrayMenu();
            CreateTrayIcon();
        
            InitializeComponent();
        }

        public void DisplayBindingSuccessMessage()
        {
            trayIcon.Icon = skypeOkIcon;
            trayIcon.ShowBalloonTip(FiveSeconds, "Success binding to skype",
                                        "PHEW! Rolled a 20 technology check and have bound to skype, Dice rolls commence!",
                                        ToolTipIcon.Info);
        }

        public void DisplayBindingProblemMessage()
        {
            trayIcon.Icon = skypeProblemIcon;
            trayIcon.ShowBalloonTip(FiveSeconds, "Problem binding to skype", 
                "Skype is having a hissy fit, we are trying to re-bind... no dice for you...", 
                ToolTipIcon.Warning);
        }

        protected void CreateTrayMenu()
        {
            trayMenu = new ContextMenu();

            var helpMenuItem = new MenuItem("Help!", OnRollerHelp);
            var exitMenuItem = new MenuItem("Exit", OnRollerExit);

            trayMenu.MenuItems.Add(helpMenuItem);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add(exitMenuItem);
        }

        protected void CreateTrayIcon()
        {
            skypeOkIcon = new Icon(Resources.joystick, 16, 16);
            skypeProblemIcon = new Icon(Resources.joystick_error, 16, 16);

            trayIcon = new NotifyIcon();
            trayIcon.Text = Resources.TrayName;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            trayIcon.Icon = skypeOkIcon;
        }

        private void OnRollerExit(object sender, EventArgs eventArgs)
        { Application.Exit(); }

        private void OnRollerHelp(object sender, EventArgs eventArgs)
        { trayIcon.ShowBalloonTip(FiveSeconds * 2, "Help!", Resources.TrayTooltip, ToolTipIcon.None); }

        protected override void OnLoad(EventArgs e)
        {
            Visible       = false; // Hide form window
            ShowInTaskbar = false; // Remove from taskbar

            skypeMediator.StartMediation();
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            trayIcon.Dispose();
            skypeMediator.StopMediation();
            base.OnClosed(e);
        }

    }
}
