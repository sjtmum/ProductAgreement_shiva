using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AgMg_SandeepTrivedi.Models
{
	[Table("Products")]
    public class Product
    {
		[Key]
		[Column("Id")]
		public int Id { get; set; }

		[Column("UserId")]
		public string UserId { get; set; }
		
		[Column("ProductGroupId")]
		public int ProductGroupId { get; set; }

		[NotMapped]
		public string ProductGroupDesc { get; set; }

		[Column("ProductNumber")]
		public string ProductNumber { get; set; }

		[Column("ProductDescription")]
		public string ProductDescription { get; set; }

		[Column("Active")]
		public bool Active { get; set; }

		[Column("Price")]
		public decimal Price{ get; set; }

	}
}
