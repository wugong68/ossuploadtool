using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ossuploadtool
{
    public partial class upload : Form
    {
        string keys = "xxxxxxxxxxwwwwww";
        public upload()
        {
            InitializeComponent();

            AccessKeyId = System.Configuration.ConfigurationManager.AppSettings["keyid"];
            AccessKeySecret = System.Configuration.ConfigurationManager.AppSettings["keyscert"];
            Endpoint = System.Configuration.ConfigurationManager.AppSettings["endpoint"];
            Buket = System.Configuration.ConfigurationManager.AppSettings["bucket"];

            //var jm = Convert.ToBase64String(AESEncryption.AESEncrypt(AccessKeySecret,keys));
            AccessKeySecret =  AESEncryption.AESDecrypt2str(AccessKeySecret,keys);
        }

        public static string AccessKeyId = "";

        public static string AccessKeySecret = "";

        public static string Endpoint = "";

        public static string Buket = "";

        private void upload_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;

            this.label3.Text = $"https://{Buket}.{Endpoint}/";

            this.labeljd.Text = "";
        }

        List<string> listimges = new List<string>();

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            listimges.Clear();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var list = (System.Array)e.Data.GetData(DataFormats.FileDrop);

                foreach (string item in list)
                {
                    this.textBox1.Text += item+"\r\n";
                    listimges.Add(item);
                }
            }

        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var pre = this.textBox2.Text;

            pre = pre.TrimEnd('/').TrimStart('/');

            if (string.IsNullOrEmpty(pre))
            {
                MessageBox.Show("必须设置一个oss存储目录名字");
                return;
            }

            this.button1.Enabled = false;

            OssClient client = new OssClient(Endpoint, AccessKeyId, AccessKeySecret);



            var sucesslist =new List<string>();

            int i = 1;
            foreach (var item in listimges)
            {
                //osspath 格式：”images/aaa.jpg“
                string osspath = pre+"/"+ System.IO.Path.GetFileName(item);
                try
                {
                    var result = client.PutObject(Buket, osspath, item);

                    if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        sucesslist.Add("https://" + Buket + "." + Endpoint + "/" + osspath);

                        labeljd.Text = $"完成 {i}/{listimges.Count} ";

                        //}));
                        Application.DoEvents();
                        //return osspath;

                        i++;
                    }
                }
                catch (Aliyun.OSS.Common.OssException ex)
                {
                    Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed with error info: {0}", ex.Message);
                }
            }

            this.button1.Enabled = true;

            var upslistform = new uploadeds();

            var tstring = "";
            foreach (var item in sucesslist)
            {
                tstring += item +"\r\n";
            }

            upslistform.InputText = tstring;

            upslistform.Show();
        }
    }
}
