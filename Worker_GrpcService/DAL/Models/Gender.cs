using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Worker_GrpcService.DAL.Models
{
    [Table("Genders")]
    public class Gender
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        #region Навигационные свойства
        public List<Worker> Workers { get; set; }

        #endregion

    }
}
