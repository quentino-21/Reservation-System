using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LogoutModel : PageModel
{
    public IActionResult OnPost()   
    {
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        return RedirectToPage("/Index");
    }
}