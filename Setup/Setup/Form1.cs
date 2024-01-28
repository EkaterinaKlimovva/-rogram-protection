using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace Setup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pathSetupSetup;

            try
            {
                pathSetupSetup = textBox1.Text;
                CreateFiles(pathSetupSetup);
                MessageBox.Show("�������� �����!");
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ����������:" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //���������� ��� ��������� ��������� ������
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int GetVolumeInformation(string strPathName,
          StringBuilder strVolumeNameBuffer,
          int lngVolumeNameSize,
          out int lngVolumeSerialNumber,
          out int lngMaximumComponentLength,
          out int lngFileSystemFlags,
          StringBuilder strFileSystemNameBuffer,
          int lngFileSystemNameSize);

        //������� �����
        public static void CreateFiles(string pathSetup)
        {
            string file = pathSetup + "\\Program.exe";
            var assembly = Assembly.GetExecutingAssembly();

            Directory.CreateDirectory(pathSetup);
            File.Copy(@"\\shoting_target\shoting_target\Resourse\shoting_target.exe", file);
            //������ ����� bin ��� ������
            Directory.CreateDirectory(pathSetup + "\\bin");
            Directory.CreateDirectory(pathSetup + "\\bin\\obj");
            int serialNum, maxNameLen, flags;
            string rootPath = pathSetup.Substring(0, pathSetup.IndexOf(":\\") + 2);
            StringBuilder a = new StringBuilder(), name = new StringBuilder();

            GetVolumeInformation(rootPath, name, 100, out serialNum, out maxNameLen, out flags, a, 100);

            string serial = serialNum.ToString();

            //���������� ��������� ������, ������ ������� ����� ���, ��� ������� ���� ��� ������, � ����� ��� ��������
             string newSerialNum = "";
             for (int i = 1; i < serial.Length; i += 2)
             {
                 newSerialNum += serial[i];
             }
             for (int i = 0; i < serial.Length; i += 2)
             {
                 newSerialNum += serial[i];
             }

            //������ ������ ����������
            string serl = "35453242423";
            int c;

            using (FileStream fstream = new FileStream($"{pathSetup}\\bin\\obj\\scrSrlFl.log", FileMode.OpenOrCreate))
            {
                // ����������� ����� ������������� �������� ����� � ����� � ������� � ����
                byte[] array = Encoding.Default.GetBytes(newSerialNum);
                fstream.Write(array, 0, 3);
            }

            using (FileStream fstream = new FileStream($"{pathSetup}\\bin\\dpScnd.ddl", FileMode.OpenOrCreate))
            {
                // ����������� ����� ������������� �������� ����� � ����� � ������� � ����
                byte[] array = Encoding.Default.GetBytes(newSerialNum);
                fstream.Write(array, 3, 3);
            }

            using (FileStream fstream = new FileStream($"{pathSetup}\\bin\\obj\\lgNmf.log", FileMode.OpenOrCreate))
            {
                // ����������� ����� ������������� �������� ����� � ����� � ������� � ����
                byte[] array = Encoding.Default.GetBytes(newSerialNum);
                fstream.Write(array, 6, 4);
            }

            //�������� �������� �����
            using (FileStream fstream = new FileStream($"{pathSetup}\\prGsn.log", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(newSerialNum);
                fstream.Write(array, 2, 3);
            }

            using (FileStream fstream = new FileStream($"{pathSetup}\\bin\\SrlNF.dll", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(newSerialNum);
                fstream.Write(array, 0, 3);
            }

        }

    }
}