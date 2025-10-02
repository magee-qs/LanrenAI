using Microsoft.AspNetCore.Hosting;
using OpenAuth;
using OpenAuth.ComfyUI;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions 
        {
            Args = args,
            //EnvironmentName = Environments.Production
        });
         
        BootStrapper.Start(builder);
    }
}