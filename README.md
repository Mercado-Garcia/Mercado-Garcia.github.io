# Event Invitations - .NET 10 Blazor WASM Event Invitation App

[![.NET CI](https://github.com/Andrick-Mercado/Alex-Rocio-Wedding/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/Andrick-Mercado/Alex-Rocio-Wedding/actions/workflows/dotnet-ci.yml)
[![.NET CD](https://github.com/Andrick-Mercado/Alex-Rocio-Wedding/actions/workflows/dotnet-cd.yml/badge.svg)](https://github.com/Andrick-Mercado/Alex-Rocio-Wedding/actions/workflows/dotnet-cd.yml)
[![GitHub issues](https://img.shields.io/github/issues/Andrick-Mercado/Alex-Rocio-Wedding)](https://github.com/Andrick-Mercado/Alex-Rocio-Wedding/issues)

This is a modern, data-driven event invitation application built with **.NET 10** and **Blazor WebAssembly**. While specifically configured for a wedding event, the architecture is designed to be easily customizable via a central JSON configuration file.

## 🚀 Live Demo

Check out the live application: [Event Invitation Demo](https://andrick-mercado.github.io/Alex-Rocio-Wedding/)

## ✨ Key Features

-   **Interactive Countdown:** Real-time countdown to the main event.
-   **Data-Driven Architecture:** All website text, images, and events are managed through a single `websiteData.json` file.
-   **RSVP Integration:** Seamless guest confirmation via [Formspree](https://formspree.io/) and hCaptcha.
-   **Responsive Design:** Optimized for mobile, tablet, and desktop using **MudBlazor**.
-   **Dark Mode & Theming:** Native support for dark mode and customizable color themes (Green, Blue, etc.).
-   **Event Management:** Detailed sections for multiple events (Ceremony, Reception, Party) with Google Maps integration.
-   **Performance Optimized:** Prerendered using `react-snap` for improved SEO and near-instant initial load.

## 🛠️ Tech Stack

-   **Frontend:** [Blazor WebAssembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) (.NET 10)
-   **UI Component Library:** [MudBlazor](https://mudblazor.com/)
-   **State Management:** [Blazored LocalStorage](https://github.com/Blazored/LocalStorage)
-   **Testing:**
    -   **xUnit:** Unit testing framework.
    -   **bUnit:** Component testing library for Blazor.
    -   **FluentAssertions:** Descriptive assertions for cleaner tests.
    -   **Moq:** Mocking library for dependency isolation.
-   **DevOps:**
    -   **GitHub Actions:** Automated CI/CD pipelines.
    -   **GitHub Pages:** Fast and secure static site hosting.
    -   **react-snap:** Static site prerendering for Blazor WASM.

## 📂 Project Structure

-   `src/EventInvitations.Blazor`: The main WebAssembly project containing pages, layouts, and assets.
-   `src/EventInvitations.Library`: Shared domain models, DTOs, and reusable UI components.
-   `src/EventInvitations.Tests`: Comprehensive test suite covering both logic and UI components.
-   `src/EventInvitations.Blazor/wwwroot/database/websiteData.json`: The central configuration file for all application content.