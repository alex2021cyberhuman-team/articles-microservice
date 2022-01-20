using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Articles.DataAccessLayer.Migrations
{
    public partial class CreateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "author",
                columns: table => new
                {
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_author", x => x.author_id);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    article_id = table.Column<Guid>(type: "uuid", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    favorites_count = table.Column<int>(type: "integer", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_article", x => x.article_id);
                    table.ForeignKey(
                        name: "fk_article_author_author_id",
                        column: x => x.author_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "author_follower",
                columns: table => new
                {
                    followeds_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followers_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_author_follower", x => new { x.followeds_id, x.followers_id });
                    table.ForeignKey(
                        name: "fk_author_follower_author_followeds_id",
                        column: x => x.followeds_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_author_follower_author_followers_id",
                        column: x => x.followers_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "author_favorite",
                columns: table => new
                {
                    favoriters_id = table.Column<Guid>(type: "uuid", nullable: false),
                    favorites_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_author_favorite", x => new { x.favoriters_id, x.favorites_id });
                    table.ForeignKey(
                        name: "fk_author_favorite_article_favorites_id",
                        column: x => x.favorites_id,
                        principalTable: "article",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_author_favorite_author_favoriters_id",
                        column: x => x.favoriters_id,
                        principalTable: "author",
                        principalColumn: "author_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_article",
                columns: table => new
                {
                    articles_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tags_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag_article", x => new { x.articles_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_tag_article_article_articles_id",
                        column: x => x.articles_id,
                        principalTable: "article",
                        principalColumn: "article_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tag_article_tag_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tag",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_article_author_id",
                table: "article",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "ix_article_created_at",
                table: "article",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_article_slug",
                table: "article",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_author_username",
                table: "author",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_author_favorite_favorites_id",
                table: "author_favorite",
                column: "favorites_id");

            migrationBuilder.CreateIndex(
                name: "ix_author_follower_followers_id",
                table: "author_follower",
                column: "followers_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_name",
                table: "tag",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tag_article_tags_id",
                table: "tag_article",
                column: "tags_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "author_favorite");

            migrationBuilder.DropTable(
                name: "author_follower");

            migrationBuilder.DropTable(
                name: "tag_article");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "author");
        }
    }
}
