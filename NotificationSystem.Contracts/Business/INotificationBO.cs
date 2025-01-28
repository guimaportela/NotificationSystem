using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Contracts.Business
{
    public interface INotificationBO
    {
        Task Send(NotificationDTO notificationDTO);
        bool IsRateLimited(NotificationDTO notificationDTO, Queue<DateTime> notificationsInWindow);
        void NotificationsHistoryHandler(string notificationKey, Queue<DateTime> notificationsInWindow);
        void OldestNotificationHandler(string notificationKey, Queue<DateTime> notificationsInWindow);
    }
}
