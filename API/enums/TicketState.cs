
namespace API.Enums
{

  public enum TicketState
  {
    New = 1,
    Pending = 2,
    Solved = 3,
    Deleted = 9
  }

  public class TicketStateHelper
  {
    public static Dictionary<string, string> GetTicketStatesDict()
    {
      var dict = new Dictionary<string, string>
      {
        {"0", "Error (delete this)"},
        {"1", "Yeni"},
        {"2", "Beklemede"},
        {"3", "Çözüldü"},
        {"9", "Silindi"}
      };

      return dict;
    }
  }

}