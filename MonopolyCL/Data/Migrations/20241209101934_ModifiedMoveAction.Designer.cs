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
    [Migration("20241209101934_ModifiedMoveAction")]
    partial class ModifiedMoveAction
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("MonopolyCL.Models.Boards.DataModel.BoardDM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("MonopolyCL.Models.Boards.DataModel.BoardToProperty", b =>
                {
                    b.Property<int>("BoardId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PropertyName")
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("BoardId", "PropertyName", "TenantId");

                    b.HasIndex("PropertyName", "TenantId");

                    b.ToTable("BoardsToProperties");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Actions.AdvanceAction", b =>
                {
                    b.Property<int>("AdvanceIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Colour")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Set")
                        .HasColumnType("INTEGER");

                    b.HasKey("AdvanceIndex");

                    b.ToTable("AdvanceActions");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Actions.CardAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Action")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ActionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CardId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.ToTable("CardActions");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Actions.KeepAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("KeepActions");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Actions.MoveAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsForward")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MoveAmount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MoveActions");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Actions.PayPlayerAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AmountToPlayer")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PayToType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PayPlayerActions");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Actions.StreetRepairsAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HotelCost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HouseCost")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("StreetRepairsActions");
                });

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

            modelBuilder.Entity("MonopolyCL.Models.Game.GameDM", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastPlayed")
                        .HasColumnType("TEXT");

                    b.Property<int>("Rules")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("MonopolyCL.Models.Players.DataModel.DiceNumbers", b =>
                {
                    b.Property<int>("GamePlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DiceOne")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DiceTwo")
                        .HasColumnType("INTEGER");

                    b.HasKey("GamePlayerId");

                    b.ToTable("PlayerDiceNumbers");
                });

            modelBuilder.Entity("MonopolyCL.Models.Players.DataModel.GamePlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("BoardIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsInJail")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("JailCost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Money")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TripleBonus")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerName", "TenantId");

                    b.ToTable("GamePlayers");
                });

            modelBuilder.Entity("MonopolyCL.Models.Players.DataModel.PlayerDM", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name", "TenantId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("MonopolyCL.Models.Properties.DataModel.GameProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BuiltLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCompleteSet")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMortgaged")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsOwned")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OwnerName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PropertyTenantId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerName", "TenantId");

                    b.HasIndex("PropertyName", "PropertyTenantId");

                    b.ToTable("GameProps");
                });

            modelBuilder.Entity("MonopolyCL.Models.Properties.DataModel.PropertyDM", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("TenantId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("BoardIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cost")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Set")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Name", "TenantId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("MonopolyCL.Models.Boards.DataModel.BoardToProperty", b =>
                {
                    b.HasOne("MonopolyCL.Models.Boards.DataModel.BoardDM", "Board")
                        .WithMany()
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MonopolyCL.Models.Properties.DataModel.PropertyDM", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyName", "TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("MonopolyCL.Models.Cards.Actions.CardAction", b =>
                {
                    b.HasOne("MonopolyCL.Models.Cards.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
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

            modelBuilder.Entity("MonopolyCL.Models.Game.GameDM", b =>
                {
                    b.HasOne("MonopolyCL.Models.Boards.DataModel.BoardDM", "BoardDataModel")
                        .WithMany()
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardDataModel");
                });

            modelBuilder.Entity("MonopolyCL.Models.Players.DataModel.DiceNumbers", b =>
                {
                    b.HasOne("MonopolyCL.Models.Players.DataModel.GamePlayer", "Player")
                        .WithMany()
                        .HasForeignKey("GamePlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MonopolyCL.Models.Players.DataModel.GamePlayer", b =>
                {
                    b.HasOne("MonopolyCL.Models.Players.DataModel.PlayerDM", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerName", "TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("MonopolyCL.Models.Properties.DataModel.GameProperty", b =>
                {
                    b.HasOne("MonopolyCL.Models.Players.DataModel.PlayerDM", "Player")
                        .WithMany()
                        .HasForeignKey("OwnerName", "TenantId");

                    b.HasOne("MonopolyCL.Models.Properties.DataModel.PropertyDM", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyName", "PropertyTenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Property");
                });
#pragma warning restore 612, 618
        }
    }
}