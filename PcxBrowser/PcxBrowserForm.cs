using PcxLibrary;

namespace PcxBrowser
{
    public partial class PcxBrowserForm : Form
    {
        public PcxBrowserForm()
        {
            InitializeComponent();

            var pcx = new PcxDecoder(File.OpenRead(Path.Combine("Resources", "marbles.pcx")));
            pcx.DecodingProgress += Pcx_DecodingProgress;

            pcx.ReadHeader();
            pcx.DecodeImageInBackgroundThread();
        }

        private void Pcx_DecodingProgress(object sender, DecodingProgressEventArgs args)
        {
            Invoke(() =>
            {
                Text = $"Loading - {args.Progress} %";

                if (args.Progress == 100)
                {
                    pictureBox.Image = ConvertImage(args.Image);
                    Refresh();
                }
            });
        }
        private static Image ConvertImage(SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>? image)
        {
            if (image == null)
                return new Bitmap(1, 1);

            MemoryStream memoryStream = new();
            image.Save(memoryStream, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
            return Bitmap.FromStream(memoryStream);
        }
    }
}