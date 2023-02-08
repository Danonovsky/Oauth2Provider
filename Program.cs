using Oauth2Provider.Extensions;
using Oauth2Provider.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ICodeStoreService, CodeStoreService>();
builder.Services.AddScoped<IAuthorizeResultService, AuthorizeResultService>();
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddSession();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.MapControllers();
app.MapRazorPages();
app.UseSession();
app.ConfigureExceptionHandler();
//middleware for auto adding token to header
app.Use(async (context, next) => {
    var token = context.Session.GetString("Token");
    if(string.IsNullOrEmpty(token) is false) {
        context.Request.Headers.Add("Authorization",$"Bearer {token}");
    }
    await next();
});

app.Run();
