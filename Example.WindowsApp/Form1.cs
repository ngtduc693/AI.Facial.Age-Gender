using AI.Facial.AgeAndGender;
using Emgu.CV.Face;
using System.Diagnostics;

namespace Example.WindowsApp
{
    public partial class frmMain : Form
    {
        private readonly AgeAnalyzer ageRecognizer;
        private readonly GenderAnalyzer genderRecognizer;
        public frmMain()
        {
            InitializeComponent();
            ageRecognizer = new AgeAnalyzer();
            genderRecognizer = new GenderAnalyzer();
        }

        private async void btnChooseFile_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    pbSelectedImage.Image = System.Drawing.Image.FromFile(filePath);

                    try
                    {
                        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        stopwatch.Start();

                        var resultGender = await genderRecognizer.AnalyzeGenderFromStreamAsync(stream);
                        stream.Position = 0;
                        var resultAge = await ageRecognizer.AnalyzeAgeFromStreamAsync(stream);
                        stopwatch.Stop();
                        lblResult.Text = $"Age: {resultAge}\n\rGender: {resultGender}\n\rin {stopwatch.ElapsedMilliseconds / 1000.0} s";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
