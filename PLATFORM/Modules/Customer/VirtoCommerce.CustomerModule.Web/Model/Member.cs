﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public abstract class Member : AuditableEntity
	{
		public Member(string memberType)
		{
			MemberType = memberType;
		}

	
		public abstract string DisplayName { get; }
		public string MemberType { get; set; }
		public ICollection<Address> Addresses { get; set; }
		public ICollection<string> Phones { get; set; }
		public ICollection<string> Emails { get; set; }
		public ICollection<Note> Notes { get; set; }

		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

	}
}