﻿// <auto-generated />
using Checkers.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Checkers.Infrastructure.Migrations
{
    [DbContext(typeof(CheckersDbContext))]
    partial class CheckersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("Checkers.Models.DbModels.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("P1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("P2")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StartingPlayer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Winner")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Checkers.Models.DbModels.GameDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("P1_Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("P2_Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GamesDetails");
                });

            modelBuilder.Entity("Checkers.Models.DbModels.Moves", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("P1_Moves")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("P2_Moves")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Moves");
                });
#pragma warning restore 612, 618
        }
    }
}