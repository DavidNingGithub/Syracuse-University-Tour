using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Web;
namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient client = new HttpClient();
        private HttpRequestMessage message;
        private HttpResponseMessage response = new HttpResponseMessage();
        private string urlBase;
        public string status { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            urlBase = "http://localhost:53505/api/File";
            b1.IsEnabled = false;
            b2.IsEnabled = false;
            b3.IsEnabled = false;
            b4.IsEnabled = false;
            b5.IsEnabled = false;
        }
        string[] getAvailableFiles()
        {
            message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri(urlBase);
            Task<HttpResponseMessage> task = client.SendAsync(message);
            HttpResponseMessage response1 = task.Result;
            response = task.Result;
            status = response.ReasonPhrase;
            string[] files = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(response1.Content.ReadAsStringAsync().Result);
            return files;
        }
        //----< open file on server for reading >------------------------------

        int openServerFile(string fileName)//upload use
        {
            message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            urlBase = "http://localhost:53505/api/File";
            string urlActn = "?fileName=" + fileName + "&open=true";
            message.RequestUri = new Uri(urlBase + urlActn);
            Task<HttpResponseMessage> task = client.SendAsync(message);//send message
            //Thread.Sleep(100);
            HttpResponseMessage response = task.Result;
            status = response.ReasonPhrase;
            return (int)response.StatusCode;
        }
        void postFileBlock(byte[] Block,string FileName,string description,string siteName,string AddOrNot)//upload use
        {

            string urlActn = "?FileName=" + FileName + "&description=" + description + "&siteName=" + siteName + "&AddOrNot=" + AddOrNot;
            message = new HttpRequestMessage();
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri(urlBase + urlActn);
            message.Content = new ByteArrayContent(Block);
            Task<HttpResponseMessage> task = client.SendAsync(message);
        }
        void closeFile(FileStream down,string FileName)
        {
            message = new HttpRequestMessage();
            message.Method = HttpMethod.Get;
            string uri = "http://localhost:53505/api/File?fileName=" + FileName + "&open=false";
            message.RequestUri = new Uri(uri);
            Task<HttpResponseMessage> task = client.SendAsync(message);
            HttpResponseMessage response = task.Result;
            status = response.ReasonPhrase;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            fd.Title = "Open Files";
            fd.Multiselect = true;
            string str1 = System.IO.Directory.GetCurrentDirectory();
            int pos = str1.LastIndexOf("\\");
            str1 = str1.Substring(0, pos) + "\\Image";
            fd.InitialDirectory = str1;
            fd.Filter = "All Files (*.*)|*.*";//|Text Files (.txt)|*.txt|H Files (.h)|*.h|CPP Files (.cpp)|*.cpp
            if ((bool)fd.ShowDialog().GetValueOrDefault())
            {
                System.Collections.ArrayList fileList = new System.Collections.ArrayList();
                foreach (string file in fd.FileNames)
                {
                    fileList.Add(file);
                    FileBox.Items.Add(file);
                }
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (FileBox.Items.Count != 0)
            {
                FileBox.Items.Remove(FileBox.Items[FileBox.Items.Count - 1]);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            urlBase = @"http://localhost:53505/api/File";
            string path = FileBox.Items[0].ToString();
            FileStream down = new FileStream(path, FileMode.Open);
            int pos = path.LastIndexOf("\\");
            string siteName=SiteName.Text.ToString();
            string FileName = path.Substring(pos+1);
            string Des = Description.Text.ToString();
            string AddOrNot = Judge.Text.ToString();
            int a = openServerFile(FileName);
            const int blockSize = 512;
            while (true)
            {
                byte[] Block = new byte[blockSize];
                int bytesRead = down.Read(Block, 0, blockSize);
                if (bytesRead < blockSize)  // compress block
                {
                    byte[] returnBlock = new byte[bytesRead];
                    for (int i = 0; i < bytesRead; ++i)
                        returnBlock[i] = Block[i];
                    Block = returnBlock;
                }              
                postFileBlock(Block,FileName,Des,siteName,AddOrNot);
                Thread.Sleep(100);//important
                if (Block.Length < blockSize)
                    break;
            }
            closeFile(down,FileName);
            down.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ServerFile.Items.Clear();
            string[] files = getAvailableFiles();
             foreach (string file in files)
             {
                 ServerFile.Items.Add(file);
             }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string filePath = ServerFile.SelectedItem.ToString();
            string judge = SimplyDeleteOrNot.Text.ToString();
            message = new HttpRequestMessage();
            message.Method = HttpMethod.Delete;
            urlBase = "http://localhost:53505/api/File";
            string urlActn = "?filePath=" + filePath + "&SimplyAddOrNot="+judge;
            message.RequestUri = new Uri(urlBase + urlActn);
            Task<HttpResponseMessage> task = client.SendAsync(message);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string UserName = Username.Text.ToString();
            string PassWord = password.Password;
            message = new HttpRequestMessage();
            message.Method = HttpMethod.Put;
            urlBase = "http://localhost:53505/api/File";
            string urlActn = "?UserName=" + UserName + "&Password="+PassWord;
            message.RequestUri = new Uri(urlBase + urlActn);
            Task<HttpResponseMessage> task = client.SendAsync(message);
            HttpResponseMessage response1 = task.Result;
            response = task.Result;
            status = response.ReasonPhrase;
            string check = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(response1.Content.ReadAsStringAsync().Result);
            if(check=="true")
            {
                b1.IsEnabled = true;
                b2.IsEnabled = true;
                b3.IsEnabled = true;
                b4.IsEnabled = true;
                b5.IsEnabled = true;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            b1.IsEnabled = false;
            b2.IsEnabled = false;
            b3.IsEnabled = false;
            b4.IsEnabled = false;
            b5.IsEnabled = false;
        }
    }
}
