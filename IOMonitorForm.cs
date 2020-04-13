using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ICT
{
    public partial class IOMonitorForm : Form
    {
        private Thread MonitorThread = null;
        public IOMonitorForm()
        {
            InitializeComponent();
        }
        private void IOMonitorForm_Load(object sender, EventArgs e)
        {
            if (MonitorThread == null)
            {
                MonitorThread = new Thread(new ThreadStart(IOMonitor));
                MonitorThread.Start();
            }
        }
        private void tabControlMonitor_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = new Font("宋体", 15);
            Brush brBack = new SolidBrush(Color.BlueViolet);
            Brush brBack1 = new SolidBrush(Color.Blue);
            SolidBrush brush = new SolidBrush(Color.White);
            Rectangle tabpageRect = (Rectangle)tabControlMonitor.GetTabRect(e.Index);

            StringFormat sf = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            e.Graphics.FillRectangle(brBack, tabpageRect); //用指定的颜色填充选项卡矩形区域

            tabpageRect = (Rectangle)tabControlMonitor.GetTabRect(tabControlMonitor.SelectedIndex);
            e.Graphics.FillRectangle(brBack1, tabpageRect);

            g.DrawString(tabControlMonitor.Controls[e.Index].Text, font, brush, e.Bounds, sf);
        }
        private string Reverse(string original)
        {
            char[] arr = original.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        } 
        private bool QueryIOPort()
        {
            int n = IOC0640.ioc_read_outport(0, 0);
            string Str = Convert.ToString(n, 2).PadLeft(32, '0');
            Str = Reverse(Str);
            for (int j = 0; j < 32; j++)
            {
                string str1 = "Lable" + "DO" + (j+1).ToString();
                Label lable = (Label)this.Controls.Find(str1, true)[0];
                if (Str.Substring(j, 1) == "1")
                {
                    lable.BackColor = Color.Red;
                }
                else
                {
                    lable.BackColor = Color.LimeGreen;
                }
            }
            int m = IOC0640.ioc_read_inport(0, 0);
            string Str2 = Convert.ToString(m, 2).PadLeft(32, '0');
            Str2 = Reverse(Str2);
            for (int i = 0; i < 32; i++)
            {
                string str3 = "Lable" + "DI" + (i + 1).ToString();
                Label lable = (Label)this.Controls.Find(str3, true)[0];
                if (Str2.Substring(i, 1) == "1")
                {
                    lable.BackColor = Color.Red;
                }
                else
                {
                    lable.BackColor = Color.LimeGreen;
                }
            }
            return true;
        }
        private void IOMonitor()
        {
            while (true)
            {
                QueryIOPort();
                Thread.Sleep(10);
            }
        }

        
    }
}
