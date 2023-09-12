using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Worker_GrpcService.DAL.Models
{
    [Table("Workers")]
    public class Worker
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string FirstName { get; set; }

        [Column("lastName")]
        public string LastName { get; set; }

        [Column("patronymic")]
        public string Patronymic { get; set; }

        [Column("birthDate")]
        public string BirthDate { get; set; }

        [Column("hasChildren")]
        public bool HasChildren { get; set; }

        [Column("gender")]
        public string Gender { get; set; }
    }
}
