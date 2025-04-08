using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Db.Migrations
{
	/// <inheritdoc />
	public partial class RemoveImagePathFromProductsTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
			   name: "ImagePath",
			   table: "Products");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
		name: "ImagePath",
		table: "Products",
		type: "nvarchar(max)", // Соответствует конфигурации из OnModelCreating
		nullable: false);
		}
	}
}
