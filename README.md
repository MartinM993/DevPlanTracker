# 🚀 Capability Development Tracker (DevPlanTracker)

A professional, client-side Blazor WebAssembly application designed to track engineering capability development, specifically focusing on Continuous Delivery and Delivery Excellence.

## ✨ Features
* **100% Client-Side:** Runs entirely in the browser using Blazor WebAssembly (.NET 10). No backend server required!
* **Offline Capable:** Data is securely saved to the browser's `LocalStorage` using a custom `TaskStorageService`.
* **Rich Text Editing:** Built-in rich text editor for tracking detailed notes, evidence, and helpful links.
* **Data Portability:** Export your entire tracker as a JSON backup, and import it to any other device.
* **Automated CI/CD:** Fully integrated GitHub Actions pipeline for automated xUnit testing and free deployment to GitHub Pages.

## 🛠️ Architecture
This project demonstrates modern .NET architecture principles:
* **Dependency Injection:** UI components are decoupled from data logic via injected Services.
* **Component Isolation:** Clean separation between the `Home.razor` dashboard and reusable `TaskCard.razor` UI elements.
* **Unit Testing:** Includes a mocked `xUnit` test suite using `Moq` to verify data service integrity without a browser.

## 🚀 How to Run Locally

1. Clone the repository:
   ```bash
   git clone [https://github.com/YOUR-USERNAME/YOUR-REPO-NAME.git](https://github.com/YOUR-USERNAME/YOUR-REPO-NAME.git)