using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AjorApi.Models
{
    public class Contributor
    {
        [Key]
        public int Id { get; set; }
        public int UsersId { get; set; }
        [ForeignKey("UsersId")]
        public int ContributionId { get; set; }
        [ForeignKey("ContributionId")]
        public int Position { get; set; }
        public Users? Users { get; set; }
        public Contribution? Contributions { get; set; }
    }
}