using System;
using System.Collections.Generic;

namespace SoarBeyond.Shared.Dto;

public class JournalEntry
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public List<Thought> Thoughts { get; set; } = new();
    public int JournalId { get; set; }

    public int UserId { get; set; }
}