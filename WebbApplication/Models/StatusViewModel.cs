namespace WebbApplication.Models;

public class StatusViewModel
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;
    
    
    public int ProjectCount { get; set; } /* Genererat av Chat GPT för att visa hur många 
    projekt som tillhör varje status */
}