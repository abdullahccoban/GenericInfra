📖 GenericInfra: Generic Unit of Work ve Repository Pattern
🌟 Proje Hakkında
GenericInfra, .NET 8 ve Entity Framework Core projeleri için tasarlanmış, temiz ve yeniden kullanılabilir bir Veri Erişim Katmanı (DAL) altyapısıdır.

Bu paket, Unit of Work (İş Birimi) ve Generic Repository (Genel Havuz) tasarım kalıplarını uygulayarak, iş mantığı katmanınızı (Servisler) ORM (EF Core) detaylarından soyutlar, kod tekrarını azaltır ve veri tutarlılığını (atomic transactions) garanti eder.

Temel Özellikler
Veri Soyutlama: İş mantığını veritabanı işlemlerinden ayırır.

Atomik İşlemler: Tüm CRUD işlemlerini tek bir veritabanı işlemi (CommitAsync()) altında toplar.

Bağımlılık Enjeksiyonu (DI) Hazır: .NET'in yerleşik DI konteyneri ile kolay entegrasyon için Extension metotları içerir.

Tekrarsız Kod: Tüm varlıklar için temel CRUD operasyonlarını otomatik sağlar.

📦 Kurulum
Bu paketi projenize eklemek için NuGet Paket Yöneticisi'ni (Package Manager) kullanabilirsiniz.

.NET CLI (Terminal)
Bash

dotnet add package GenericInfra --version 1.0.0 
# Veya en son sürüm için --version kısmını kaldırabilirsiniz.
NuGet Package Manager (Visual Studio / Rider)
PowerShell

Install-Package GenericInfra -Version 1.0.0
🚀 Kullanım (Hızlı Başlangıç)
Paketi kullanmaya başlamak için iki temel adım vardır: Veritabanı Bağlamını (DbContext) kaydetmek ve ardından AddUnitOfWork extension metodunu kullanmak.

Adım 1: Projenizde DbContext Oluşturma
Paketi kullanacak olan uygulamanızda, DbContext sınıfınızı tanımlayın. (Paketiniz bu sınıfa generic olarak bağlanacaktır.)

C#

// Infrastructure/Context/MyProjectDbContext.cs

public class MyProjectDbContext : DbContext
{
    // MyEntity, IEntity arayüzünden miras almalıdır.
    public DbSet<MyEntity> Entities { get; set; } 
    
    public MyProjectDbContext(DbContextOptions<MyProjectDbContext> options) : base(options) { }
}
Adım 2: Program.cs Dosyasında Servisleri Yapılandırma
GenericInfra paketi, IUnitOfWork arayüzünü sizin oluşturduğunuz DbContext ile eşleştirmek için bir extension metodu sunar.

C#

// Program.cs

using GenericInfra.Extensions; // Extension metotları için

// ...
builder.Services.AddDbContext<MyProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Unit of Work'ü projeye özgü DbContext ile kaydediyoruz.
builder.Services.AddUnitOfWork<MyProjectDbContext>();
// ...
Adım 3: İş Katmanında Kullanım (Services)
İş mantığı katmanınız (Servisler), sadece IUnitOfWork arayüzünü inject ederek tüm veri işlemlerini yapabilir.

C#

// Services/MyBusinessService.cs

public class MyBusinessService
{
    private readonly IUnitOfWork _uow;

    public MyBusinessService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task UpdateAndLog(int entityId, string newName)
    {
        // 1. Repository'ye erişim
        var repository = _uow.GetRepository<MyEntity>();
        
        // 2. İşlemler
        var entity = await repository.GetByIdAsync(entityId);
        if (entity == null) return;
        
        entity.Name = newName;
        repository.Update(entity); 
        
        // 3. Atomik Kayıt (Unit of Work)
        // Tüm değişiklikler tek bir veritabanı işlemi olarak kaydedilir.
        await _uow.CommitAsync(); 
    }
}
🤝 Katkıda Bulunma
Geri bildirim ve katkılarınız her zaman açığız! Lütfen bir Issue açmaktan veya Pull Request göndermekten çekinmeyin.

📄 Lisans
Bu proje MIT Lisansı altında yayınlanmıştır. Detaylar için [LICENSE] dosyasına bakınız.
