﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Laudatio.Core.Entities;

[Table("users")]
public class User
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string AvatarUrl { get; set; }
    public required string Bio { get; set; }

    public IEnumerable<Kudo>? KudosSent { get; set; }
    public IEnumerable<Kudo>? KudosReceived { get; set; }
    public IEnumerable<Comment>? CommentsSent { get; set; }
}
