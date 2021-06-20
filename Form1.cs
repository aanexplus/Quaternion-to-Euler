using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Quat2Euler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static double Pitch(float x, float y, float z, float w)
        {
            double value1 = 2.0 * (w * x + y * z);
            double value2 = 1.0 - 2.0 * (x * x + y * y);
            double roll = Math.Atan2(value1, value2);
            return roll * (180.0 / Math.PI);
        }

        public static double Yaw(float x, float y, float z, float w)
        {
            double value = +2.0 * (w * y - z * x);
            value = value > 1.0 ? 1.0 : value;
            value = value < -1.0 ? -1.0 : value;
            double pitch = Math.Asin(value);
            return pitch * (180.0 / Math.PI);
        }

        public static double Roll(float x, float y, float z, float w)
        {
            double value1 = 2.0 * (w * z + x * y);
            double value2 = 1.0 - 2.0 * (y * y + z * z);
            double yaw = Math.Atan2(value1, value2);
            return yaw * (180.0 / Math.PI);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the Participant Folder";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string selectedpath = fbd.SelectedPath;
                DirectoryInfo info = new DirectoryInfo(selectedpath);
                FileInfo[] files = info.GetFiles("*.txt");
                string foldername = selectedpath + "\\Euler Rotation";
                System.IO.Directory.CreateDirectory(foldername);

                foreach (FileInfo file in files)
                {
                    string[] contents = File.ReadAllLines(selectedpath+"\\"+file.Name);
                    StreamWriter writer = new StreamWriter(foldername + "\\" + file.Name);
                    for (int i = 0; i < contents.Length - 1; i++)
                    {
                        string[] content = contents[i].Split();

                        for (int j = 0; j < content.Length - 4; j += 4)
                        {
                            float x = float.Parse(content[j]);
                            float y = float.Parse(content[j + 1]);
                            float z = float.Parse(content[j + 2]);
                            float w = float.Parse(content[j + 3]);

                            double rotX = Pitch(x, y, z, w);
                            double roty = Yaw(x, y, z, w);
                            double rotz = Roll(x, y, z, w);
                            writer.Write(rotz + "\t" + rotX + "\t"+ roty + "\t");
                        }
                        writer.WriteLine();
                    }
                }
                MessageBox.Show("Processing Completed");
            }

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
