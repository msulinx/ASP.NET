namespace Business.Results;

/* För denna del har result för varje del av projektet delats upp i 2 klasser.
 xxxResult används för att hämta ett enskilt objekt. 
 xxxListResult används för att hämta en lista av flera objekt */
public class ServiceResult
{
    public bool Succeeded { get; set; }
    
    public int StatusCode { get; set; }
    
    public string? Error { get; set; }
}