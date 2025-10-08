using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// CORS Politikas� Tan�mlama
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IHttpClientFactory i�in ekleme
builder.Services.AddHttpClient();

// 2. CORS servisini ekleyin
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // **Geli�tirme ve Test A�amas�nda:** T�m origin'lere izin veriyoruz.
                          // Render'a y�kledikten sonra, g�venlik i�in buray� Frontend URL'lerinizle k�s�tlayabilirsiniz.
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

// 4. CORS kullan�m�n� etkinle�tirin
// Bu sat�r, UseAuthorization'dan �nce olmal�d�r.
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
