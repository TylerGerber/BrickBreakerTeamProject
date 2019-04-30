using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BrickBreaker
{
    public partial class HighScreen : UserControl
    {
        public HighScreen()
        {
            InitializeComponent();
        }

		int score1, score2, score3, score4, score5, score6, score7, score8, score9, score10;

		private void HighScreen_Load(object sender, EventArgs e)
		{
            allign();
            XmlReader reader = XmlReader.Create("Resources/HighScores.xml");

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Text)
				{
					score1 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score2 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score3 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score4 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score5 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score6 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score7 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score8 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score9 = Convert.ToInt16(reader.ReadString());
					reader.ReadToNextSibling("score");
					score10 = Convert.ToInt16(reader.ReadString());
				}
			}
			reader.Close();
			highScores.Text = "1. " + score1 + "\n" + "\n" +
				  "2. " + score2 + "\n" + "\n" +
				   "3. " + score3 + "\n" + "\n" +
					"4. " + score4 + "\n" + "\n" +
					 "5. " + score5 + "\n" + "\n" +
					  "6. " + score6 + "\n" + "\n" +
					   "7. " + score7 + "\n" + "\n" +
						"8. " + score8 + "\n" + "\n" +
						 "9. " + score9 + "\n" + "\n" +
						  "10. " + score10;
		}

		private void label1_Click(object sender, EventArgs e)
        {
            Form form = this.FindForm();
            MenuScreen ms = new MenuScreen();

            ms.Location = new Point((form.Width - ms.Width) / 2, (form.Height - ms.Height) / 2);

            form.Controls.Add(ms);
            form.Controls.Remove(this);
        }

        private void allign()
        {
            label1.Location = new Point((this.Width - label1.Width) / 2, (this.Height - label1.Height) / 2);
        }

    }
}
