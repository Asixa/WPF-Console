using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using System.Collections.Generic;
namespace ADK.Console
{
    public partial class Console : UserControl
    {
        Process process;
        ProcessStartInfo psi;
        
#region Core
        public Console()
        {
            InitializeComponent();
            psi = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                FileName = "cmd.exe",
                UseShellExecute = false
            };
            process = Process.Start(psi);
            process.OutputDataReceived += OutputDataReceived;
            process.ErrorDataReceived += ErrorDataReceived;
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        void ConsoleWrite(string s)
        {
            if (s.ToLower() == "cls")
            {
                Output.Text = "";
                return;
            }
            process.StandardInput.WriteLine(s);
        }

        void ConsoleRead(string s)
        {
            Output.Text = Output.Text + s + Environment.NewLine;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (Input.Text == "") return;
                ConsoleWrite(Input.Text);
                Input.Text = "";
             
            }
        }
        List<string> result = new List<string>();

        void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            
                Output.Dispatcher.BeginInvoke(new Action(() =>
                {   
                    string newline = e.Data + Environment.NewLine;
                    Output.Text = Output.Text.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine) + newline;
                    Output.ScrollToEnd();
                    result.Add(newline);
                    
                }));
                
            
        }

        void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output.Dispatcher.BeginInvoke(new Action(() =>
            {
                string newline = e.Data + Environment.NewLine;
                Output.Text = Output.Text.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine) + newline;
                Output.ScrollToEnd();
                result.Add(newline);
            }));
          
        }
#endregion

        public List<string> GetNewestResult()
        {
            List<string> _return = result;
            _return.Remove(_return[0]);
            return _return;
        }
        public void AddCommand(string cmd)
        {
            ConsoleWrite(cmd);
        }

   
    }
}
