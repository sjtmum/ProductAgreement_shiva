using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgMg_SandeepTrivedi.Models
{
	[Table("ProductGroup")]
	public class ProductGroup : ICommonColumns
	{
		[Key]
		[Column("Id")]
		public int Id { get; set; }
		[Column("UserId")]
		public string UserId { get; set; }
		[Column("GroupCode")]
		public string GroupCode { get; set; }
		[Column("GroupDescription")]
		public string GroupDescription { get; set; }
		[Column("Active")]
		public bool Active { get; set; }
	}
}
