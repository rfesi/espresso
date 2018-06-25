﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Espresso {

    /// <summary>
    ///     Tray icon that displays in the taskbar
    /// </summary>
    public class TrayView {

        // Hidden window for running code on GUI thread
        private System.Windows.Window _hiddenWindow;

        private NotifyIcon _notifyIcon; // The tray display
        private IContainer _components; // For grouping components

        // FORMS

        // TOOLSTRIP MENU ITEMS
        private ToolStripMenuItem _testItem;
        private ToolStripMenuItem _preventSleepItem;
        private ToolStripMenuItem _allowSleepItem;
        private ToolStripMenuItem _exitItem;

        public TrayView() {

            _components = new Container();
            _notifyIcon = new NotifyIcon(_components) {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Espresso.Properties.Resources.NotReadyIcon,
                Text = "Espresso (not running)",
                Visible = true,
            };

            // Hook events
            _notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            _hiddenWindow = new System.Windows.Window();
            _hiddenWindow.Hide();
        }

        System.Windows.Media.ImageSource AppIcon {
            get {
                System.Drawing.Icon icon = Properties.Resources.ReadyIcon;
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle,
                    System.Windows.Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
        }

        /// <summary>
        ///     Construct toolstrip menu item with handler
        /// </summary>
        /// <param name="displayText">
        ///     Tex to display
        /// </param>
        /// <param name="tooltipText">
        ///     Hover tooltip text
        /// </param>
        /// <param name="eventHandler">
        ///     Event callback
        /// </param>
        /// <returns>
        ///     Constructed toolstrip menu item
        /// </returns>
        private ToolStripMenuItem buildMenuItem(String displayText, String tooltipText, EventHandler eventHandler) {
            ToolStripMenuItem item = new ToolStripMenuItem(displayText);
            if (eventHandler != null) {
                item.Click += eventHandler;
            }

            item.ToolTipText = tooltipText;
            return item;
        }

        /// <summary>
        ///     Populate context menu on each open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuStrip_Opening(Object sender, CancelEventArgs e) {
            e.Cancel = false;

            // Populate list if empty
            if (_notifyIcon.ContextMenuStrip.Items.Count == 0) {
                _exitItem = buildMenuItem("&Exit", "Exits System Tray App", exitItem_Click);
                _notifyIcon.ContextMenuStrip.Items.Add(_exitItem);
            }
        }

        private void exitItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void displayStatusMessage(String text) {
            _hiddenWindow.Dispatcher.Invoke(delegate {
                _notifyIcon.BalloonTipText = "Test";
                // The timeout is ignored on recent Windows
                _notifyIcon.ShowBalloonTip(3000);
            });
        }
    }

}
