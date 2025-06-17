# 🛒 E-Ticaret Web Uygulaması

Bu proje, ASP.NET Core 9.0 ile geliştirilmiş tam kapsamlı bir e-ticaret web uygulamasıdır. Kullanıcılar ürünleri inceleyebilir, sepete ekleyebilir, ödeme yapabilir ve sipariş oluşturabilir. Yönetici paneli sayesinde ürün, kategori, kullanıcı ve sipariş işlemleri kolayca yönetilebilir.

---

## 🔧 Proje Özellikleri

✅ Ürün listeleme ve detay sayfaları  
✅ Sepet ve sipariş yönetimi  
✅ Güvenli ödeme işlemleri (İyzico sandbox ile test edilmiştir)  
✅ Kullanıcı kayıt, giriş ve rol bazlı panel erişimleri  
✅ E-posta doğrulama ve şifre sıfırlama işlemleri  
✅ Yönetici paneli (ürün, kategori, kullanıcı ve sipariş yönetimi)

---

## 🛠️ Kullanılan Teknolojiler

### Backend:

- 🔹 ASP.NET Core 9.0 (MVC mimarisi)
- 🔹 Entity Framework Core 9.0 (Code First yaklaşımı)
- 🔹 ASP.NET Core Identity (Rol bazlı kimlik doğrulama ve yetkilendirme)

### Frontend:

- 🔹 Razor Pages & ViewComponents ile modüler yapı
- 🔹 Bootstrap 5, HTML5, CSS3, JavaScript

### Veritabanı:

- 🔹 Microsoft SQL Server (MSSQL)

### Entegrasyonlar:

- 🔹 💳 İyzico ile ödeme sistemi entegrasyonu
- 🔹 📧 SMTP üzerinden e-posta gönderimi (üyelik işlemleri ve şifre sıfırlama)

---

## 🚀 Kurulum ve Çalıştırma

1. Reposu klonlayın:
   ```bash
   git clone https://github.com/ferhatkymz/e-commerce-aspnetcore.git
   cd e-commerce-aspnetcore
   ```
