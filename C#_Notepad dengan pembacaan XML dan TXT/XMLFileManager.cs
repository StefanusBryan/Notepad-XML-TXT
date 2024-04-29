using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TEXTEDITOR
{
    public class XMLFileManager
    {
        public string filepath { get; set; }

        public void WriteXMLFile(string fontfamily, int fontsize, string fonttype, Color fontcolor, Color bgcolor, string content)
        {
            // Menggunakan using statement agar objek IDisposable seperti XmlWriter akan secara otomatis di-dispose setelah digunakan
            using (XmlWriter writer = XmlWriter.Create(filepath, new XmlWriterSettings { Indent = true }))
            {
                // Mulai menulis dokumen XML
                writer.WriteStartDocument();
                writer.WriteStartElement("editor");

                // Menulis informasi font
                writer.WriteStartElement("font");
                writer.WriteElementString("family", fontfamily);
                writer.WriteElementString("size", fontsize.ToString());
                writer.WriteElementString("type", fonttype);
                writer.WriteStartElement("color");
                writer.WriteElementString("r", fontcolor.R.ToString());
                writer.WriteElementString("g", fontcolor.G.ToString());
                writer.WriteElementString("b", fontcolor.B.ToString());
                writer.WriteEndElement(); // Penutup color
                writer.WriteEndElement(); // Penutup font

                // Menulis informasi warna background
                writer.WriteStartElement("bg-color");
                writer.WriteElementString("r", bgcolor.R.ToString());
                writer.WriteElementString("g", bgcolor.G.ToString());
                writer.WriteElementString("b", bgcolor.B.ToString());
                writer.WriteEndElement(); // Penutup bg-color

                // Menulis konten teks
                writer.WriteElementString("content", content);

                writer.WriteEndElement(); // Penutup editor
                writer.WriteEndDocument(); // Menutup dokumen XML
            }
        }

        public void ReadXMLFile(RichTextBox richTextBox)
        {
            XmlTextReader reader = new XmlTextReader(filepath);

            string fontfamily = "";
            float fontsize = 0;
            FontStyle fontStyle = FontStyle.Regular;
            string content = "";
            Color fontColor = Color.Black;
            Color backColor = Color.White;

            bool readBack = false;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "family":
                            fontfamily = reader.ReadString().Trim();
                            break;
                        case "size":
                            fontsize = float.Parse(reader.ReadString().Trim());
                            break;
                        case "type":
                            // Ubah string gaya font menjadi nilai FontStyle
                            fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), reader.ReadString().Trim());
                            break;
                        case "content":
                            content = reader.ReadString().Trim();
                            break;
                        case "color":
                            readBack = true;
                            break;
                        case "bg-color":
                            readBack = false;
                            break;
                        case "r":
                            if (readBack)
                            {
                                fontColor = Color.FromArgb(int.Parse(reader.ReadString().Trim()), fontColor.G, fontColor.B);
                            }
                            else
                            {
                                backColor = Color.FromArgb(int.Parse(reader.ReadString().Trim()), backColor.G, backColor.B);
                            }
                            break;
                        case "g":
                            if (readBack)
                            {
                                fontColor = Color.FromArgb(fontColor.R, int.Parse(reader.ReadString().Trim()), fontColor.B);
                            }
                            else
                            {
                                backColor = Color.FromArgb(backColor.R, int.Parse(reader.ReadString().Trim()), backColor.B);
                            }
                            break;
                        case "b":
                            if (readBack)
                            {
                                fontColor = Color.FromArgb(fontColor.R, fontColor.G, int.Parse(reader.ReadString().Trim()));
                            }
                            else
                            {
                                backColor = Color.FromArgb(backColor.R, backColor.G, int.Parse(reader.ReadString().Trim()));
                            }
                            break;
                    }
                }
            }
            reader.Close();

            // Set RichTextBox dengan pengaturan font yang sesuai
            richTextBox.Font = new Font(fontfamily, fontsize, fontStyle);
            richTextBox.ForeColor = fontColor;
            richTextBox.BackColor = backColor;
            richTextBox.Text = content;
        }
    }
}