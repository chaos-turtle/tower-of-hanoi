using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tower_Of_Hanoi.Models
{
    public class Score
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public int Moves { get; set; }
        public int Disks { get; set; }
        public bool IsPerfect { get; set; }
        public DateTime DateAchieved { get; set; }
    }

}