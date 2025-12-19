# Dokumentacja techniczna – FitnessApp

## 1. Opis projektu
**FitnessApp** - aplikacja webowa typu **ASP.NET Core MVC**, przeznaczona do zarządzania operatorami systemu fitness oraz ich rolami.  
Projekt wykorzystuje **Entity Framework Core** jako warstwę dostępu do danych oraz **SQLite** jako bazę danych.

Aplikacja została zaprojektowana w architekturze warstwowej, co ułatwia jej rozwój, testowanie oraz utrzymanie.

---

## 2. Stos technologiczny
- **Backend:** ASP.NET Core MVC (.NET)
- **ORM:** Entity Framework Core
- **Baza danych:** SQLite
- **Frontend:** Razor Views, HTML5, CSS3
- **JavaScript:** jQuery, jQuery Validation
- **Architektura:** MVC + Services
- **Kontrola wersji:** Git

---

## 3. Architektura aplikacji

### 3.1 Wzorzec MVC
Aplikacja została oparta na wzorcu **Model–View–Controller**:

- **Model** - reprezentuje dane i logikę domenową
- **View** - odpowiada za warstwę prezentacji
- **Controller** - obsługuje żądania HTTP i koordynuje działanie aplikacji

---
## 4. Baza danych

### 4.1 Technologia
- SQLite
- Entity Framework Core


### 4.2 Struktura tabel

![Diagram schematu bazy danych](wwwroot/images/readme/DBSchema.png)

## 5. Moduły funkcjonalne

### 5.1 Zarządzanie rolami operatorów
Funkcjonalności:
- Wyświetlanie listy ról
- Dodawanie nowych ról
- Walidacja danych
- Usuwanie ról
- Dodawanie nowych artykułów

---

## 6. Konfiguracja aplikacji

### 6.1 appsettings.json
Plik zawiera:
- konfigurację połączenia z bazą SQLite
- ustawienia środowiskowe aplikacji

### 6.2 Program.cs
- Rejestracja kontrolerów MVC
- Konfiguracja Dependency Injection
- Konfiguracja Entity Framework Core

---

## 7. Uruchomienie projektu

1. Rozpakuj projekt
2. Otwórz w Visual Studio / Rider / VS Code
3. Przywróć pakiety NuGet
4. Wykonaj migracje bazy danych: ```dotnet ef database update```
5. wpisz w url ```debug/add-roles``` - doda to przykłądowe role 
6. wpisz w url ```debug/create-admin``` - doda administratora z danymi logowania ```mail admin@admin.pl ``` oraz ```hasło: admin```
