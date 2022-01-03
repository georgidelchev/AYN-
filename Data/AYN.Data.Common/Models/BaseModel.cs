using System;
using System.ComponentModel.DataAnnotations;

namespace AYN.Data.Common.Models;

public abstract class BaseModel<TKey> : IAuditInfo
{
    [Key]
    public TKey Id { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
