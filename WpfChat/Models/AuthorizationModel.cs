using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using WpfChat.Entities;
using WpfChat.Workers;
using ChatServer_1.Core.Networking;
using WpfChat.Handlers;
using System.Windows;
using WpfChat.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace WpfChat
{
    class AuthorizationModel : INotifyPropertyChanged
    {
        Dispatcher dispatcher;
        TcpClient Client;
        NetworkStream Stream;
        string data = "";
        User user = null;
        public AuthorizationModel()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            try
            {
                Client = new TcpClient("", 5400);
                Stream = Client.GetStream();
            }
            catch (Exception)
            {

            }
        }
        private string login { get; set; }
        private string password { get; set; }
        public bool Online 
        {
            get
            {
                if (Client != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public string Login
        { 
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public string OnlineText
        {
            get
            {
                if (Online)
                {
                    return "Состояние сервера: В сети!";
                }
                else
                {
                    return "Состояние сервера: Недоступен...";
                }
            }
        }
        public RelayCommand Logining
        {
            get
            {
                return new RelayCommand(
                    obj =>
                    {
                        user = new User(Login, Password);
                        PacketSender.SendJsonString(Stream, "LOGINING::" +  JsonWorker.UserToJson(user));
                        while (true)
                        {
                            data = PacketRecipient.GetJsonData(Stream);
                            user = JsonWorker.JsonToUser(data);
                            if (user.SessionKey == "ERROR::BAD::LOGIN")
                            {
                                MessageBox.Show("Неверные данные!");
                                break;
                            }
                            else
                            {
                                user = JsonWorker.JsonToUser(data);
                                ChatWindow chat = new ChatWindow(ref Client, ref Stream);
                                chat.Show();
                                Application.Current.MainWindow.Close();
                                //MessageBox.Show("Успешная авторизация!");
                                break;
                            }
                        }
                        data = "";
                        user = null;
                    }
                    );
            }
        }
        public RelayCommand Registration
        {
            get
            {
                return new RelayCommand(
                    obj =>
                    {
                        user = new User(Login, Password);
                        PacketSender.SendJsonString(Stream, "REGISTER::" + JsonWorker.UserToJson(user));
                        while (true)
                        {
                            data = PacketRecipient.GetJsonData(Stream);
                            user = JsonWorker.JsonToUser(data);
                            if (user.SessionKey == "ERROR:EXISTS::USER")
                            {
                                MessageBox.Show("Пользователь существует!");
                                break;
                            }
                            else
                            {
                                user = JsonWorker.JsonToUser(data);
                                MessageBox.Show("Успешная регистрация!");
                                break;
                            }
                        }
                        data = "";
                        user = null;
                    }
                    );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
