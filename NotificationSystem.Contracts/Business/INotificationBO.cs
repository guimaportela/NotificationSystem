using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Contracts.Business
{
    public interface INotificationBO
    {
        Task Send(string type, string userId, string message);
        bool IsRateLimited(string type, string userId, Queue<DateTime> notificationsInWindow);
        void NotificationsHistoryHandler(string notificationKey, Queue<DateTime> notificationsInWindow);
        void OldestNotificationHandler(string notificationKey, Queue<DateTime> notificationsInWindow);
    }
}
