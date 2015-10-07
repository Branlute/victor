using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;

namespace VictorNamespace
{
    public partial class Explorer : PhoneApplicationPage
    {

        private Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //socket client
        //private IPEndPoint iep = new IPEndPoint(IPAddress.Parse("138.231.143.173"), 4321); 
        //private IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.173.1"), 4321);
        //private IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 4321);
        private IPEndPoint iep = new IPEndPoint(IPAddress.Parse("168.63.134.8"), 4321); //info serveur azure distant
        private byte[] imageByte; //image a envoyer
        private Int32 tailleBack;//taille image a recevoir
        private List<byte> imageByteBack = new List<byte>();//image a recevoir
        private bool isImageSize = false;
        private bool isTheFile = false;
        private BitmapImage img;
        private BitmapImage imgFin;
        //private ManualResetEvent ServerDone = new ManualResetEvent(false);

        public Explorer()
        {
            InitializeComponent();

            imgFin = new BitmapImage();

        }


        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            btnAzure.Visibility = Visibility.Collapsed;
            btnCreate.Visibility = Visibility.Collapsed;

            NavigationService.Navigate(new Uri("/Explorer/ExplorerMenu.xaml", UriKind.Relative));
        }


        void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {

            switch (e.LastOperation)
            {

                case SocketAsyncOperation.Connect:
                    if (e.SocketError == SocketError.Success)
                        connectEventArgs_Completed(e);
                    break;

                case SocketAsyncOperation.Receive:
                    if (isTheFile)
                        receiveAll(e);
                    else
                        onReceivefileSize(e);
                    break;

                case SocketAsyncOperation.Send:
                    if (isImageSize)
                        sentImageEventArgs_Completed(e);
                    else
                        sentSizeEventArgs_Completed(e);
                    break;

                default:
                    throw new Exception("Invalid operation completed");
            }
        }



        //envoi taille image 
        void connectEventArgs_Completed(SocketAsyncEventArgs e)
        {
            Int32 taille = imageByte.Length;
            e.SetBuffer(BitConverter.GetBytes(taille), 0, sizeof(Int32));
            clientSock.SendAsync(e);

        }


        //envoi image
        void sentSizeEventArgs_Completed(SocketAsyncEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                chargementLabel.Text = "Envoie";
            });
            isImageSize = true;
            e.SetBuffer(imageByte, 0, imageByte.Length);
            clientSock.SendAsync(e);
        }


        //reception size image de retour
        void sentImageEventArgs_Completed(SocketAsyncEventArgs e)
        {
            
            byte[] bufferSize = new byte[sizeof(Int32)];
            e.SetBuffer(bufferSize, 0, bufferSize.Length);
            clientSock.ReceiveAsync(e);
            //ServerDone.WaitOne();

        }

        void onReceivefileSize(SocketAsyncEventArgs e)
        {

            tailleBack = BitConverter.ToInt32(e.Buffer, 0);
            byte[] bufferSizeTemp;
            bufferSizeTemp = new byte[tailleBack];
            e.SetBuffer(bufferSizeTemp, 0, bufferSizeTemp.Length);
            isTheFile = true;
            clientSock.ReceiveAsync(e);

        }

        void receiveAll(SocketAsyncEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    chargementLabel.Text = "Reception";
                });
            for (int i = 0; i < e.BytesTransferred; i++)
            {
                imageByteBack.Add(e.Buffer[i]);
            }
            if (imageByteBack.Count < tailleBack)
            {
                byte[] bufferSizeTemp = new byte[tailleBack];
                e.SetBuffer(bufferSizeTemp, 0, bufferSizeTemp.Length);
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    chargementLabel.Text = "" + ((imageByteBack.Count) / tailleBack) * 100;

                });
                clientSock.ReceiveAsync(e);
                //ServerDone.WaitOne();

            }
            else
            {
                receivefull(e);
            }
        }


        private void receivefull(SocketAsyncEventArgs e)
        {
            isImageSize = false;
            isTheFile = false; 
            byte[] bufferSizeTemp = new byte[tailleBack];
            for (int i = 0; i < imageByteBack.Count; i++)
                bufferSizeTemp[i] = imageByteBack[i];

            MemoryStream ms = new MemoryStream(bufferSizeTemp);

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                imgFin.SetSource(ms);
                imgResult.Source = imgFin;
            });

            imageByteBack = new List<byte>();
        }

        //apparution caméra
        private void appBarCamera_Click(object sender, EventArgs e)
        {
            CameraCaptureTask camera = new CameraCaptureTask();
            camera.Completed += new EventHandler<PhotoResult>(camera_Completed);
            camera.Show();
        }

        //apres la prise de la photo
        private void camera_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                img = new BitmapImage();
                img.SetSource(e.ChosenPhoto);
                imgResult.Source = img;

                //On fait apparaître les boutons
                btnAzure.Visibility = Visibility.Visible;
                btnCreate.Visibility = Visibility.Visible;

                /*imageByte = new byte[e.ChosenPhoto.Length];
                e.ChosenPhoto.Read(imageByte, 0, imageByte.Length);//img to byte
                SocketAsyncEventArgs connectEventArgs = new SocketAsyncEventArgs();
                connectEventArgs.UserToken = clientSock;
                connectEventArgs.RemoteEndPoint = iep;
                connectEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SocketEventArg_Completed);
                clientSock.ConnectAsync(connectEventArgs);*/
            }
        }

        private void appBarChoix_Click(object sender, EventArgs e)
        {
            PhotoChooserTask task = new PhotoChooserTask();
            task.Completed += new EventHandler<PhotoResult>(task_Completed);
            task.Show();
        }

        private void task_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                img = new BitmapImage();
                img.SetSource(e.ChosenPhoto);

                //On vérifie que l'image est bien en paysage
                if (img.PixelHeight < img.PixelWidth)
                {
                    if (img.PixelWidth / img.PixelHeight != 800 / 480)
                    {
                        MessageBox.Show("Your photo can be deformed", "Warning", MessageBoxButton.OK);
                    }

                    imgResult.Source = img;

                    //On fait apparaître les boutons
                    btnAzure.Visibility = Visibility.Visible;
                    btnCreate.Visibility = Visibility.Visible;
                    
                }
                //Image portrait
                else
                {
                    MessageBox.Show("You have to choose a photo into landscape format", "Error", MessageBoxButton.OK);
                }  
            }
        }

        private void btnAzure_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This functionnality doesn't work yet !", "Coming soon", MessageBoxButton.OK);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            App.Image = img;

            NavigationService.Navigate(new Uri("/Explorer/Editor.xaml", UriKind.Relative));
        }
    }
}