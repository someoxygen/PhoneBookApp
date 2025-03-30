
```md
# 📱 PhoneBook Microservices Uygulaması

Bu proje, basit bir telefon rehberi uygulamasının microservice mimarisi ile geliştirilmiş halidir. 
İki servis birbirinden bağımsızdır ve Kafka üzerinden haberleşir. Veritabanı olarak MongoDB kullanılmıştır.

---

## 🧱 Proje Yapısı

- **ContactService**: Kişi ve iletişim bilgilerini yöneten servis
- **ReportService**: Lokasyon bazlı rapor oluşturan servis (Kafka ile tetiklenir)
- **Shared**: Ortak event ve model yapıları

---

## ⚙️ Kullanılan Teknolojiler

- .NET 8
- ASP.NET Core Web API
- MongoDB
- Kafka (Confluent Platform)
- xUnit & Moq (Unit Test)
- Docker + Docker Compose
- FluentAssertions
- Coverlet (Code Coverage)
- ReportGenerator (HTML Coverage Raporu)

---

## 🚀 Kurulum ve Çalıştırma

### 1. Gerekli Araçlar

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Kafka (Docker ile)

### 2. Docker ile Kafka & MongoDB Başlatma

```bash
docker-compose up -d
```

### 3. Servisleri Çalıştırma

Her bir servisi ayrı terminalde şu komutlarla başlat:

```bash
cd ContactService.API
dotnet run

cd ReportService.API
dotnet run
```

---

## 📬 Rapor Talebi Akışı

1. `/api/Reports/request-report` endpoint’i çağrılır
2. Kafka’ya `ReportRequestedEvent` gönderilir
3. `ReportRequestedConsumer` mesajı alır ve MongoDB’ye rapor verisini yazar
4. Raporun durumu `Completed` olduğunda `/api/Reports` üzerinden görüntülenebilir

---

## ✅ Unit Test & Coverage

Testleri çalıştırmak için:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

HTML coverage raporu üretmek için:

```bash
reportgenerator "-reports:**/coverage.cobertura.xml" "-targetdir:coverage-report" -reporttypes:Html
```

Raporu `coverage-report/index.html` üzerinden açabilirsiniz.

---

## 📁 API Endpoint’leri

### ContactService

- `GET /api/person` – Tüm kişileri getirir
- `POST /api/person` – Yeni kişi ekler
- `DELETE /api/person/{id}` – Kişiyi siler
- `POST /api/contact-information` – Kişiye iletişim bilgisi ekler

### ReportService

- `POST /api/reports/request-report` – Yeni rapor oluşturma isteği
- `GET /api/reports` – Tüm raporları getir
- `GET /api/reports/{id}` – Belirli bir raporun detayları

---

## 📂 Proje Klasör Yapısı

```
├── ContactService
│   ├── API
│   ├── Application
│   ├── Domain
│   ├── Infrastructure
│
├── ReportService
│   ├── API
│   ├── Application
│   ├── Domain
│   ├── Infrastructure
│
├── Shared
├── docker-compose.yml
└── README.md
```

---

## 👨‍💻 Geliştiren

- [Mustafa Yücel]  
- [https://github.com/someoxygen]  
- İletişim: [mustafaycl37@gmail.com]
```
