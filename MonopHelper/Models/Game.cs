using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MonopHelper.Models;

public class Game
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    
    [MaxLength(50)]
    public required string GameName { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
    
    public virtual List<Player> Players { get; set; }
}