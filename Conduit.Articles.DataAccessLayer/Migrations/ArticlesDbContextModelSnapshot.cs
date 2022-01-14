﻿// <auto-generated />
using System;
using Conduit.Articles.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Conduit.Articles.DataAccessLayer.Migrations
{
    [DbContext(typeof(ArticlesDbContext))]
    partial class ArticlesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ArticleDbModelAuthorDbModel", b =>
                {
                    b.Property<Guid>("FavoritersId")
                        .HasColumnType("uuid")
                        .HasColumnName("favoriter_id");

                    b.Property<Guid>("FavoritesId")
                        .HasColumnType("uuid")
                        .HasColumnName("favorite_id");

                    b.HasKey("FavoritersId", "FavoritesId")
                        .HasName("pk_author_favorite");

                    b.HasIndex("FavoritesId")
                        .HasDatabaseName("ix_author_favorite_favorite_id");

                    b.ToTable("author_favorite", (string)null);
                });

            modelBuilder.Entity("ArticleDbModelTagDbModel", b =>
                {
                    b.Property<Guid>("ArticlesId")
                        .HasColumnType("uuid")
                        .HasColumnName("article_id");

                    b.Property<Guid>("TagsId")
                        .HasColumnType("uuid")
                        .HasColumnName("tag_id");

                    b.HasKey("ArticlesId", "TagsId")
                        .HasName("pk_tag_article");

                    b.HasIndex("TagsId")
                        .HasDatabaseName("ix_tag_article_tag_id");

                    b.ToTable("tag_article", (string)null);
                });

            modelBuilder.Entity("AuthorDbModelAuthorDbModel", b =>
                {
                    b.Property<Guid>("FollowedsId")
                        .HasColumnType("uuid")
                        .HasColumnName("followed_id");

                    b.Property<Guid>("FollowersId")
                        .HasColumnType("uuid")
                        .HasColumnName("follower_id");

                    b.HasKey("FollowedsId", "FollowersId")
                        .HasName("pk_author_follower");

                    b.HasIndex("FollowersId")
                        .HasDatabaseName("ix_author_follower_follower_id");

                    b.ToTable("author_follower", (string)null);
                });

            modelBuilder.Entity("Conduit.Articles.DataAccessLayer.Models.ArticleDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("article_id");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("body");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("FavoritesCount")
                        .HasColumnType("integer")
                        .HasColumnName("favorites_count");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_article");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_article_author_id");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("ix_article_created_at");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasDatabaseName("ix_article_slug");

                    b.ToTable("article", (string)null);
                });

            modelBuilder.Entity("Conduit.Articles.DataAccessLayer.Models.AuthorDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("author_id");

                    b.Property<string>("Bio")
                        .HasColumnType("text")
                        .HasColumnName("bio");

                    b.Property<string>("Image")
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_author");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_author_username");

                    b.ToTable("author", (string)null);
                });

            modelBuilder.Entity("Conduit.Articles.DataAccessLayer.Models.TagDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("tag_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_tag");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_tag_name");

                    b.ToTable("tag", (string)null);
                });

            modelBuilder.Entity("ArticleDbModelAuthorDbModel", b =>
                {
                    b.HasOne("Conduit.Articles.DataAccessLayer.Models.AuthorDbModel", null)
                        .WithMany()
                        .HasForeignKey("FavoritersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_favorite_author_favoriter_id");

                    b.HasOne("Conduit.Articles.DataAccessLayer.Models.ArticleDbModel", null)
                        .WithMany()
                        .HasForeignKey("FavoritesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_favorite_article_favorite_id");
                });

            modelBuilder.Entity("ArticleDbModelTagDbModel", b =>
                {
                    b.HasOne("Conduit.Articles.DataAccessLayer.Models.ArticleDbModel", null)
                        .WithMany()
                        .HasForeignKey("ArticlesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tag_article_article_article_id");

                    b.HasOne("Conduit.Articles.DataAccessLayer.Models.TagDbModel", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tag_article_tag_tag_id");
                });

            modelBuilder.Entity("AuthorDbModelAuthorDbModel", b =>
                {
                    b.HasOne("Conduit.Articles.DataAccessLayer.Models.AuthorDbModel", null)
                        .WithMany()
                        .HasForeignKey("FollowedsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_follower_author_followed_id");

                    b.HasOne("Conduit.Articles.DataAccessLayer.Models.AuthorDbModel", null)
                        .WithMany()
                        .HasForeignKey("FollowersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_author_follower_author_follower_id");
                });

            modelBuilder.Entity("Conduit.Articles.DataAccessLayer.Models.ArticleDbModel", b =>
                {
                    b.HasOne("Conduit.Articles.DataAccessLayer.Models.AuthorDbModel", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_article_author_author_id");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Conduit.Articles.DataAccessLayer.Models.AuthorDbModel", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
