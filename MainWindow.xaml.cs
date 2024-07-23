
using LaserVL.config;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows;


namespace LaserVL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private LaserConfigs laserConfigs;
        private TcpClient connexion;
        private NetworkStream stream;
        public MainWindow()
        {
            InitializeComponent();
            laserConfigs = LaserConfigs.LoadLaserConfigs(Path.Combine("config", "LaserConfigs.xml"));
            TryConnection();

        }


        public bool TryConnection()
        {
            try
            {
                this.connexion = new TcpClient(laserConfigs.IpAddress, laserConfigs.Port);
                this.stream = connexion.GetStream();
                return true;
            }
            catch { 
                LogTextBox.Text = "No connexion";
            }
            return false;
        }

        ~MainWindow()
        {
            if (!this.isConnexionEstablished()) return;
            this.stream.Dispose();
            this.connexion.Dispose();
        }
        public bool isConnexionEstablished()
        {
            return this.connexion != null;
        }

        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            if (!this.isConnexionEstablished()) return;
            this.stream.Dispose();
            this.connexion.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes(laserConfigs?.commands?.CommandList.FirstOrDefault(cmd => cmd.Name == "ReadDeviceStatus").Send);
            this.stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = this.stream.Read(buffer, 0, buffer.Length);
            LogTextBox.Text = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
    }
}