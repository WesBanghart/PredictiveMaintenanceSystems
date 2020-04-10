using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataModels.Migrations
{
    public partial class EFContextChangeOnDeleteRev1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl");

            migrationBuilder.DropForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl");

            migrationBuilder.AddForeignKey(
                name: "FK_DataSources_user_tbl_UserId",
                table: "DataSources",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_models_tbl_user_tbl_UserId",
                table: "models_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_scheduler_tbl_user_tbl_UserId",
                table: "scheduler_tbl",
                column: "UserId",
                principalTable: "user_tbl",
                principalColumn: "UserId");
        }
    }
}
