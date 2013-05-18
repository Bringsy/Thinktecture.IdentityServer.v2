using System;
using System.ComponentModel.DataAnnotations;

namespace Thinktecture.IdentityServer.Repositories.Sql.Configuration
{
    public class ClaimInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Type { get; set; }

        [Required]
        public String Value { get; set; }
    }
}
