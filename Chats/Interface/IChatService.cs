using Chats.Classes;
using QEX_Lib.ClientDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chats.Services
{
    public interface IChatService
    {
        // Режимы боковой панели
        ChatService.ChatMode CurrentMode { get; }

        // Группы
        List<Guild> Guilds { get; }

        // Личные чаты
        List<DirectChat> DirectChats { get; }

        // Текущие выбранные элементы
        Guild? CurrentGuild { get; }
        СhatChannel? CurrentChannel { get; }
        DirectChat? CurrentDirectChat { get; }

        event Action? OnUIChanged;
        event Action? OnMessagesChanged;

        void SwitchMode(ChatService.ChatMode mode);
        void SelectGuild(Guild guild);
        void SelectChannel(СhatChannel channel);
        void SelectDirectChat(DirectChat chat);
        DirectChat GetOrCreateDirectChat(User friend);
        Task SendMessage(User sender, string text);
    }
}