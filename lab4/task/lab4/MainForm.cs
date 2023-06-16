namespace lab4
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            cbMethod.SelectedIndex = 0;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            string url = tbSrc.Text;
            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }
            string res = "";
            url += tbPath.Text;

            switch (cbMethod.SelectedIndex)
            {
                case 0:
                    res = Client.HttpGet(url);
                    break;
                case 1:
                    res = Client.HttpPost(url, "test");
                    break;
                case 2:
                    res = Client.HttpHead(url);
                    break;
                default:
                    break;
            }

            tbAnswer.Text += res;
        }

        private void Answer_DoubleClick(object sender, EventArgs e)
        {
            tbAnswer.Clear();
        }
    }
}