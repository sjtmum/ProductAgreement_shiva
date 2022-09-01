using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AgMg_SandeepTrivedi.Models
{
	[Table("Agreements")]
	public class Agreement : ICommonColumns
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

		[Column("ProductId")]
		public int ProductId { get; set; }
		[NotMapped]
		public string ProductDesc { get; set; }

		[Column("ProductPrice")]
		public decimal ProductPrice { get; set; }

		[Column("NewPrice")]
		public decimal NewPrice { get; set; }

		[Column("EffectiveDate")]
		public DateTime EffectiveDate { get; set; }

		[Column("ExpirationDate")]
		public DateTime ExpirationDate { get; set; }

		[Column("Active")]
		public bool Active { get; set; }

	}
}
