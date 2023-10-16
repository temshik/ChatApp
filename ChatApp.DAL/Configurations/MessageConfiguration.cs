﻿using ChatApp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.Property(s => s.Content).IsRequired().HasMaxLength(500);

            builder.HasOne(s => s.ToRoom)
                .WithMany(m => m.Messages)
                .HasForeignKey(s => s.ToRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.FromUser)
                .WithMany(m => m.Messages)
                .HasForeignKey(s => s.FromUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
