using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WindowsInput;


namespace ClipboardMagic
{


    public partial class Form1 : Form
    {

        //Setting the hook handle Var.
        private static int _hookHandle = 0;

        //Importing the DLL's

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetAsyncKeyState(int nVirtKey);

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Setting the Vars.
        public const int WH_KEYBOARD_LL = 13;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;

        //Setting the Vars for the folowing keys.
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        //Paste
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam); 
        private const uint WM_COMMAND = 0x0111;

        private static bool isPopupReady = true;

        public Form1()
        {
            
            InitializeComponent();
            SetHook();
            
            
        }

        private void SetHook()
        {
            // Set system-wide hook.
            _hookHandle = SetWindowsHookEx( WH_KEYBOARD_LL, KbHookProc, (IntPtr)0, 0);
        }

        private int KbHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {

            var hookStruct = (KbLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KbLLHookStruct));
            bool ctrlDown = GetKeyState(VK_LCONTROL) < 0 || GetKeyState(VK_RCONTROL) < 0;
            bool isPopperOpen = false;
            string Key = "";

            //Check if the clipzboard is open already
            if (wParam == (IntPtr)WM_KEYUP && Application.OpenForms.OfType<popup>().Count() > 0)
            {
                isPopperOpen = true;
                isPopupReady = false;
                Key = new KeysConverter().ConvertToString(hookStruct.vkCode);
            }

            
            //If the clipsboard is open and a user presses a key we handle that here.
            if (wParam == (IntPtr)WM_KEYUP &&  isPopperOpen == true)
            {
                //MessageBox.Show("See it : " + Key.ToString());

                //Clipboard.SetText(ClipsBoard.printClip(Key.ToString()));
                
                Clipboard.SetText(ClipsBoard.printClip("1"));
                //((popup)Application.OpenForms["popper1"]).Close();
                string id = "popper1";
                foreach (Form f in Application.OpenForms)
                    
                    if (Convert.ToString(id) == f.Name)
                    {
                        f.Close();
                        break;

                    }
                runPaste();

            }

            //Fires when the CTRL + C buttons are DOWN
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP && hookStruct.vkCode == 0x43 && ctrlDown)
            {

                //Clipboard.SetText("Are your base, are belone to us!");
                
                /*if (Clipboard.ContainsText(TextDataFormat.Rtf))
                {
                    MessageBox.Show("RTF!!");
                }*/


                CopyClip(Clipboard.GetText());
                Clipboard.SetText(" ");
                return -1;
            }


            //Fires when the the CTRL is down and the V is released
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN && hookStruct.vkCode == 0x56 && ctrlDown && isPopperOpen == false && isPopupReady == true)
            {
                //Starts the timmer that check for the release of the CTRL button
                //checkbtnup.Start();

                popup popper = new popup(ClipsBoard,this);
                popper.Name = "popper1";
                popper.Show();
                popper.BringToFront();

                return -1;
            }
            else
            {
                // Pass to other keyboard handlers. Makes the Ctrl+V pass through.
                return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
            }
        }

        public void runPaste()
        {
            try
            {
                SendKeys.SendWait("^{v}");
            }
            catch { }

            isPopupReady = true;
        }

        public void resetPaste()
        {
            isPopupReady = true;
        }

        private void CopyClip(string text)
        {

            if (text.ToString() != " ")
            {
                //MessageBox.Show("Copy: " + text);
                int complete = ClipsBoard.addClip(text);
                if (ClipsBoard.addClip(text) != 0)
                {
                    MessageBox.Show("There was an error");
                }
            }
            else { MessageBox.Show("It was blank"); } 
        }

        private static int KbHookProc2(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //MessageBox.Show(lParam.ToString());
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                var hookStruct = (KbLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KbLLHookStruct));

                // Quick and dirty check. You may need to check if this is correct. See GetKeyState for more info.
                bool ctrlDown = GetKeyState(VK_LCONTROL) != 0 || GetKeyState(VK_RCONTROL) != 0;


                if (ctrlDown && hookStruct.vkCode == 0x56 && hookStruct.KF_REPEAT == 0) // Ctrl+V
                {
                    Clipboard.SetText(" "); 
                    //MessageBox.Show(" Paste ?");


                    //Load a new instance of the popup form.
                    //popup popper = new popup();
                    //popper.Show();
                    //popper.BringToFront();


                    InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_O);


                    //SendKeys.SendWait("{HELLO!}");

                    //droptext();

                }

            }

            // Pass to other keyboard handlers. Makes the Ctrl+V pass through.
            return CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnhookWindowsHookEx(_hookHandle);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Clipboard.SetText(" ");
            this.Hide();
        }

        private void checkbtnup_Tick(object sender, EventArgs e)
        {
            bool ctrlDown = GetKeyState(VK_LCONTROL) < 0 || GetKeyState(VK_RCONTROL) < 0;

            if (!ctrlDown)
            {

            //Keeping theses here incase i need to knwo how to firs off a single letter to teh system, usefull for activating system/hot keys.
            //InputSimulator.SimulateKeyDown(VirtualKeyCode.SHIFT);
            //InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_H);

                InputSimulator.SimulateTextEntry("This is a line of text that I paste automatically from within the C# program.");
            checkbtnup.Stop();
            }
        }


        //Declare the wrapper managed MouseHookStruct class.
        [StructLayout(LayoutKind.Sequential)]
        public class KbLLHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int KF_REPEAT;
            public int WM_KEYDOWN;
            public int WM_KEYUP;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        clipsclass ClipsBoard = new clipsclass();
    }
}
