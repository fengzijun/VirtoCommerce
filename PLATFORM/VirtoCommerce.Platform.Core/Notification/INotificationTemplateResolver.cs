﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notification
{
	public interface INotificationTemplateResolver
	{
		void ResolveTemplate(Notification notification);

		NotificationParameter[] ResolveNotificationParameters(Notification notification);
	}
}
