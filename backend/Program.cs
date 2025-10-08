using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS Politikasý Tanýmlama
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IHttpClientFactory için ekleme
builder.Services.AddHttpClient();

// 2. CORS servisini ekleyin
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // **Geliþtirme ve Test Aþamasýnda:** Tüm origin'lere izin veriyoruz.
                          // Render'a yükledikten sonra, güvenlik için burayý Frontend URL'lerinizle kýsýtlayabilirsiniz.
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. CORS kullanýmýný etkinleþtirin
// Bu satýr, UseAuthorization'dan önce olmalýdýr.
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
