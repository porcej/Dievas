﻿// <auto-generated />
using System;
using Dievas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dievas.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("CampaignMessage", b =>
                {
                    b.Property<int>("CampaignsCampaignId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MessagesMessageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CampaignsCampaignId", "MessagesMessageId");

                    b.HasIndex("MessagesMessageId");

                    b.ToTable("CampaignMessage");
                });

            modelBuilder.Entity("Dievas.Models.Auth.UserModel", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.Property<string>("Roles")
                        .HasColumnType("TEXT");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Dievas.Models.Messages.Campaign", b =>
                {
                    b.Property<int>("CampaignId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("CampaignId");

                    b.ToTable("Campaigns");
                });

            modelBuilder.Entity("Dievas.Models.Messages.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Approved")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Emergent")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MessageTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("MessageId");

                    b.HasIndex("MessageTypeId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Dievas.Models.Messages.MessageType", b =>
                {
                    b.Property<int>("MessageTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("MessageTypeId");

                    b.ToTable("MessageTypes");

                    b.HasData(
                        new
                        {
                            MessageTypeId = 1,
                            Name = "full-page"
                        },
                        new
                        {
                            MessageTypeId = 2,
                            Name = "marquee"
                        });
                });

            modelBuilder.Entity("Dievas.Models.Settings.SettingType", b =>
                {
                    b.Property<int>("SettingTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("InputControlType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("SettingTypeId");

                    b.ToTable("SettingTypes");

                    b.HasData(
                        new
                        {
                            SettingTypeId = 1,
                            InputControlType = "text",
                            Name = "text"
                        },
                        new
                        {
                            SettingTypeId = 2,
                            InputControlType = "button",
                            Name = "button"
                        },
                        new
                        {
                            SettingTypeId = 3,
                            InputControlType = "checkbox",
                            Name = "checkbox"
                        },
                        new
                        {
                            SettingTypeId = 4,
                            InputControlType = "color",
                            Name = "color"
                        },
                        new
                        {
                            SettingTypeId = 5,
                            InputControlType = "date",
                            Name = "date"
                        },
                        new
                        {
                            SettingTypeId = 6,
                            InputControlType = "datetime-local",
                            Name = "datetime-local"
                        },
                        new
                        {
                            SettingTypeId = 7,
                            InputControlType = "email",
                            Name = "email"
                        },
                        new
                        {
                            SettingTypeId = 8,
                            InputControlType = "file",
                            Name = "file"
                        },
                        new
                        {
                            SettingTypeId = 9,
                            InputControlType = "hidden",
                            Name = "hidden"
                        },
                        new
                        {
                            SettingTypeId = 10,
                            InputControlType = "image",
                            Name = "image"
                        },
                        new
                        {
                            SettingTypeId = 11,
                            InputControlType = "month",
                            Name = "month"
                        },
                        new
                        {
                            SettingTypeId = 12,
                            InputControlType = "number",
                            Name = "number"
                        },
                        new
                        {
                            SettingTypeId = 13,
                            InputControlType = "password",
                            Name = "password"
                        },
                        new
                        {
                            SettingTypeId = 14,
                            InputControlType = "radio",
                            Name = "radio"
                        },
                        new
                        {
                            SettingTypeId = 15,
                            InputControlType = "range",
                            Name = "range"
                        },
                        new
                        {
                            SettingTypeId = 16,
                            InputControlType = "reset",
                            Name = "reset"
                        },
                        new
                        {
                            SettingTypeId = 17,
                            InputControlType = "search",
                            Name = "search"
                        },
                        new
                        {
                            SettingTypeId = 18,
                            InputControlType = "submit",
                            Name = "submit"
                        },
                        new
                        {
                            SettingTypeId = 19,
                            InputControlType = "tel",
                            Name = "tel"
                        },
                        new
                        {
                            SettingTypeId = 20,
                            InputControlType = "time",
                            Name = "time"
                        },
                        new
                        {
                            SettingTypeId = 21,
                            InputControlType = "url",
                            Name = "url"
                        },
                        new
                        {
                            SettingTypeId = 22,
                            InputControlType = "week",
                            Name = "week"
                        },
                        new
                        {
                            SettingTypeId = 23,
                            InputControlType = "int",
                            Name = "int"
                        },
                        new
                        {
                            SettingTypeId = 24,
                            InputControlType = "float",
                            Name = "float"
                        },
                        new
                        {
                            SettingTypeId = 25,
                            InputControlType = "double",
                            Name = "double"
                        });
                });

            modelBuilder.Entity("Dievas.Models.Settings.SystemSetting", b =>
                {
                    b.Property<int>("SystemSettingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Field")
                        .HasColumnType("TEXT");

                    b.Property<int>("SettingTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("SystemSettingId");

                    b.HasIndex("SettingTypeId");

                    b.ToTable("SystemSettings");
                });

            modelBuilder.Entity("CampaignMessage", b =>
                {
                    b.HasOne("Dievas.Models.Messages.Campaign", null)
                        .WithMany()
                        .HasForeignKey("CampaignsCampaignId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dievas.Models.Messages.Message", null)
                        .WithMany()
                        .HasForeignKey("MessagesMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dievas.Models.Messages.Message", b =>
                {
                    b.HasOne("Dievas.Models.Messages.MessageType", "MessageType")
                        .WithMany()
                        .HasForeignKey("MessageTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MessageType");
                });

            modelBuilder.Entity("Dievas.Models.Settings.SystemSetting", b =>
                {
                    b.HasOne("Dievas.Models.Settings.SettingType", "SettingType")
                        .WithMany()
                        .HasForeignKey("SettingTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SettingType");
                });
#pragma warning restore 612, 618
        }
    }
}
