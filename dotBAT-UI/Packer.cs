using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotBAT_UI
{
    class Packer
    {
        const string startcontainer = @"using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace dotBAT
{
    class container
    {
        [DllImport(""msvcrt.dll"", CallingConvention = CallingConvention.Cdecl, SetLastError=true)]
        public static extern int system(string command);

        static void Main(string[] args)
        {
";
        const string endcontainer = @"string code = string.Empty;
            for (int i = 0; i < batchdata.Length; i += 2)
            {
                string hs = string.Empty;
                hs = batchdata.Substring(i, 2);
                uint decval = Convert.ToUInt32(hs, 16);
                char character = Convert.ToChar(decval);
                code += character;
            }

            if (!Directory.Exists(""C:\\temp""))
            {
                Directory.CreateDirectory(""C:\\temp"");
            }

            using (StreamWriter sw = File.CreateText(""C:\\temp\\unpacked.bat""))    
            {
                sw.WriteLine(""@echo off"");
                sw.WriteLine(""cd "" + Environment.CurrentDirectory);
                sw.WriteLine(code);     
            }

            system(""C:\\temp\\unpacked.bat "" + string.Join("" "", args));
            File.Delete(""C:\\temp\\unpacked.bat"");
        }
    }
}";

        public static void Pack(string code, string path, bool icon)
        {
            string compileroptions = string.Empty;
            if (icon)
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Title = "Browse Icons",
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Filter = "ICO Files (*.ico)|*.ico",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    ReadOnlyChecked = true,
                };

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    compileroptions += "\"/win32icon:" + Path.GetFullPath(ofd.FileName) + "\" ";
                }
            }

            byte[] bytes = Encoding.Default.GetBytes(code);
            string batchdata = BitConverter.ToString(bytes).Replace("-", "");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(startcontainer);
            sb.AppendLine("string batchdata = \"" + batchdata + "\";");
            sb.AppendLine(endcontainer);

            CodeDomProvider csc = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters compars = new CompilerParameters();
            compars.GenerateExecutable = true;
            compars.OutputAssembly = path;
            compars.TreatWarningsAsErrors = false;
            compars.ReferencedAssemblies.Add("System.dll");
            compars.CompilerOptions = compileroptions;

            csc.CompileAssemblyFromSource(compars, sb.ToString());
        }
    }
}
