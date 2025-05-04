using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CSV_Libary;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static AccountingEveryDay.Contract.AccountContract;
using AccountingEveryDay.Presenter;
using AccountingEveryDay.Utility;

namespace AccountingEveryDay.Forms
{
    public partial class 記一筆 : Form , IAccountView
    {
        IAccountPresenter accountPresenter = null;
        string dataOutputFolder = "C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay_data\\";
        string Date;

        public 記一筆()
        {
            InitializeComponent();
            accountPresenter = new AccountPresenter(this);
        }

        private void 記一筆_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = DataModel.Type;
            comboBox3.DataSource = DataModel.PaymentType;
            comboBox4.DataSource = DataModel.Target;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Image.FromFile("C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay\\assets\\upload-cloud.jpg");
            pictureBox2.Image = Image.FromFile("C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay\\assets\\upload-cloud.jpg");
            Date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            accountPresenter = new AccountPresenter(this);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Date = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string Type = comboBox1.Text;
            string Item = comboBox2.Text;
            int Amount = Convert.ToInt32(textBox2.Text);
            string Target = comboBox4.Text;
            string PaymentType = comboBox3.Text;
            string Note = textBox1.Text;
            string Img1Path = dataOutputFolder + Date + "\\" + Guid.NewGuid() + ".jpg";
            string Img2Path = dataOutputFolder + Date + "\\" + Guid.NewGuid() + ".jpg";
            string Img1MinPath = dataOutputFolder + Date + "\\" + Guid.NewGuid() + ".jpg";
            string Img2MinPath = dataOutputFolder + Date + "\\" + Guid.NewGuid() + ".jpg";

            ItemModel model = new ItemModel();
            model.Date = Date;
            model.Type = Type;
            model.Item = Item;
            model.Amount = Amount;
            model.Target = Target;
            model.PaymentType = PaymentType;
            model.Note = Note;
            model.Img1Path = Img1Path;
            model.Img2Path = Img2Path;
            model.Img1MinPath = Img1MinPath;
            model.Img2MinPath = Img2MinPath;

            accountPresenter.UpdateDates(model);


            //Image.FromFile(Utility.ConvertImg(Img1Path, 50, 50)).Save(Img1MinPath);

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = comboBox1.Text;
            comboBox2.DataSource = DataModel.ItemTypes[text];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader(dataOutputFolder);
            while(reader.Peek() >= 0)
            {
                Console.WriteLine(reader.ReadLine().ToString());
            }
            reader.Close();
        }

        private void SelectedImageClick(object sender, EventArgs e)
        {            
            OpenFileDialog openFileDialog = new OpenFileDialog();

            PictureBox pictureBox =  (PictureBox)sender;
            openFileDialog.InitialDirectory = @"C:\Users\USERA\Documents\C#\pic";
            openFileDialog.Filter = "圖片檔|*.jpg;*.png";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = Utility.Utility.ConvertImg(openFileDialog.FileName);
                //Utility.CompressAndSaveImage(Image.FromFile(openFileDialog.FileName), filePath, 30);
                ((PictureBox)sender).Image = bmp;
                //bmp.Save(filePath);
                //pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                //Utility.ConvertImg(openFileDialog.FileName, 50, 50).Save("C:\\Users\\USERA\\Downloads\\test\\test.jpg");
                //Image.FromFile(openFileDialog.FileName).Save("C:\\Users\\USERA\\Downloads\\test\\test.jpg");
            }
        }

        void IAccountView.DataResponse(ItemModel model)
        {
            //PictureBoxSaving(pictureBox1, model);
            //PictureBoxSaving(pictureBox2, model);
            pictureBox1.Image.Save(model.Img1Path);
            Console.WriteLine($"Img1Path: {model.Img1Path}");
            using (Image sourceImage = new Bitmap(pictureBox1.Image))
            {
                var resizeImg = Utility.Utility.ResizeAndCompressImage(sourceImage, 50, 50, 50L);
                resizeImg.Save(model.Img1MinPath);
                Console.WriteLine($"Img1MinPath: {model.Img1MinPath}");
            }
            pictureBox1.Image.Dispose();
            pictureBox1.Image = Image.FromFile(@"C:\Users\USERA\Documents\C#\AccountingEveryDay\assets\upload-cloud.jpg");

            pictureBox2.Image.Save(model.Img2Path);
            Console.WriteLine($"Img1Path: {model.Img2Path}");
            using (Image sourceImage = new Bitmap(pictureBox2.Image))
            {
                var resizeImg = Utility.Utility.ResizeAndCompressImage(sourceImage, 50, 50, 50L);
                resizeImg.Save(model.Img2MinPath);
                Console.WriteLine($"Img1MinPath: {model.Img2MinPath}");
            }
            pictureBox2.Image.Dispose();
            pictureBox2.Image = Image.FromFile(@"C:\Users\USERA\Documents\C#\AccountingEveryDay\assets\upload-cloud.jpg");


            GC.Collect();
        }

        private void PictureBoxSaving(PictureBox pictureBox, ItemModel model)
        {
            pictureBox.Image.Save(model.Img1Path);
            Console.WriteLine($"Img1Path: {model.Img1Path}");
            using (Image sourceImage = new Bitmap(pictureBox.Image))
            {
                var resizeImg = Utility.Utility.ResizeAndCompressImage(sourceImage, 50, 50, 50L);
                resizeImg.Save(model.Img1MinPath);
                Console.WriteLine($"Img1MinPath: {model.Img1MinPath}");
            }
            pictureBox.Image.Dispose();
            pictureBox.Image = Image.FromFile(@"C:\Users\USERA\Documents\C#\AccountingEveryDay\assets\upload-cloud.jpg");
        }
    }
}
