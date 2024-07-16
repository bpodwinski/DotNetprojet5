using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpressVoitures.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteCascadeForModelBrandTrimLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrimLevels_Models_ModelId",
                table: "TrimLevels");

            migrationBuilder.AddForeignKey(
                name: "FK_TrimLevels_Models_ModelId",
                table: "TrimLevels",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrimLevels_Models_ModelId",
                table: "TrimLevels");

            migrationBuilder.AddForeignKey(
                name: "FK_TrimLevels_Models_ModelId",
                table: "TrimLevels",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
