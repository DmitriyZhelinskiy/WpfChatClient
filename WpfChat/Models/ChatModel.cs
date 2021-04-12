using ChatServer_1.Core.Networking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfChat.Entities;
using WpfChat.Workers;

namespace WpfChat.Models
{
    class ChatModel : INotifyPropertyChanged
    {
        Dispatcher dispatcher;
        TcpClient Client;
        NetworkStream Stream;
        private string clientData { get; set; } = "";
        public string ClientData
        {
            get
            {
                return clientData;
            }
            set
            {
                clientData = value;
                OnPropertyChanged("ClientData");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ChatModel(ref TcpClient client, ref NetworkStream Stream)
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            this.Client = client;
            this.Stream = Stream;
            Thread thread = new Thread(new ThreadStart(ThreadWorker));
            thread.Start();
        }
        private ObservableCollection<string> messages { get; set; } = new ObservableCollection<string>();
        private ObservableCollection<string> users { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                OnPropertyChanged("Messages");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<string> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }
        public RelayCommand SendMessage
        {
            get
            {
                return new RelayCommand(
                    obj =>
                    {
                        if (ClientData.Length != 0)
                        {
                            PacketSender.SendJsonString(Stream, "MESSAGE::" + ClientData);
                        }
                    }
                    );
            }
        }
        public void ThreadWorker()
        {
            try
            {
                string data = "";
                PacketSender.SendJsonString(Stream, "STARTAPP::");
                while (true)
                {
                    Thread.Sleep(1000);
                    data = PacketRecipient.GetJsonData(Stream);

                    //  Обработка данных, которые отправляет сервер
                    if (data.Contains("USERS_LIST::"))
                    {
                        data = data.Remove(0, "USERS_LIST::".Length);
                        dispatcher.Invoke(new Action(() => { Users = JsonWorker.JsonToUsersList(data); }));
                    }
                    if (data.Contains("MESSAGE::"))
                    {
                        data = data.Remove(0, "MESSAGE::".Length);
                        dispatcher.Invoke(new Action(() => { Messages.Add(data); }));
                    }

                    //  Обработка данных, которые отправляет клиент
                    if (ClientData.Length != 0)
                    {
                        PacketSender.SendJsonString(Stream, ClientData);
                        ClientData = "";
                    }
                    data = "";
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Stream.Close();
                Client.Close();
            }
        }
    }
}
