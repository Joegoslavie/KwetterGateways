﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.ServiceLayer.Model
{
    public class Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WebsiteUri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Tweet> Tweets { get; set; } = new List<Tweet>();

        /// <summary>
        /// 
        /// </summary>
        public List<Account> Following { get; set; } = new List<Account>();

        /// <summary>
        /// 
        /// </summary>
        public List<Account> Followers { get; set; } = new List<Account>();

        /// <summary>
        /// 
        /// </summary>
        public List<Account> Blocked { get; set; } = new List<Account>();
    }
}
