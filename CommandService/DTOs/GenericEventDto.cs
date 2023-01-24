using System.ComponentModel.DataAnnotations;

namespace DotNet.CommandService.DTOs;

public class GenericEventDto
{
    [Required]
    public string? Event { get; set; }
}