using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDataModels.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tenant_tbl",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(nullable: false),
                    Company = table.Column<string>(nullable: true),
                    ContactName = table.Column<string>(nullable: true),
                    ContactPhone = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenant_tbl", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "user_tbl",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_tbl", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_user_tbl_tenant_tbl_TenantId",
                        column: x => x.TenantId,
                        principalTable: "tenant_tbl",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "models_tbl",
                columns: table => new
                {
                    ModelId = table.Column<Guid>(nullable: false),
                    ModelName = table.Column<string>(nullable: true),
                    Configuration = table.Column<string>(nullable: true),
                    File = table.Column<byte[]>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    DataSourceTableDataSourceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_models_tbl", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK_models_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    DataSourceId = table.Column<Guid>(nullable: false),
                    DataSourceName = table.Column<string>(nullable: true),
                    Configuration = table.Column<string>(nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    ModelTableModelId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.DataSourceId);
                    table.ForeignKey(
                        name: "FK_DataSources_models_tbl_ModelTableModelId",
                        column: x => x.ModelTableModelId,
                        principalTable: "models_tbl",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataSources_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "scheduler_tbl",
                columns: table => new
                {
                    ScheduleId = table.Column<Guid>(nullable: false),
                    ScheduleConfiguration = table.Column<string>(nullable: true),
                    IsScheduled = table.Column<bool>(nullable: false),
                    LastRan = table.Column<DateTime>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ModelId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduler_tbl", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_scheduler_tbl_models_tbl_ModelId",
                        column: x => x.ModelId,
                        principalTable: "models_tbl",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scheduler_tbl_user_tbl_UserId",
                        column: x => x.UserId,
                        principalTable: "user_tbl",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_ModelTableModelId",
                table: "DataSources",
                column: "ModelTableModelId");

            migrationBuilder.CreateIndex(
                name: "IX_DataSources_UserId",
                table: "DataSources",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_models_tbl_DataSourceTableDataSourceId",
                table: "models_tbl",
                column: "DataSourceTableDataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_models_tbl_UserId",
                table: "models_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_scheduler_tbl_ModelId",
                table: "scheduler_tbl",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_scheduler_tbl_UserId",
                table: "scheduler_tbl",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_tbl_TenantId",
                table: "user_tbl",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_models_tbl_DataSources_DataSourceTableDataSourceId",
                table: "models_tbl",
                column: "DataSourceTableDataSourceId",
                principalTable: "DataSources",
                principalColumn: "DataSourceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DataSources_models_tbl_ModelTableModelId",
                table: "DataSources");

            migrationBuilder.DropTable(
                name: "scheduler_tbl");

            migrationBuilder.DropTable(
                name: "models_tbl");

            migrationBuilder.DropTable(
                name: "DataSources");

            migrationBuilder.DropTable(
                name: "user_tbl");

            migrationBuilder.DropTable(
                name: "tenant_tbl");
        }
    }
}
