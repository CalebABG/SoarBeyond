﻿namespace SoarBeyond.Data.Entities;

public class MomentEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Color { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public int JournalId { get; set; }
    public JournalEntity Journal { get; set; }

    public IEnumerable<NoteEntity> Notes { get; set; }
    public IEnumerable<ReflectionEntity> Reflections { get; set; }
}