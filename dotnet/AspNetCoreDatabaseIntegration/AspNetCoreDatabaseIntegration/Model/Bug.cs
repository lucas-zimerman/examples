using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreDatabaseIntegration.Model
{
    [Table("Bug")]
    public class Bug
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }

    }
}
