using BuisnessLayer.Abstract;
using DataAcsessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Concrete
{
	public class NotificationManager : INotificationService

	{
		private INotificationRepository _notificationRepository;

		public NotificationManager(INotificationRepository notificationRepository)
		{
			_notificationRepository= notificationRepository;
		}

		public void Create(Notification notification)
		{
			_notificationRepository.Create(notification);
		}
	}
}
