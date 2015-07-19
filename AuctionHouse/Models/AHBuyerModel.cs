﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Models
{
    public class AHBuyer
    {
        public string ID { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "General Contact Info")]
        public string ContactInfo { get; set; }
    }
}