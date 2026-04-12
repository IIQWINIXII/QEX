using QEX.Components.Extensions.Installed.QEX_Chats.Classes;
using QEX_Chat.Classes;
using QEX_Lib.ClientDB.Model;
using System.Collections.ObjectModel;

namespace QEX_Chat.Services
{
    public class ChatService
    {
        // Режимы боковой панели
        public enum ChatMode
        {
            DirectMessages,
            Guilds
        }

        public ChatMode CurrentMode { get; private set; } = ChatMode.DirectMessages;

        // Группы
        public List<Guild> Guilds { get; set; } = new();

        // Личные чаты
        public List<DirectChat> DirectChats { get; set; } = new();

        // Текущие выбранные элементы
        public Guild? CurrentGuild { get; private set; } 
        public СhatChannel? CurrentChannel { get; private set; }
        public DirectChat? CurrentDirectChat { get; private set; }

        public event Action? OnUIChanged;
        public event Action? OnMessagesChanged;

        public ChatService()
        {
            // Пример группы
            var guild = new Guild { Name = "QEX Team" };
            guild.TextChannels.Add(new СhatChannel { Name = "общий" });
            guild.TextChannels.Add(new СhatChannel { Name = "разработка" });
            guild.VoiceChannels.Add(new VoiceChannel { Name = "Голосовой 1" });
            var guild1 = new Guild { Name = "Work" };
            guild1.TextChannels.Add(new СhatChannel { Name = "общий" });
            guild1.TextChannels.Add(new СhatChannel { Name = "разработка 11" });
            guild1.VoiceChannels.Add(new VoiceChannel { Name = "Голосовой 111" });

            Guilds.Add(guild1);
            Guilds.Add(guild);
            CurrentGuild = guild;
            CurrentChannel = guild.TextChannels.First();
        }

        public void SwitchMode(ChatMode mode)
        {
            CurrentMode = mode;
            OnUIChanged?.Invoke();
        }

        public void SelectGuild(Guild guild)
        {
            CurrentGuild = guild;
            CurrentDirectChat = null;
            CurrentChannel = guild.TextChannels.FirstOrDefault();
            OnUIChanged?.Invoke();
        }

        public void SelectChannel(СhatChannel channel)
        {
            CurrentChannel = channel;
            CurrentDirectChat = null;
            OnUIChanged?.Invoke();
        }

        public void SelectDirectChat(DirectChat chat)
        {
            CurrentDirectChat = chat;
            CurrentGuild = null;
            CurrentChannel = null;
            OnUIChanged?.Invoke();
        }
        public DirectChat GetOrCreateDirectChat(User friend)
        {
            // Ищем существующий личный чат
            var chat = DirectChats.FirstOrDefault(c => c.User.ID == friend.ID);

            // Если нет — создаём новый
            if (chat == null)
            {
                chat = new DirectChat
                {
                    User = friend,
                    Messages = new ObservableCollection<ChatMessage>()
                };

                DirectChats.Add(chat);
            }

            return chat;
        }


        public async Task SendMessage(User sender, string text)
        {
            var msg = new ChatMessage
            {
                Sender = sender,
                Message = text,
                Timestamp = DateTime.Now
            };

            if (CurrentChannel != null)
                CurrentChannel.Messages.Add(msg);
            else if (CurrentDirectChat != null)
                CurrentDirectChat.Messages.Add(msg);
            else
                return; // Нет выбранного канала или личного чата

            OnMessagesChanged?.Invoke();
        }
    }
}

