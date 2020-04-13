using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace bcfamilyalbum_db.Database.Migrations
{
    [DbContext(contextType: typeof(FamilyAlbumDbContext))]
    [Migration("20200413210500_InitialCreate")]
    public partial class InitialCreate : Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "0.0.1");
            base.BuildTargetModel(modelBuilder);
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RelativePath = table.Column<string>(nullable: false),
                    RemovalTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.UniqueConstraint("UNIQ_RelativePath", x => x.RelativePath);
                });

            migrationBuilder.CreateTable(
                name: "MovedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    RelativePath = table.Column<string>(nullable: false),
                    OriginalRelativePath = table.Column<string>(nullable: false),
                    RemovalTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.UniqueConstraint("UNIQ_RelativePath", x => x.RelativePath);
                    table.UniqueConstraint("UNIQ_OriginalRelativePath", x => x.RelativePath);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovedFiles");
            migrationBuilder.DropTable(
                name: "DeletedFiles");
        }
    }
}
