﻿using System.ComponentModel.DataAnnotations;

namespace ComplaintBox.Web.Models
{
    public class SetSubjectTitle : AdminViewModel
    {
        [Display(Name = "Topic Title")]
        public string NewSubjectTitle { get; set; }
    }
}