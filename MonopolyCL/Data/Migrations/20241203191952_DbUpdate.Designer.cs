﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MonopolyCL.Data;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20241203191952_DbUpdate")]
    partial class DbUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("MonopolyCL.Models.Cards.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CardTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeckId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CardTypeId");

                    b.HasIndex("DeckId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.CardDeck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("DiffRating")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CardDecks");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.CardGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<int>("DeckId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastPlayed")
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeckId");

                    b.ToTable("CardGames");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.CardToGame", b =>
                {
                    b.Property<int>("CardId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("Index")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CardId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("CardsToGames");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.CardType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CardTypes");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.TypeToGame", b =>
                {
                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("CurrentIndex")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TypeId", "GameId");

                    b.HasIndex("GameId");

                    b.ToTable("TypesToGames");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Card", b =>
                {
                    b.HasOne("MonopolyCL.Models.Cards.CardType", "CardType")
                        .WithMany()
                        .HasForeignKey("CardTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MonopolyCL.Models.Cards.CardDeck", "CardDeck")
                        .WithMany()
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CardDeck");

                    b.Navigation("CardType");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.CardGame", b =>
                {
                    b.HasOne("MonopolyCL.Models.Cards.CardDeck", "Deck")
                        .WithMany()
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Deck");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.CardToGame", b =>
                {
                    b.HasOne("MonopolyCL.Models.Cards.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MonopolyCL.Models.Cards.CardGame", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.TypeToGame", b =>
                {
                    b.HasOne("MonopolyCL.Models.Cards.CardGame", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MonopolyCL.Models.Cards.CardType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Type");
                });
#pragma warning restore 612, 618
        }
    }
}