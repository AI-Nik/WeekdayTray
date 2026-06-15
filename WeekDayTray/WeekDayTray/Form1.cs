using System;
using System.Drawing;
using System.Windows.Forms;

namespace WeekDayTray   // замените на имя вашего проекта
{
    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private Timer updateTimer;

        public Form1()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trayIcon = new NotifyIcon();
            trayIcon.Visible = true;
            UpdateTrayIcon();

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Выход", null, (s, ev) => Application.Exit());
            trayIcon.ContextMenuStrip = menu;

            updateTimer = new Timer();
            updateTimer.Interval = 86400000; // 24 часа
            updateTimer.Tick += (s, ev) => UpdateTrayIcon();
            updateTimer.Start();
        }

        private void UpdateTrayIcon()
        {
            string dayName = DateTime.Now.ToString("ddd"); // "Пн", "Вт"...
            trayIcon.Icon = LoadIconFromResources(dayName);
            trayIcon.Text = DateTime.Now.ToString("dddd, d MMMM yyyy");
        }

        private Icon LoadIconFromResources(string dayName)
        {
            // Получаем Bitmap из встроенных ресурсов по имени файла (без расширения)
            // Например, Properties.Resources.Пн
            object obj = Properties.Resources.ResourceManager.GetObject(dayName);
            if (obj is Bitmap bmp)
            {
                return Icon.FromHandle(bmp.GetHicon());
            }
            else
            {
                // Если по какой-то причине ресурс не найден
                return SystemIcons.Application;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                trayIcon?.Dispose();
                updateTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}