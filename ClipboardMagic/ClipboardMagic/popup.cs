using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Runtime.InteropServices;

namespace ClipboardMagic
{

    public partial class popup : Form
    {
        
        public static int deskHeight = Screen.PrimaryScreen.Bounds.Height - 35;
        public static int deskWidth = Screen.PrimaryScreen.Bounds.Width;

        //Setting up the mouse drag functions
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public int MaxClips = 3;
        public int ClipPosY = 25;
        
        public clipsclass _clipboard;

        private Form1 _mainForm;

        public popup(clipsclass clipsBoard, Form1 frm)
        {
            _mainForm = frm;
            InitializeComponent();
            this.KeyPreview = true;
            _clipboard = clipsBoard;            
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
        }

        private void popup_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            //MessageBox.Show("test");
            
        }

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Hand;
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                this.Cursor = Cursors.Arrow;

            }
        }

        private void popup_Load(object sender, EventArgs e)
        {

            int i = 1;
            Dictionary<string, string> _clipDict = _clipboard.returnClips();
            clipz.AutoSize = true;
            this.Text = "popper";

            //Create the header area and add the close button and movement button
            PictureBox Closer = new PictureBox();
            Closer.Image = ClipboardMagic.Properties.Resources.button_cancel;
            Closer.Width = 20;
            Closer.Height = 20;
            Closer.Top = 3;
            Closer.MouseClick += new MouseEventHandler(closer);
            this.clipz.Controls.Add(Closer);

            PictureBox Mover = new PictureBox();
            Mover.Image = ClipboardMagic.Properties.Resources.cursor_move;
            Mover.Width = 20;
            Mover.Height = 20;
            Mover.Top = Closer.Top;
            Mover.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.clipz.Controls.Add(Mover);

            foreach (KeyValuePair<string, string> pair in _clipDict)
            {
                //MessageBox.Show("looping");

                //Add the clip seperator
                if (i != 1)
                {
                    PictureBox border = new PictureBox();
                    border.Image = ClipboardMagic.Properties.Resources.border;
                    border.Width = 390;
                    border.Height = 2;
                    border.Top = ClipPosY;
                    border.Left = 9;
                    ClipPosY += 5;
                    this.clipz.Controls.Add(border); 
                }

                //Create the Panel to hold the clip
                Panel clip = new Panel();
                clip.Name = "clip" + i.ToString();
                clip.Left = 3;
                clip.Top = ClipPosY;
                clip.Width = 400;
                //clip.MaximumSize = new Size(390, 100);
                //clip.MinimumSize = new Size(390, 40);
                //clip.BorderStyle = BorderStyle.FixedSingle;
                this.clipz.Controls.Add(clip);

                //Add the clipboard image
                PictureBox ClipboardImage = new PictureBox();
                ClipboardImage.Name = "ClipboardImage" + i.ToString();
                ClipboardImage.Image = ClipboardMagic.Properties.Resources.clipboard1;
                ClipboardImage.Width = 20;
                ClipboardImage.Height = 30;
                ClipboardImage.Left = 3;
                clip.Controls.Add(ClipboardImage);


                //Create the number
                Label ClipboardNumber = new Label();
                ClipboardNumber.Name = "ClipboardNumber" + i.ToString();
                ClipboardNumber.Text = i.ToString();
                ClipboardNumber.Left = 6;
                ClipboardNumber.Width = 14;
                ClipboardNumber.Height = 14;
                ClipboardNumber.BackColor = Color.White;
                ClipboardNumber.Font = new Font("Arial", 10, FontStyle.Bold);
                clip.Controls.Add(ClipboardNumber);
                ClipboardImage.SendToBack();


                //Create the text box
                RichTextBox ClipboardText = new RichTextBox();
                ClipboardText.Name = "ClipboardText" + i.ToString();
                ClipboardText.Top = 5;
                ClipboardText.Left = 30;
                ClipboardText.Text = _clipDict["Clipboard" + i.ToString()];                
                Size strSize = TextRenderer.MeasureText(ClipboardText.Text, ClipboardText.Font, new Size(350, 100), TextFormatFlags.WordBreak); 
                ClipboardText.Height = ClipboardText.Height = strSize.Height;
                ClipboardText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.popup_KeyDown);
                ClipboardText.ReadOnly = true;
                ClipboardText.BackColor = SystemColors.Menu;
                ClipboardText.BorderStyle = BorderStyle.None;
                ClipboardText.ScrollBars = 0;
                ClipboardText.MinimumSize = new Size(350, 30);
                ClipboardText.MaximumSize = new Size(350, 100);
                clip.Controls.Add(ClipboardText);

                //Create the Checkbox
                CheckBox ClipboardCheckbox = new CheckBox();
                ClipboardCheckbox.Name = "ClipboardCheckbox" + i.ToString();
                ClipboardCheckbox.Tag = ClipboardCheckbox.Name;
                ClipboardCheckbox.Left = 380;
                ClipboardCheckbox.Visible = false;
                clip.Controls.Add(ClipboardCheckbox);


                //Add then lock image
                PictureBox Locker = new PictureBox();
                Locker.Name = i.ToString();
                Locker.Left = ClipboardCheckbox.Left;
                Locker.Height = ClipboardCheckbox.Height;
                Locker.Width = 20;
                Locker.Height = 20;
                Locker.MouseClick += new MouseEventHandler(Locker_Click);
                if(ClipboardCheckbox.Checked == true)
                {
                    Locker.Image = ClipboardMagic.Properties.Resources.lock_closed;
                }
                else
                {
                    Locker.Image = ClipboardMagic.Properties.Resources.lock_open;
                }

                clip.Controls.Add(Locker);

                //Adjust the height of each clip and the position of the objects 
                clip.Height = ClipboardText.Height + 10;
                ClipboardImage.Top = clip.Height / 2 - ClipboardImage.Height / 2;
                ClipboardNumber.Top = clip.Height / 2 - ClipboardNumber.Height / 2;
                ClipboardCheckbox.Top = clip.Height / 2 - ClipboardCheckbox.Height / 2;
                Closer.Left = clip.Width - 17;
                Mover.Left = Closer.Left - 25;

                //ajust the position of the next clip
                ClipPosY = ClipPosY + clip.Height + 5;

                //MessageBox.Show(_clipDict["Clipboard" + i.ToString()]);
                i++;

            }


            // Add the logo and shawnpick.com address

            //Add the border
            PictureBox Logoborder = new PictureBox();
            Logoborder.Image = ClipboardMagic.Properties.Resources.border;
            Logoborder.Width = 390;
            Logoborder.Height = 2;
            Logoborder.Top = ClipPosY;
            Logoborder.Left = 9;
            ClipPosY += 5;
            this.clipz.Controls.Add(Logoborder); 

            //Add the logo
            PictureBox Logo = new PictureBox();
            Logo.Image = ClipboardMagic.Properties.Resources.Clipboard_Logo_Small;
            Logo.Width = 20;
            Logo.Height = 20;
            Logo.Top = ClipPosY + 5;
            Logo.Left = 5;
            clipz.Controls.Add(Logo);

            //Add the label
            Label LogoText = new Label();
            LogoText.Text = "Clipboard Magic";
            LogoText.Font = new Font("Arial", 10, FontStyle.Regular);
            LogoText.Left = Logo.Left + Logo.Width + 5;
            LogoText.Top = ClipPosY + 5;
            LogoText.Width = 130;
            //LogoText.BackColor = Color.Tomato;
            clipz.Controls.Add(LogoText);

            //Add the website address
            LinkLabel website = new LinkLabel();
            website.Text = "shawnpick.com";
            website.Font = new Font("Arial", 10, FontStyle.Bold);
            website.LinkColor = Color.Black;
            website.Width = 120;
            website.Left = clipz.Width - website.Width;
            website.Top = ClipPosY + 5;
            website.MouseClick += new MouseEventHandler(webHop);
            clipz.Controls.Add(website);



            this.Height = clipz.Height;
            this.Width = clipz.Width;
            clipz.BorderStyle = BorderStyle.FixedSingle;
            clipz.Left = 0;
            clipz.Top = 0;


        }

        public void closer(object sender, EventArgs e)
        {
            _mainForm.resetPaste();
            this.Close();
        }

        public void webHop(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.shawnpick.com");
        }

        public void Locker_Click(object sender, EventArgs e)
        {
            RichTextBox TB = new RichTextBox();
            PictureBox CN = new PictureBox();
            PictureBox b = (PictureBox)sender; 
            string name = b.Name;


            foreach (Control c in this.clipz.Controls)
            {
                //MessageBox.Show(c.Name);
                if (c.Name == "clip" + name)
                {

                    foreach (Control i in c.Controls)
                    {
                        if (i.Name == "ClipboardText" + name)
                        {
                            TB = (RichTextBox)i;
                        }
                    }

                    foreach (Control i in c.Controls)
                    {
                        if (i.Name == "ClipboardImage" + name)
                        {
                            CN = (PictureBox)i;
                        }
                    }

                    foreach (Control i in c.Controls)
                    {
                        //MessageBox.Show(i.Name);
                        if (i.Name == "ClipboardCheckbox" + name)
                        {
                            CheckBox cb = (CheckBox)i;
                            if(cb.Checked == false)
                            {
                                cb.Checked = true;
                                b.Image = ClipboardMagic.Properties.Resources.lock_closed;
                                TB.Enabled = false;
                                //CN.Image = ClipboardMagic.Properties.Resources.clipboard_bw;
                            }
                            else
                            {
                                cb.Checked = false;
                                b.Image = ClipboardMagic.Properties.Resources.lock_open;
                                TB.Enabled = true;
                                //CN.Image = ClipboardMagic.Properties.Resources.clipboard;
                            }
                                
                        }
                    }
                }
            }
        
            //MessageBox.Show(name);
        }


    }
}
