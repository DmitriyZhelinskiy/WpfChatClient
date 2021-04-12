using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfChat.Entities;

namespace WpfChat.Workers
{
    public static class JsonWorker
    {
        //  Сериализация объекта типа User в Json строку
        public static string UserToJson(User user)
        {
            var settings = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(user, settings);
        }
        public static User JsonToUser(string jsonData)
        {
            return JsonSerializer.Deserialize<User>(jsonData);
        }

        //  Сериализация объекта типа Message в Json строку
        public static string MessageToJson(Message message)
        {
            var settings = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(message, settings);
        }
        public static Message JsonToMessage(string jsonData)
        {
            return JsonSerializer.Deserialize<Message>(jsonData);
        }

        public static ObservableCollection<string> JsonToUsersList(string data)
        {
            var temp = new ObservableCollection<string>();
            var list = JsonSerializer.Deserialize<List<User>>(data);
            foreach (var item in list)
            {
                temp.Add(item.Login);
            }
            return temp;
        }

    }
}
