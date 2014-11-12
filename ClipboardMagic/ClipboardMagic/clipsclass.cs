using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ClipboardMagic
{
    public class clipsclass
    {
        int MaxClips = Properties.Settings.Default.NumClips;
        Dictionary<string, string> Clipz = new Dictionary<string, string>();


        public clipsclass()
        {
            //Array Clipz;
            string _name = "";

            for (int i = 1; i < MaxClips + 1; i++) 
            {
                _name = "Clipboard" + i.ToString();
                Clipz.Add(_name, "");
                //Clipz.Add(_name, "testing dict");
            }
        }

        public string printClip(string ClipNum)
        {
            //MessageBox.Show("Getting the clip: " + Clipz["Clipboard" + ClipNum]);
            return Clipz["Clipboard" + ClipNum];
        }

        public int addClip(string text)
        {
            string oldText;
            string newText = text;
            //Clipz["Clipboard1"] = text;
            int i = 1;

            foreach (KeyValuePair<string, string> pair in Clipz.ToList())
            {
                oldText = pair.Value;
                Clipz["Clipboard" + i.ToString()] = newText;
                newText = oldText;
                i++;
            }
            oldText = "";
            newText = "";
            return 0;

        }

        public Dictionary<string, string> returnClips()
        {
            return Clipz;
        }


    }

}
